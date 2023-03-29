using System;
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

        [HttpPost]
        public IActionResult SendPasswordResetEmail(string email)
        {
            var forgotPasswordServices = new ForgotPasswordServices();
            forgotPasswordServices.SendPasswordResetEmail(email);
            return View();
        }

        // Méthode "Index" appelée lorsqu'un utilisateur accède à la page de connexion
        public IActionResult Index()
        {            
            UserViewModel viewModel = new UserViewModel { Authenticated = HttpContext.User.Identity.IsAuthenticated };

            if (viewModel.Authenticated)
            {
                // Si l'user est déjà connecté, récupère ses données et affiche la page d'accueil correspondante
                viewModel.User = dal.GetUser(HttpContext.User.Identity.Name);
                return View(viewModel);
            }
            // Sinon, affiche la page de connexion
            return View(viewModel);
        }

        // Méthode "Index" appelée lorsqu'un user soumet le formulaire de connexion
        [HttpPost]
        public IActionResult Index(UserViewModel viewModel, string returnUrl)
        {
            // Vérifie si le modèle est valide
            if (ModelState.IsValid)
            {
                // Authentifie l'utilisateur avec le nom d'utilisateur et le mot de passe fournis dans le modèle
                User user = dal.Authentifier(viewModel.User.Login, viewModel.User.Password);

                // Vérifie si l'utilisateur existe
                if (user != null)
                {
                    // Vérifie si l'utilisateur a le rôle correspondant à celui sélectionné dans le formulaire
                    if (!dal.checkUserRole(user.Id, viewModel.UserRoleViewModel.SelectedRole))
                    {
                        // Ajoute une erreur de modèle si l'utilisateur n'a pas le rôle sélectionné
                        ModelState.AddModelError("UserRoleViewModel.SelectedRole", "Le rôle sélectionné ne correspond pas");
                        return View(viewModel);
                    }

                    // Crée un cookie pour l'utilisateur
                    var userClaims = new List<Claim>()
            {
                // Ajoute une claim de nom d'utilisateur à l'identité de l'utilisateur
                new Claim(ClaimTypes.Name, user.Id.ToString()),
            };

                    // Récupère tous les rôles de l'utilisateur et les ajoute à l'identité de l'utilisateur
                    List<RoleUser> roles = dal.GetAllRolesById(user.Id);

                    foreach (RoleUser roleUser in roles)
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, roleUser.RoleType.ToString()));
                    }

                    // Crée une identité pour l'utilisateur avec les claims récupérées
                    var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");

                    var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });

                    HttpContext.SignInAsync(userPrincipal);

                    // Redirige l'user vers la page d'accueil correspondant à son rôle.
                    switch (viewModel.UserRoleViewModel.SelectedRole)
                    {
                        case RoleTypeEnum.ADMIN:
                            return RedirectToAction("Index", "Admin");
                        case RoleTypeEnum.SALARIE:
                            return RedirectToAction("Index", "Salarie");
                        case RoleTypeEnum.MANAGER:
                            return RedirectToAction("index", "Manager");
                    }

                    // Si l'user n'a pas de rôle, le redirige vers la page de connexion
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Login");
                }
                // Si le nom d'utilisateur ou le mot de passe est incorrect, affiche le message d'erreur
                ModelState.AddModelError("User.Login", "Login et/ou mot de passe incorrect(s)");
            }
            // Si le formulaire n'est pas valide, affiche la page de connexion avec les erreurs
            return View(viewModel);
        }

        // Méthode de déconnexion
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }

        // ne fonctionne pas
        public ActionResult ResetPassword()
        {
            HttpContext.SignOutAsync();
            return View();
        }

    }
}
