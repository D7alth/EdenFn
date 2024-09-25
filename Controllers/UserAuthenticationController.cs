using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Eden_Fn.Models.DTO;
using Eden_Fn.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Eden_Fn.Controllers
{
    [Authorize]

    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _authService;
        public UserAuthenticationController(IUserAuthenticationService authService)
        {
            this._authService = authService;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _authService.LoginAsync(model);
            
            if(result.StatusCode == 1 && User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if(result.StatusCode==1 && User.IsInRole("user"))
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }

        public async Task<IActionResult> Logout()
        {
            await this._authService.LogoutAsync();  
            return RedirectToAction(nameof(Login));
        }
        // [AllowAnonymous]
        // public async Task<IActionResult> RegisterAdmin()
        // {
        //    RegistrationModel model = new RegistrationModel
        //    {
        //        Username="admin",
        //        Email="admin@gmail.com",
        //        Password="Admin@12345#",
        //        FrameLink = "http://localhost"
        //    };
        //    model.Role = "admin";
        //    var result = await this._authService.RegisterAsync(model);
        //    return Ok(result);
        // }


        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
              return View(model);
            var result = await _authService.ChangePasswordAsync(model, User.Identity.Name);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(ChangePassword));
        }

    }
}