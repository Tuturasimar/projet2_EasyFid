﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projet2_EasyFid.Data;
using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;
using Projet2_EasyFid.ViewModels;

namespace Projet2_EasyFid.Controllers
{
    public class LoginController : Controller
    {
        private Dal dal;
        public LoginController()
        {
            dal = new Dal();
        }
        public IActionResult Index()
        {
            UserViewModel viewModel = new UserViewModel { Authenticated = HttpContext.User.Identity.IsAuthenticated };
            if (viewModel.Authenticated)
            {
                viewModel.User = dal.GetUser(HttpContext.User.Identity.Name);
                return View(viewModel);
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(UserViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = dal.Authentifier(viewModel.User.Login, viewModel.User.Password);
                if (user != null)
                {
                    if (!dal.checkUserRole(user.Id, viewModel.UserRoleViewModel.SelectedRole))
                    {
                        return View(viewModel);
                    }
                    var userClaims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, user.Id.ToString())
                        };

                    var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");

                    var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });

                    HttpContext.SignInAsync(userPrincipal);

                    switch (viewModel.UserRoleViewModel.SelectedRole)
                    {
                        case RoleTypeEnum.ADMIN:
                            return RedirectToAction("Index", "Admin");
                        case RoleTypeEnum.SALARIE:
                            return RedirectToAction("IndexSalarie", "Salarie");
                        case RoleTypeEnum.MANAGER:
                            return RedirectToAction("index", "Manager");
                    }

                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Login");
                }
                ModelState.AddModelError("User.Login", "Login et/ou mot de passe incorrect(s)");
            }
            return View(viewModel);
        }


        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
    }
}