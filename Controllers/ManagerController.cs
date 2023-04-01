using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Projet2_EasyFid.Data;
using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Data.Services;
using Projet2_EasyFid.Models;
using Projet2_EasyFid.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet2_EasyFid.Controllers
{
    [Authorize(Roles = "MANAGER")]
    // Controller qui va gérer les méthodes Manager (require authentification Manager)
    public class ManagerController : Controller
    {
        // GET: /<controller>/
        //Affichage de l'ensemble des missions
        public IActionResult Index()
        {
            using Dal dal = new Dal();
            {
                User user = dal.GetUser(HttpContext.User.Identity.Name);
                //Récupération des missions que l'on stocke dans une liste


                List<Cra> crasForManager = dal.GetAllCrasByManagerIdOrderByCreationDate(user.Id);
                


                CraListViewModel craList = new CraListViewModel { Cras = crasForManager };
                return View(craList);
            }

        }

        public IActionResult CraDetails(int id)
        {
            using (Dal dal = new Dal())
            {
                //On recupere le Cra en fonction de son Id
                Cra cra = dal.GetCraById(id);

                //On vérifie si le Cra existe en bdd
                if (cra != null)
                {
                    // On instancie une liste d'un CraDetailViewModel
                    // Ce modèle contient une Activity et une liste d'ActivityDate
                    List<CraDetailViewModel> craDetailViewModels = new List<CraDetailViewModel>();

                    // On récupère toutes les activités présentes dans ce Cra
                    List<Activity> activities = dal.GetAllActivityByCraId(id).ToList();

                    // Sur chaque activité, on boucle
                    foreach (Activity activity in activities)
                    {
                        // Pour chaque activité, on récupère les ActivityDate qui correspondent
                        List<ActivityDate> activityDates = dal.GetAllActivityDateByActivityIdAndCraId(activity.Id, cra.Id);
                        // On rajoute à notre liste de CraDetailViewModel une instance du modele
                        craDetailViewModels.Add(new CraDetailViewModel { Activity = activity, ActivityDates = activityDates });
                    }
                    // On envoie le cra dans une ViewBag
                    ViewBag.cra = cra;

                    return View(craDetailViewModels);
                }
            }
            //Si il n'existe pas, on retourne sur la vue Index
            return RedirectToAction("Index");

        }

        public IActionResult DenyCraValidation(int id)
        {
            using (Dal dal = new Dal())
            {
                Cra cra = dal.GetCraById(id);

                if (cra != null && cra.StateCra == StateEnum.INHOLD)
                {
                    ViewBag.cra = cra;

                    return View();
                }


                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult CraToDraft(int id, Notification notification)
        {
            using (Dal dal = new Dal())
            {
                Cra cra = dal.GetCraById(id);

                if (cra != null && cra.StateCra == StateEnum.INHOLD)
                {
                    cra.StateCra = StateEnum.DRAFT;
                    dal.ModifyCra(cra);
                    Notification notif = new Notification { MessageContent = "ERREUR CRA - " + notification.MessageContent, ClassContext = "danger", UserId = (int)cra.UserId };
                    dal.CreateNotification(notif);

                    return RedirectToAction("Index");
                }
                return View("Error");
            }
        }

        public IActionResult CraToValidation(int id)
        {
            using (Dal dal = new Dal())
            {
                Cra cra = dal.GetCraById(id);
                if (cra != null && cra.StateCra == StateEnum.INHOLD)
                {
                    cra.StateCra = StateEnum.VALIDATED;
                    dal.ModifyCra(cra);
                    Notification notification = new Notification { MessageContent = "Votre Cra a bien été validé", ClassContext = "success", UserId = (int)cra.UserId };
                    dal.CreateNotification(notification);
                    return RedirectToAction("Index");
                }
                return View("Error");
            }
        }


        public IActionResult SeeMissions()
        {
            using Dal dal = new Dal();
            {
                //Récupération des missions que l'on stocke dans une liste
                List<Mission> missions = dal.GetAllMissions();
                MissionListViewModel missionList = new MissionListViewModel { Missions = missions };
                return View(missionList);
            }
        }
        public IActionResult SeeMissionUsers()
        {
            using Dal dal = new Dal();
            {
                User user = dal.GetUser(HttpContext.User.Identity.Name);
                //Récupération des missions que l'on stocke dans une liste
                List<MissionUser> missionUsers = dal.GetAllMissionUserByManagerId(user.Id);
                
                return View(missionUsers);
            }
        }
        public IActionResult SeeFormation()
        {
            using Dal dal = new Dal();
            {
                //Récupération des formations que l'on stocke dans une liste
                List<Formation> formations = dal.GetAllFormations();
                FormationListViewModel formationList = new FormationListViewModel { Formations = formations };
                return View(formationList);
            }
        }

        [Produces("application/json")]
        public IActionResult GetAllStatistics()
        {
            try
            {
                var statistics = new BddContext().Statistics.OrderBy(s => s.Date).ToList();
                return Ok(statistics);
            }
            catch
            {
                return BadRequest();
            }
        }

        public IActionResult DisplayStatistics()
        {

            return View("EditDashboard");
        }

        //ma methode de modification d'une mission
        public IActionResult UpdateMission(int id)
        {

            if (id != 0)
            {

                using (Dal dal = new Dal())
                {
                    //je recherche l'ID qui est egal au parametre que m'a transmis l'utilisateur
                    Mission mission = dal.GetMissionById(id);
                    if (mission == null)
                    {
                        return View("Error");
                    }
                    return View(mission);
                }
            }
            return View("Error");
        }

        public IActionResult UpdateFormation(int id)
        {

            if (id != 0)
            {

                using (Dal dal = new Dal())
                {
                    //je recherche l'ID qui est egal au parametre que m'a transmis l'utilisateur
                    Formation formation = dal.GetAllFormations().Where(f => f.Id == id).FirstOrDefault();
                    if (formation == null)
                    {
                        return View("Error");
                    }
                    return View(formation);
                }
            }
            return View("Error");
        }
        public IActionResult EditDashboard()
        {
            using Dal dal = new Dal();
            {

                return View();
            }

        }

        [HttpPost]
        public IActionResult UpdateMission(Mission mission)
        {

            if (!ModelState.IsValid)
                return View(mission);


            if (mission.Id != 0)
            {
                using (Dal dal = new Dal())
                {
                    dal.UpdateMission(mission);
                    return RedirectToAction("SeeMissions", new { @id = mission.Id });
                }
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult UpdateFormation(Formation formation)
        {

            if (!ModelState.IsValid)
                return View(formation);


            if (formation.Id != 0)
            {
                using (Dal dal = new Dal())
                {
                    dal.UpdateFormation(formation);
                    return RedirectToAction("SeeFormation", new { @id = formation.Id });
                }
            }
            else
            {
                return View("Error");
            }
        }



        //Affiche le formulaire de creation d'une mission
        public IActionResult CreateMission()
        {

            return View();
        }
        

        [HttpPost]
        //Une fois qu'on appuie sur le bouton du formulaire, cette methode recupere un objet Mission
        public IActionResult CreateMission(Mission mission)
        {
            using (Dal dal = new Dal())

            {
                User user = dal.GetUser(HttpContext.User.Identity.Name);
                bool isDateValid = ActivityServices.isDateValid(mission.MissionStart, mission.MissionEnd);
                if (isDateValid)
                {
                    Mission newMission = new Mission
                    {
                        Name = mission.Name,
                        MissionStart = mission.MissionStart,
                        MissionEnd = mission.MissionEnd,
                        MissionType = mission.MissionType
                    };
                    int missionId = dal.CreateMission(newMission);
                    //Recuperation de l'id de la mission que nous venons de creer
                    Activity activity = new Activity
                    {
                        LabelActivity = mission.Name,
                        MissionId = missionId
                    };
                    dal.CreateActivity(activity);
                }
                else
                {
                    Notification notif = new Notification
                    {
                        MessageContent = "Merci de bien vouloir renseigner des dates cohérentes",
                        ClassContext = "danger",
                        UserId = user.Id
                    };
                    dal.CreateNotification(notif);
                }
            }

            //Pour retourner sur la page d'affichage des mission

            return RedirectToAction("SeeMissions");

        }
        

        
        //Une fois qu'on appuie sur le bouton du formulaire, cette methode recupere un objet Mission
        public IActionResult CreateMissionUser()
        {
            using (Dal dal = new Dal())

            {
                
                User user = dal.GetUser(HttpContext.User.Identity.Name);
             
                List<User> userList = dal.GetAllUsersByManagerId(user.Id);
                List<UserData> userDatas = new List<UserData>();
                foreach (User newUser in userList)
                {
                    userDatas.Add(newUser.UserData);
                }
                List<Mission>missions = dal.GetAllMissionActive();
                ViewBag.userDatas = userDatas;
                ViewBag.missions = missions;
                
            }
            return View();
        }

       
        
        [HttpPost]
        //Une fois qu'on appuie sur le bouton du formulaire, cette methode recupere un objet Mission
        public IActionResult CreateMissionUser(int userDatas, int missions)
        {
            using (Dal dal = new Dal())
            {
                User user = dal.GetUserByUserDataId(userDatas);
                MissionUser missionUser = new MissionUser { MissionId = missions, UserId =user.Id, MissionState= MissionStateEnum.ACTIVE };
                dal.CreateMissionUser(missionUser);
            }
            
            return RedirectToAction("SeeMissionUsers");

        }


        public IActionResult CreateFormation()
        {

            return View();
        }

        [HttpPost]
        //Une fois qu'on appuie sur le bouton du formulaire, cette methode recupere un objet Mission
        public IActionResult CreateFormation(Formation formation)
        {
            using (Dal dal = new Dal())
            {

                Formation newFormation = new Formation
                {
                    Name = formation.Name,
                    FormationStatus = formation.FormationStatus,
                    LocationFormation = formation.LocationFormation,
                    NbOfDays = formation.NbOfDays,
                    Description = formation.Description,
                    Location = formation.Location

                };
                int formationId = dal.CreateFormation(newFormation);
                //Recuperation de l'id de la mission que nous venons de creer
                Activity activity = new Activity
                {
                    LabelActivity = formation.Name,
                    FormationId = formationId
                };
                dal.CreateActivity(activity);
            }

            //Pour retourner sur la page d'affichage des mission
            return RedirectToAction("SeeFormation");




        }

        public IActionResult SeeUserFeedback(int id)


        {
            using (Dal dal = new Dal())
            {
                List<MissionUser> missionUsers = dal.GetAllMissionUserByMissionId(id);

                if (missionUsers.Count == 0)
                {
                    User user = dal.GetUser(HttpContext.User.Identity.Name);
                    Notification notif = new Notification { MessageContent = "Il n'y pas de notes disponibles pour cette mission", ClassContext = "danger", UserId = user.Id };
                    dal.CreateNotification(notif);
                    return RedirectToAction("SeeMissions");
                }

                return View(missionUsers);


            }
        }

        public IActionResult UserProfile(int id)
        {
            using (Dal dal = new Dal())
            {
                // On récupère l'id de l'utilisateur authentifié
                string authenticatedUserId = HttpContext.User.Identity.Name;


                // On vérifie si l'utilisateur existe en BDD et si l'id correspond à l'utilisateur authentifié
                User user = dal.GetUserById(id);
                if (user == null || user.Id.ToString() != authenticatedUserId)
                {
                    // Si l'utilisateur n'existe pas ou si l'id ne correspond pas à l'utilisateur authentifié, on redirige vers l'Index Admin
                    return RedirectToAction("Index");
                }

                // On récupère tous les rôles de l'utilisateur
                List<RoleUser> rolesUser = dal.GetAllRolesById(id);


                // On crée un ViewModel pour les détails de l'utilisateur et ses rôles
                UserRoleViewModel urvm = new UserRoleViewModel { User = user, RolesUser = rolesUser };

                // Envoi en paramètre à la vue UserDetail
                return View(urvm);
            }
        }

    }
}

