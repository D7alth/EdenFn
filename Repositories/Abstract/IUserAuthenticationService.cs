using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eden_Fn.Models.Domain;
using Eden_Fn.Models.DTO;

namespace Eden_Fn.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);
        Task<Status> UpdateAsync(ApplicationUser model, string id);
        Task LogoutAsync();
        Task<Status> RegisterAsync(RegistrationModel model);
        Task<Status> ChangePasswordAsync(ChangePassword model, string username);
    }
}
