using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Eden_Fn.Models.Domain;
using Eden_Fn.Models.DTO;
using Eden_Fn.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Eden_Fn.Models;
using System.Text;

namespace Eden_Fn.Repositories.Implementation
{
    public class UserAuthenticationService: IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public UserAuthenticationService(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager; 

        }
        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "O utilizador já existe";
                return status;
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FrameLink = model.FrameLink,
                EmailConfirmed=true,
                PhoneNumberConfirmed=true,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Falha na criação do usuário";
                return status;
            }

            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));
            

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "Você se registrou com sucesso";
            return status;
        }

         public async Task<Status> UpdateAsync(ApplicationUser model, string id)
        {
            var status = new Status();
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Usuário inexistente";
                return status;
            }

            if (!string.IsNullOrWhiteSpace(model.Email) && user.Email != model.Email)
            {
                user.Email = model.Email;
                user.EmailConfirmed = false;
                var setEmailResult = await userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    status.StatusCode = 0;
                    status.Message = "Erro ao atualizar o e-mail do usuário.";
                    return status;
                }
            }

            if (!string.IsNullOrWhiteSpace(model.UserName) && user.UserName != model.UserName)
            {
                user.UserName = model.UserName;
                var setUsernameResult = await userManager.SetUserNameAsync(user, model.UserName);
                if (!setUsernameResult.Succeeded)
                {
                    status.StatusCode = 0;
                    status.Message = "Erro ao atualizar o nome de usuário do usuário.";
                    return status;
                }
            }

            if (!string.IsNullOrWhiteSpace(model.FrameLink) && user.FrameLink != model.FrameLink)
            {
                user.FrameLink = model.FrameLink;
            }

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Erro ao atualizar o usuário.";
                return status;
            }

            status.StatusCode = 1;
            status.Message = "Usuário atualizado com sucesso.";
            return status;
        }

        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Nome de usuário Inválido";
                return status;
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "senha inválida";
                return status;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Conectado com sucesso";
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "O usuário está bloqueado";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Erro ao logar";
            }
           
            return status;
        }

        public async Task LogoutAsync()
        {
           await signInManager.SignOutAsync();
           
        }

        public async Task<Status> ChangePasswordAsync(ChangePassword model,string username)
        {
            var status = new Status();
            
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.Message = "Usuário não existe";
                status.StatusCode = 0;
                return status;
            }
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "A senha foi atualizada com sucesso";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Ocorreu algum erro";
                status.StatusCode = 0;
            }
            return status;

        }
        
    }
}