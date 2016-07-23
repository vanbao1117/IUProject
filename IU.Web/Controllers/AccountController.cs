using IU.Web.Models;
using IU.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace IU.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationManager _auth;

        public AccountController(IAuthenticationManager auth)
        {
            this._auth = auth;
        }

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            using (UserService _userService = new UserService())
            {
                if (_userService.CheckExistUser(model))
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.UserName), }, DefaultAuthenticationTypes.ApplicationCookie);

                    this._auth.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    }, identity);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this._auth.SignOut();
            return RedirectToAction("Login", "Account");
        }

    }
}