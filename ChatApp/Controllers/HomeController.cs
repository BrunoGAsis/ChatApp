using ChatApp.Models;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orchestration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMessageManager _msgManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IMessageManager msgManager)
        {
            _logger = logger;
            _userManager = userManager;
            _msgManager = msgManager;
        }

        public async Task<IActionResult> Index()
        {
            IdentityUser currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                ViewBag.CurrentUserEmail = currentUser.Email;
                ViewBag.CurrentUserName = currentUser.UserName;
            }
            return View();
        }

        public IActionResult GetAvailableMessages()
        {
            return Json(_msgManager.GetAvailableMessages());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
