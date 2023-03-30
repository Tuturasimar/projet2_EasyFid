using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration.UserSecrets;
using Projet2_EasyFid.Data;
using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Data.Services;
using Projet2_EasyFid.Models;
using Projet2_EasyFid.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet2_EasyFid.Controllers
{
    [Authorize(Roles = "SALARIE")]
    // Controller qui va gérer les méthodes basiques de l'application (authentification en tant que salarie)
    public class SalarieController : Controller
    {
        // GET: /<controller>/


        [Produces("application/json")]
        [AllowAnonymous]
        public IActionResult GetAllNotificationsByUser()
        {
            try
            {
                using(Dal dal = new Dal())
                {
                    // On récupère le User authentifié
                    User user = dal.GetUser(HttpContext.User.Identity.Name);

                    // On récupère toutes les notifications de l'utilisateur
                    var notifications = dal.GetAllNotificationsByUserId(user.Id);
                    foreach(Notification notif in notifications)
                    {
                        // On boucle dessus pour enlever le contenu de User (trop lourd pour le format Json)
                        notif.User = null;
                    }
                    return Ok(notifications);
                }
                
            }
            catch
            {
                return BadRequest();
            }
        }
        [AllowAnonymous]
        public IActionResult DeleteOneNotification(int id)
        {
            using(Dal dal = new Dal())
            {
                User user = dal.GetUser(HttpContext.User.Identity.Name);
                Notification notifToDelete = dal.GetNotificationById(id);
                dal.DeleteNotification(notifToDelete);

                // On récupère l'ancienne url 
                string route = HttpContext.Request.Headers["Referer"];
                string controllerName = "";
                // On récupère l'index du / de l'url, après le http://
                int indexOfControllerSlash = route.IndexOf("/", 8);
                // On récupère l'index d'un eventuel autre / à la suite
                int indexOfNextSlash = route.IndexOf("/", indexOfControllerSlash + 1);
                // S'il existe
                // Un indexOf qui vaut - 1 veut dire qu'on ne récupère pas l'élément recherché
                if(indexOfNextSlash != -1)
                {
                    // On utilise le substring pour récupérer uniquement ce qui se trouve après le premier / (salarie, manager, admin)
                    // Substring peut attendre deux arguments :
                        // Le premier est l'index à partir duquel on récupère le string (ici l'index du / + 1 pour commencer directement là où on souhaite obtenir la donnée)
                        // Le second est la longueur, ce qui correspond ici à la différence entre les deux index des / - 1
                    controllerName = route.Substring(indexOfControllerSlash + 1, indexOfNextSlash - indexOfControllerSlash - 1);
                } else
                {
                    // Substring avec un seul argument, va récupérer tout ce qui se situe après le premier /
                    controllerName = route.Substring(indexOfControllerSlash + 1);
                }
               

                if(controllerName != null)
                {
                    return RedirectToAction("Index", controllerName);
                }

                return View("Error");
            }
        }

        //Affiche tous les cras du salarie

        public IActionResult Index()
        {
            using (Dal dal = new Dal())
            {
                //On recupere tous les cras pour les stocker dans une liste

                // Récupérer l'utilisateur actuellement connecté
                User user = dal.GetUser(HttpContext.User.Identity.Name);


                List<Cra> cras = dal.GetAllCrasByUserId(user.Id);

                SalarieViewModel svm = new SalarieViewModel { Cras = cras };
                return View(svm);
            }
        }

        //Affiche le formulaire de modification du Cra en fonction de son id
        public IActionResult UpdateCra(int id)
        {
            if (id != 0)
            {
                using (Dal dal = new Dal())
                {
                    //On recupere l'utilisateur actuellement connecté
                    User user = dal.GetUser(HttpContext.User.Identity.Name);
                    //On recupere le menu deroulant
                    List<Activity> activities = new List<Activity>();
                    List<Mission> missionUsers = dal.GetAllMissionUserByUserId(user.Id);
                    foreach (Mission mission in missionUsers)
                    {
                        activities.Add(dal.GetActivityByMissionId(mission.Id));
                    }
                    List<Activity> otherActivities = dal.GetAllAbsenceAndFormation();
                    activities.AddRange(otherActivities);
                    //On recupere les valeurs en BDD
                    List<CraActivity> userActivities = dal.GetAllCraActivityByCraId(id);
                    //On instancie le ViewModel
                    List<CraDetailViewModel> cdvm = new List<CraDetailViewModel>();
                    // On récupère toutes les activités présentes dans ce Cra
                    List<Activity> activitiesByCraId = dal.GetAllActivityByCraId(id).ToList();
                    // Sur chaque activité, on boucle
                    foreach (Activity activity in activitiesByCraId)
                    {
                        // Pour chaque activité, on récupère les ActivityDate qui correspondent
                        List<ActivityDate> activityDates = dal.GetAllActivityDateByActivityIdAndCraId(activity.Id, id);
                        // On rajoute à notre liste de CraDetailViewModel une instance du modele
                        cdvm.Add(new CraDetailViewModel { Activity = activity, ActivityDates = activityDates });
                    }
                    ViewBag.activities = activities;
                    ViewBag.craId = id;
                    return View(cdvm);
                }
            }
            // Sinon, on est redirigé vers l'index
            return RedirectToAction("Index");
        }

        //Cette méthode recupère un objet de type Cra 
        [HttpPost]
        public IActionResult UpdateCra(List<CraDetailViewModel> cdvm, List<DateTime> BeginDate, List<DateTime> EndDate, List<int> activities, int Id)
        {

            using (Dal dal = new Dal())
            {
                //On recupere l'utilisateur actuellement connecté
                User user = dal.GetUser(HttpContext.User.Identity.Name);

                // Pour vérifier les dates : instancier une liste de DateTime (BeginDate et EndDate), récup celles du modeles et ajouter nouvelles
                // Aussi, instancier une liste qui regroupe toutes les ID des activités (en comptant aussi l'invisible, car pris en compte dans les méthodes)

                List<DateTime> BeginDateList = new List<DateTime>();
                List<DateTime> EndDateList = new List<DateTime>();
                List<int> activitiesId = new List<int>();


                foreach(CraDetailViewModel craModel in cdvm)
                {
                    activitiesId.Add(craModel.Activity.Id);
                    
                }

                for(int i = 0; i<BeginDate.Count - 1; i++)
                {
                    BeginDateList.Add(BeginDate[i]);
                    EndDateList.Add(EndDate[i]);
                }
                activitiesId.AddRange(activities);

                //On vérifie que les dates sont correctes
                bool isDateValid = dal.CheckActivityDateComptability(BeginDateList, EndDateList, activitiesId, user);

                if (!isDateValid)
                {

                    return RedirectToAction("Index");
                }

                //// On récupère l'ensemble des données renseignées pour ce Cra en BDD grâce à une requête
                Cra oldCra = dal.GetCraById(Id);

                //// On remplace un par un l'ensemble des champs du formulaire
                oldCra.UpdatedAt = DateTime.Now;
                oldCra.StateCra = StateEnum.DRAFT;
                dal.ModifyCra(oldCra);


                ////On recupère tous les CraActivity en fonction de l'id du Cra pour les supprimer ensuite
                List<CraActivity> oldCraActivity = dal.GetAllCraActivityByCraId(Id);
                // On récupère tous les ActivityDate liés à ces CraActivity pour les supprimer
                List<ActivityDate> oldActivityDate = new List<ActivityDate>();

                for(int i = 0; i< activitiesId.Count - 1; i++)
                {
                    oldActivityDate.AddRange(dal.GetAllActivityDateByActivityIdAndCraId(activitiesId[i],Id));
                }

                foreach(ActivityDate activityDateToDelete in oldActivityDate)
                {
                    dal.DeleteActivityDate(activityDateToDelete);
                }

                foreach(CraActivity craActivityToDelete in oldCraActivity)
                {
                    dal.DeleteCraActivity(craActivityToDelete);
                }

                // A ce niveau là, les anciennes dates renseignées et les CraActivity qui liaient le CRA en cours avec les activités n'existent plus

                // Il ne reste plus qu'à recréer les nouvelles qui les remplaceront

                // Création des nouveaux CraActivity

                for (int i = 0; i < activitiesId.Count - 1; i++)

                {
                    //On cree le CraActivity qui relie l'Activity et le Cra
                    //On cree le Cra avant cette methode car on a besoin de l'id du Cra
                    CraActivity newCraActivity = new CraActivity
                    {
                        CraId = Id,
                        ActivityId = activitiesId[i]
                    };

                    // On recupere l'id de la nouvelle CraActivity créée
                    // Il nous servira pour la suite, pour creer l'ActivityDate
                    int craActivityId = dal.CreateCraActivity(newCraActivity);

                    //On cree l'ActivityDate 
                    ActivityDate newActivityDate = new ActivityDate
                    {
                        BeginDate = BeginDateList[i],
                        EndDate = EndDateList[i],
                        CraActivityId = craActivityId
                    };

                    dal.CreateActivityDate(newActivityDate);
                }

                return RedirectToAction("CraDetail", new { @id = Id });    
               

        }

        //Affiche le formulaire de creation du Cra
        public IActionResult CreateCra()
        {
            using (Dal dal = new Dal()) 
            {
                // Récupérer l'utilisateur actuellement connecté
                User user = dal.GetUser(HttpContext.User.Identity.Name);

                // vérifier si un CRA de ce mois existe déjà ou non pour l'utilisateur connecté -> récupérer son ID
                List<Cra> craList = dal.GetAllCrasByUserId(user.Id);
                foreach(Cra cra in craList)
                {
                    // S'il existe, est ce que son State est DRAFT ?
                    if(cra.CreatedAt.Month == DateTime.Now.Month && cra.CreatedAt.Year == DateTime.Now.Year)
                    {
                        if(cra.StateCra == StateEnum.DRAFT)
                        {
                            Notification notification = new Notification { ClassContext = "info", MessageContent = "Le Cra du mois est déjà créé", UserId = user.Id };
                            return RedirectToAction("UpdateCra", new { id = cra.Id });
                        } else
                        {
                            Notification notification = new Notification { ClassContext = "danger", MessageContent = "Le CRA du mois a déjà été validé ou est en cours de traitement.", UserId = user.Id };
                            dal.CreateNotification(notification);
                            return RedirectToAction("Index");
                        }
                    } 
                }


                // Si le cra n'existe pas, suite de la méthode habituelle

                List<Activity> activities = new List<Activity>();
                List<Mission> missionUsers = dal.GetAllMissionUserByUserId(user.Id);
                foreach(Mission mission in missionUsers)
                {
                    activities.Add(dal.GetActivityByMissionId(mission.Id));
                }
                List<Activity> otherActivities = dal.GetAllAbsenceAndFormation();
                activities.AddRange(otherActivities);

                ViewBag.activities = activities;

            }
            return View();
        }

        [HttpPost]
        //Une fois qu'on appuie sur le bouton du formulaire, cette methode recupere un objet Cra
        public IActionResult CreateCra(List<DateTime> BeginDate, List<DateTime> EndDate, List<int> activities, int total)
        {
            using (Dal dal = new Dal())
            {
                User user = dal.GetUser(HttpContext.User.Identity.Name);

                bool isDateValid =  dal.CheckActivityDateComptability(BeginDate, EndDate, activities, user);
                if (!isDateValid)
                {
                    
                    return RedirectToAction("Index");
                }

                // On cree un nouveau Cra qui recupere la date de creation et de modification, ainsi que le statut du Cra
                // Il y aura un seul Cra d'instancié, qui aura un lien avec plusieurs activités

                Cra newCra = new Cra
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    StateCra = StateEnum.DRAFT,
                    UserId = user.Id
                };

                // On recupere l'id du nouveau Cra grace à la méthode CreateCra
                // Il servira pour la suite, pour faire le lien entre le Cra et les activités (dans CraActivity)
                int craId = dal.CreateCra(newCra);

                // On boucle en fonction du total d'activités récupérées afin de créer 

                for (int i = 0; i < total ; i++)

                {
                    //On cree le CraActivity qui relie l'Activity et le Cra
                    //On cree le Cra avant cette methode car on a besoin de l'id du Cra
                    CraActivity newCraActivity = new CraActivity
                    {
                        CraId = craId,
                        ActivityId = activities[i]
                    };

                    // On recupere l'id de la nouvelle CraActivity créée
                    // Il nous servira pour la suite, pour creer l'ActivityDate
                    int craActivityId = dal.CreateCraActivity(newCraActivity);

                    //On cree l'ActivityDate 
                    ActivityDate newActivityDate = new ActivityDate
                    {
                        BeginDate = BeginDate[i],
                        EndDate = EndDate[i],
                        CraActivityId = craActivityId
                    };

                    dal.CreateActivityDate(newActivityDate);
                }

                //On recupere l'id de la nouvelle ActivityDate creee
                //Utile ou non ??
                //int activityDateId = dal.CreateActivityDate(newActivityDate);
            }

            //Pour retourner sur la page d'affichade des cras
            return RedirectToAction("Index");
        }

        public IActionResult CraDetail(int id)
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
                        List<ActivityDate> activityDates = dal.GetAllActivityDateByActivityIdAndCraId(activity.Id,cra.Id);
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

        public IActionResult AskForCraValidation(int id)
        {
            using (Dal dal = new Dal())
            {
                User user = dal.GetUser(HttpContext.User.Identity.Name);
                Cra cra = dal.GetCraById(id);

                if(cra != null && cra.StateCra == StateEnum.DRAFT)
                {
                    cra.StateCra = StateEnum.INHOLD;
                    dal.ModifyCra(cra);
                    Notification notif = new Notification { ClassContext = "success", MessageContent = "Votre CRA va être consulté par votre manager pour validation.", UserId = user.Id };
                    Notification notifToManager = new Notification { ClassContext = "info", MessageContent = user.UserData.Firstname + " " + user.UserData.Lastname + " demande validation de son CRA.", UserId= (int)user.ManagerId };
                    dal.CreateNotification(notif);
                    dal.CreateNotification(notifToManager);

                    return RedirectToAction("Index");
                }
                return View("Error");
            }
        }

        public IActionResult SeeCurrentUserFeedback()
        {
            using(Dal dal = new Dal())
            {
                // On récupère l'utilisateur actuellement authentifié
                User user = dal.GetUser(HttpContext.User.Identity.Name);
                // On récupère les missions attribuées par son manager en amont
                List<MissionUser> activeMissions = dal.GetAllActiveMissionsByUserId(user.Id);
                ViewBag.activeMissions = activeMissions;
                return View(activeMissions);
            }

        }

        [AllowAnonymous]
        public IActionResult UserDetail(int id)
        {
            using (Dal dal = new Dal())
            {
                // On récupère l'id de l'utilisateur authentifié
                string authenticatedUserId = HttpContext.User.Identity.Name;
                if(authenticatedUserId == null)
                {
                    return RedirectToAction("Index", "Login");
                }

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

        [HttpPost]
        public IActionResult SeeCurrentUserFeedback(List<MissionUser> missionUsers)
        {
            using(Dal dal = new Dal())
            {
                // On boucle sur la liste de MissionUser que l'on reçoit
                foreach (MissionUser missionUser in missionUsers)
                {
                    // Si c'est une modification d'un UserFeedback
                    if (missionUser.UserFeedbackId != null)
                    {
                        // On récupère celui qui existe déjà
                      UserFeedback oldUserFeedback = dal.GetUserFeedbackById((int)missionUser.UserFeedbackId);

                        // On change les différentes informations
                        oldUserFeedback.GradeClientRelation = missionUser.UserFeedback.GradeClientRelation;
                        oldUserFeedback.GradeManager = missionUser.UserFeedback.GradeManager;
                        oldUserFeedback.GradeMission = missionUser.UserFeedback.GradeMission;
                        oldUserFeedback.GradeUserComfort = missionUser.UserFeedback.GradeUserComfort;
                        oldUserFeedback.Comment = missionUser.UserFeedback.Comment;

                        // On le modifie
                        dal.ModifyUserFeedback(oldUserFeedback);
                    }
                    // Si c'est une création d'un UserFeedback
                    else
                    {
                        // On instancie un nouveau userFeedback
                        UserFeedback userFeedback = new UserFeedback
                        {
                            Comment = missionUser.UserFeedback.Comment,
                            GradeClientRelation = missionUser.UserFeedback.GradeClientRelation,
                            GradeManager = missionUser.UserFeedback.GradeManager,
                            GradeMission = missionUser.UserFeedback.GradeMission,
                            GradeUserComfort = missionUser.UserFeedback.GradeUserComfort
                        };
                        // On récupère l'Id lors de sa création dans la BDD
                        int userFeedbackId = dal.CreateUserFeedback(userFeedback);

                        // On récupère l'ancien MissionUser qui ne disposait pas encore de UserFeedback
                        MissionUser oldMissionUser = dal.GetMissionUserById(missionUser.Id);
                        // On renseigne la valeur de l'Id du UserFeedback
                        oldMissionUser.UserFeedbackId = userFeedbackId;

                        // On modifie le changement
                        dal.ModifyMissionUser(oldMissionUser);
                        
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // Malgré le fait que le controller nécessite d'être salarié, on override les autorisations ici
        [AllowAnonymous]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
    }
}

