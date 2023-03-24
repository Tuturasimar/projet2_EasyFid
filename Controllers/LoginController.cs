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
            if (ModelState.IsValid)
            {
                // Vérifie que le nom d'user et le mot de passe sont corrects
                User user = dal.Authentifier(viewModel.User.Login, viewModel.User.Password);
                if (user != null)
                {
                    // Vérifie que l'user a le rôle correspondant à celui sélectionné dans le formulaire
                    if (!dal.checkUserRole(user.Id, viewModel.UserRoleViewModel.SelectedRole))
                    {
                        ModelState.AddModelError("UserRoleViewModel.SelectedRole", "Le rôle sélectionné ne correspond pas au rôle assigné à cet utilisateur.");
                        return View(viewModel);
                    }

                    // Crée un cookie pour l'user
                    var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                };

                    var ClaimIdentity = new ClaimsIdentity(userClaims, "User Identity");

                    var userPrincipal = new ClaimsPrincipal(new[] { ClaimIdentity });

                    HttpContext.SignInAsync(userPrincipal);

                    // Redirige l'user vers la page d'accueil correspondant à son rôle.
                    switch (viewModel.UserRoleViewModel.SelectedRole)
                    {
                        case RoleTypeEnum.ADMIN:
                            return RedirectToAction("Index", "Admin");
                        case RoleTypeEnum.SALARIE:
                            return RedirectToAction("IndexSalarie", "Salarie");
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
    }
}
