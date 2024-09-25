using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Eden_Fn.Models.Domain;
using Eden_Fn.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Eden_Fn.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUserAuthenticationService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            UserManager<ApplicationUser> userManager,
            IUserAuthenticationService authService
        )
        {
            _userManager = userManager;
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Display()
        {
            var userName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }
            var applicationUser = new ApplicationUser() { FrameLink = user.FrameLink };

            return View(applicationUser);
        }
    }
}
