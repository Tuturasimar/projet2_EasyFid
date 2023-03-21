using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Projet2_EasyFid.Data;
using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;
using Projet2_EasyFid.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet2_EasyFid.Controllers
{
    // Controller qui gère les méthodes pour l'Administrateur (require authentificateur Admin)
    public class AdminController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            using (Dal dal = new Dal())
            {
                // On récupère tous les utilisateurs pour les stocker dans une liste
                List<User> users = dal.GetAllUsers();
                UserListViewModel userList = new UserListViewModel { Users = users };
                return View(userList);
            }
        }

        public IActionResult UserDetail(int id)
        {
            using (Dal dal = new Dal())
            {
                // On récupère l'utilisateur en fonction de son id
                User user = dal.GetUserById(id);
                // Si l'utilisateur existe en BDD
                if(user != null)
                {
                    List<RoleUser> rolesUser = dal.GetAllRolesById(id);
                    UserRoleViewModel urvm = new UserRoleViewModel { User = user, RolesUser= rolesUser};
                    // Envoi en paramètre à la vue UserDetail
                    return View(urvm);
                }
                // Si l'utilisateur n'existe pas, redirection vers l'Index Admin
                return RedirectToAction("Index");
            }
        }

        public IActionResult ModifyUser(int id)
        {
            using (Dal dal = new Dal())
            {
                    User user = dal.GetUserById(id);
                    if (user != null)
                    {
                  //  List<RoleUser> rolesUser = dal.GetAllRolesById(id);
                    UserRoleViewModel urvm = new UserRoleViewModel { User = user };
                    return View(urvm);
                    }
                
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        // Une fois qu'on appuie sur le bouton du formulaire, cette méthode récupère un objet user
        public IActionResult ModifyUser(User user)
        {
            using (Dal dal = new Dal())
            {
                // On récupère l'ensemble des données renseignées pour cet utilisateur en BDD grâce à une requête
                User oldUser = dal.GetUserById(user.Id);
                // On remplace un par un l'ensemble des champs du formulaire
                oldUser.Login = user.Login;
                oldUser.UserData.Firstname = user.UserData.Firstname;
                oldUser.UserData.Lastname = user.UserData.Lastname;
                oldUser.UserData.Birthday = user.UserData.Birthday;
                //oldUser.UserData.Email = user.UserData.Email;
                // On envoie l'ancien user maintenant modifié à la méthode pour confirmer les changements dans la BDD
                dal.ModifyUser(oldUser);
            }
            // Une fois que c'est réalisé, on redirige vers la vue UserDetail avec un id en paramètre.
            // On va donc sur la page des détails de l'utilisateur qu'on vient de modifier.
            return RedirectToAction("UserDetail", new { @id = user.Id });
        }

        public IActionResult CreateUser()
        {
            using(Dal dal = new Dal())
            {
                List<Company> companies = dal.GetAllCompanies();
                ViewBag.companies = companies;
            }
            return View();
        }

        [HttpPost]
        // Une fois qu'on appuie sur le bouton du formulaire, cette méthode récupère un objet user
        public IActionResult CreateUser(User user, List<RoleTypeEnum> roleType, int company)
        {
            using (Dal dal = new Dal())
            {
                // On crée les UserData en premier (aucune clé étrangère dans la table)
                UserData newUserData = new UserData { Firstname = user.UserData.Firstname, Lastname = user.UserData.Lastname, Birthday = user.UserData.Birthday, Email= user.UserData.Email };
                // On récupère l'ID du UserData fraichement créé pour l'utiliser dans la création du User (clé étrangère)
                int UserDataId = dal.CreateUserData(newUserData);
                // On créé un User
                User newUser = new User { Login = user.Login, Password = Dal.EncodeMD5(user.Password), CompanyId = company, CreationDate = DateTime.Now, UserDataId = UserDataId };
                int UserId = dal.CreateUser(newUser);

                // boucler sur RoleType pour instancier des roleType en fonction du nombre de role coché
                foreach(RoleTypeEnum item in roleType)
                {
                    // On le fait après avoir créé un User car on a besoin de son ID pour lier les tables User et RoleUser
                    RoleUser roleUser = new RoleUser { UserId = UserId, RoleType = item };
                    dal.CreateRoleUser(roleUser);
                }

                // Une fois que c'est réalisé, on redirige vers la vue UserDetail avec un id en paramètre.
                // On va donc sur la page des détails de l'utilisateur qu'on vient de créer
                return RedirectToAction("UserDetail", new { @id = UserId });
            }
            
        }

    }
}

