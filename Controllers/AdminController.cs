using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Eden_Fn.Models.Domain;
using Eden_Fn.Models.DTO;
using Eden_Fn.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Eden_Fn.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserAuthenticationService _authService;

        public AdminController(UserManager<ApplicationUser> userManager, IUserAuthenticationService authService)
        {
            _userManager = userManager;
            _authService = authService;

        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationUser> users = _userManager.Users.ToList();
            return View(users);
        }
          public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(RegistrationModel model)
        {
            if(!ModelState.IsValid) { return View(model); }
            model.Role = "user";
            var result = await _authService.RegisterAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ApplicationUser model, string id)
        {
            if (!ModelState.IsValid) { return View(model); }
            var result = await _authService.UpdateAsync(model, id);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Deletar(string? id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            var deleteOperation = await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Dashboard()
        {   
            var totalUser =  _userManager.Users.Count();
            return View(totalUser);
        }
    }
}
