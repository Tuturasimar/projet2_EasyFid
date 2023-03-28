using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    // Controller qui va gérer les méthodes basiques de l'application (authentification en tant que salarie)
    public class SalarieController : Controller
    {
        // GET: /<controller>/


        [Produces("application/json")]
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

        //Affiche tous les cras du salarie

        public IActionResult IndexSalarie()
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
                using (IDal dal = new Dal())
                {
                    Cra cra = dal.GetAllCras().Where(c => c.Id == id).FirstOrDefault();
                    if (cra == null) 
                    {
                        return View("Error");
                    }
                    return View(cra);
                }
            }
            return View("Error");
        }

        //Affiche le formulaire de modification du Cra
        [HttpPost]
        public IActionResult UpdateCra(Cra cra)
        {
            if (!ModelState.IsValid)
            {
                return View(cra);
            }
            if (cra.Id != 0)
            {
                using (Dal dal = new Dal())
                {
                    dal.UpdateCra(cra.Id, cra.StateCra);
                    return RedirectToAction("UpdateCra", new { @id = cra.Id });    
                }  
            }
            else
            {
                return View("Error");
            }
        }

        //Affiche le formulaire de creation du Cra
        public IActionResult CreateCra()
        {
            using (Dal dal = new Dal()) 
            {
                // Récupérer l'utilisateur actuellement connecté
                User user = dal.GetUser(HttpContext.User.Identity.Name);

                List<Mission> missions = dal.GetAllMissions();
                List<Formation> formations = dal.GetAllFormations();
                //List<Activity> activities = dal.GetAllActivities();
                List<MissionUser> missionUsers = dal.GetAllMissionUserByUserId(user.Id).ToList();
                //List<UserMissionViewModel> activities = dal.GetAllActivityByUserId(user.Id).ToList();
                
                ViewBag.missions = missions;
                ViewBag.formations = formations;    
                //ViewBag.activities = activities;
                ViewBag.missionUsers = missionUsers;

            }
            return View();
        }

        [HttpPost]
        //Une fois qu'on appuie sur le bouton du formulaire, cette methode recupere un objet Cra
        public IActionResult CreateCra(List<DateTime> BeginDate, List<DateTime> EndDate, List<int> activities, StateEnum stateEnum, int total)
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
                    StateCra = stateEnum
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
            return RedirectToAction("IndexSalarie");
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
                return RedirectToAction("IndexSalarie");
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
     
    }
}

