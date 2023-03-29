using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Projet2_EasyFid.Data;
using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;
using Projet2_EasyFid.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet2_EasyFid.Controllers
{
    [Authorize (Roles ="ADMIN")]
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
                if (user != null)
                {
                    List<RoleUser> rolesUser = dal.GetAllRolesById(id);
                    UserRoleViewModel urvm = new UserRoleViewModel { User = user, RolesUser = rolesUser };
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
                // Si on récupère un utilisateur existant dans la BDD
                    if (user != null)
                    {
                    // On obtient la liste des entreprises
                    List<Company> companies = dal.GetAllCompanies();
                    // On instancie une liste qui contiendra des UserData que l'on va utiliser pour la liste déroulante des managers
                    List<UserData> userDatas = new List<UserData>();
                    // On obtient la liste des roles de l'utilisateur
                    List<RoleUser> rolesUser = dal.GetAllRolesById(id);
                    // On rajoute à la liste des UserData un nouveau qui permet d'avoir l'option "Aucun manager"
                    userDatas.Add(new UserData { Lastname = "Aucun manager" });
                    userDatas.AddRange(dal.GetAllManagerUserDatas(user.Id));
                    ViewBag.companies = companies;
                    ViewBag.userDatas = userDatas;
                    ViewBag.rolesUser = rolesUser;
                    return View(user);
                    }
                // Sinon, on est redirigé vers l'index
                return RedirectToAction("Index");
            }
        }
  
        [HttpPost]
        // Une fois qu'on appuie sur le bouton du formulaire, cette méthode récupère un objet user
        public IActionResult ModifyUser(User user, List<RoleTypeEnum> roleType, int company, int manager, JobEnum jobEnum)
        {
            using (Dal dal = new Dal())
            {
                if (!ModelState.IsValid)
                {
                    List<Company> companies = dal.GetAllCompanies();
                    // On instancie une liste qui contiendra des UserData que l'on va utiliser pour la liste déroulante des managers
                    List<UserData> userDatas = new List<UserData>();
                    // On obtient la liste des roles de l'utilisateur
                    List<RoleUser> rolesUser = dal.GetAllRolesById(user.Id);
                    // On rajoute à la liste des UserData un nouveau qui permet d'avoir l'option "Aucun manager"
                    userDatas.Add(new UserData { Lastname = "Aucun manager" });
                    userDatas.AddRange(dal.GetAllManagerUserDatas(user.Id));
                    ViewBag.companies = companies;
                    ViewBag.userDatas = userDatas;
                    ViewBag.rolesUser = rolesUser;
                    return View(user);
                }
                    // On récupère l'ensemble des données renseignées pour cet utilisateur en BDD grâce à une requête
                    User oldUser = dal.GetUserById(user.Id);

                // On remplace un par un l'ensemble des champs du formulaire
                oldUser.Login = user.Login;
                oldUser.UserData.Firstname = user.UserData.Firstname;
                oldUser.UserData.Lastname = user.UserData.Lastname;
                oldUser.UserData.Birthday = user.UserData.Birthday;
                oldUser.UserData.Email = user.UserData.Email;
                oldUser.CompanyId = company;
                oldUser.JobEnum = jobEnum;

                // Si on a choisi un manager
                if(manager != 0)
                {
                    oldUser.ManagerId = manager;
                } else
                // Si on a choisi "Aucun manager"
                {
                    oldUser.ManagerId = null;
                }
               
                
                // On envoie l'ancien user maintenant modifié à la méthode pour confirmer les changements dans la BDD
                dal.ModifyUser(oldUser);

                // Partie changement des accréditations \\

                // On supprime les anciens
                dal.DeleteAllRoleUsersByUserId(user.Id);

                // On ajoute les nouveaux
                foreach (RoleTypeEnum item in roleType)
                {
                 RoleUser roleUser = new RoleUser { UserId = user.Id, RoleType = item };
                 dal.CreateRoleUser(roleUser);
                }

            }
            // Une fois que c'est réalisé, on redirige vers la vue UserDetail avec un id en paramètre.
            // On va donc sur la page des détails de l'utilisateur qu'on vient de modifier.
            return RedirectToAction("UserDetail", new { @id = user.Id });
        }

        public IActionResult CreateUser()
        {
            using(Dal dal = new Dal())
            {
                // On obtient la liste des entreprises
                List<Company> companies = dal.GetAllCompanies();
                // On instancie une liste de UserData pour ajouter au début un UserData factice "Aucun manager"
                List<UserData> userDatas = new List<UserData>();
                userDatas.Add(new UserData { Lastname="Aucun manager"});
                // On rajoute à la liste des UserData tous ceux des manager
                // On envoie 0 en argument car cette méthode attend une ID, mais l'utilisateur n'existe pas encore
                userDatas.AddRange(dal.GetAllManagerUserDatas(0));
                ViewBag.companies = companies;
                ViewBag.userDatas = userDatas;
            }
            return View();
        }

        [HttpPost]
        // Une fois qu'on appuie sur le bouton du formulaire, cette méthode récupère un objet user
        public IActionResult CreateUser(User user, List<RoleTypeEnum> roleType, int company, int manager, JobEnum jobEnum)
        {
            
            using (Dal dal = new Dal())
            {
                if(!ModelState.IsValid)
                {
                    List<Company> companies = dal.GetAllCompanies();
                    List<UserData> userDatas = new List<UserData>();
                    userDatas.Add(new UserData { Lastname = "Aucun manager" });
                    userDatas.AddRange(dal.GetAllManagerUserDatas(0));
                    ViewBag.companies = companies;
                    ViewBag.userDatas = userDatas;

                    return View(user);
                }
                // On crée les UserData en premier (aucune clé étrangère dans la table)
                UserData newUserData = new UserData {
                    Firstname = user.UserData.Firstname,
                    Lastname = user.UserData.Lastname,
                    Birthday = user.UserData.Birthday,
                    Email= user.UserData.Email };

                // On récupère l'ID du UserData fraichement créé pour l'utiliser dans la création du User (clé étrangère)
                int UserDataId = dal.CreateUserData(newUserData);

                // On créé un User
                User newUser = new User {
                    Login = user.Login,
                    Password = Dal.EncodeMD5(user.Password),
                    JobEnum = jobEnum,
                    CompanyId = company,
                    CreationDate = DateTime.Now,
                    UserDataId = UserDataId};

                // Si un manager a été choisi dans la liste
                if(manager != 0)
                {
                    // On récupère l'ID du User manager gràce à l'ID de son UserData
                    int managerId = dal.GetUserByUserDataId(manager).Id;
                    newUser.ManagerId = managerId;
                }

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

        public IActionResult DeleteUser(int id)
        {
            using (Dal dal = new Dal())
            {
                // On récupère l'utilisateur en fonction de son id
                User user = dal.GetUserById(id);
                // Si l'utilisateur existe en BDD
                if (user != null)
                {
                    List<RoleUser> rolesUser = dal.GetAllRolesById(id);
                    UserRoleViewModel urvm = new UserRoleViewModel { User = user, RolesUser = rolesUser };
                    // Envoi en paramètre à la vue UserDelete
                    return View(urvm);
                }
                // Si l'utilisateur n'existe pas, redirection vers l'Index Admin
                return RedirectToAction("Index");
            }
        }

        public IActionResult ConfirmDeleteUser(int id)
        {
            using(Dal dal = new Dal())
            {
                // On récupère le user à supprimer
                User userToDelete = dal.GetUserById(id);
                // On récupère l'ensemble de ses CRA
                List<Cra> crasFromUser = dal.GetAllCrasByUserId(id);

                // On boucle sur chacun d'entre eux
                foreach(Cra cra in crasFromUser)
                {
                    // On change l'IdUser de chaque CRA en null
                    dal.SetUserIdNullOnDelete(cra);
                }

                // On récupère la liste des User qui avaient pour Manager la personne à supprimer
                List<User> usersManaged = dal.GetAllUsersByManagerId(id);
                // On boucle sur chacun d'entre eux
                foreach(User user in usersManaged)
                {
                    // On change l'IdManager de chaque User en null
                    dal.SetManagerIdNullOnDelete(user);
                }

                // On supprime les accréditations liées à l'utilisateur
                dal.DeleteAllRoleUsersByUserId(id);
                // On supprime le UserData de l'utilisateur à supprimer. Cela déclenche aussi la suppression du User
                dal.DeleteUserDataById(userToDelete.UserDataId);

            }

            return RedirectToAction("Index");
        }

        public IActionResult ModifyPassword(int id)
        {
            using(Dal dal = new Dal())
            {
                User user = dal.GetUserById(id);
                if(user != null)
                {
                    return View(user);
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult ModifyPassword(User user)
        {
            using(Dal dal = new Dal())
            {
                User userToChange = dal.GetUserById(user.Id);
                userToChange.Password = Dal.EncodeMD5(user.Password);
                dal.ModifyUser(userToChange);
            }

            return RedirectToAction("Index");

        }
    }
}

