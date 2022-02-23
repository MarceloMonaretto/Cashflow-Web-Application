using CashFlowUI.Models;
using CashFlowUI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CashFlowUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoginManager _loginManager;

        public AccountController(ILoginManager loginManager)
        {
            _loginManager = loginManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _loginManager.SignOutUserAsync();
            return RedirectToAction("Login");
        }

        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> Login(LoginViewModel login, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LoginErrorMessage = StandardMessages.LoginMessages.InvalidLoginMessage;
                return View(login);
            }

            var isUserValid = _loginManager.ValidateLoginInfo(login.User, login.Password);
            if (!isUserValid)
            {
                ViewBag.LoginErrorMessage = StandardMessages.LoginMessages.InvalidLoginMessage;
                return View(login);
            }

            await _loginManager.SignInUserAsync(login.User);
            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.LoginErrorMessage = StandardMessages.LoginMessages.InvalidLoginMessage;
                return View("Login", login);
                
            }

            return RedirectToAction("Login", "Home");
        }
    }
}
