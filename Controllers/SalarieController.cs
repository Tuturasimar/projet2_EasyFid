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
using Projet2_EasyFid.Models;
using Projet2_EasyFid.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet2_EasyFid.Controllers
{
    // Controller qui va gérer les méthodes basiques de l'application (authentification en tant que salarie)
    public class SalarieController : Controller
    {
        // GET: /<controller>/
        //Affiche tous les cras du salarie
        public IActionResult IndexSalarie()
        {
            using (Dal dal = new Dal())
            {
                //On recupere tous les cras pour les stocker dans une liste

                // Récupérer l'utilisateur actuellement connecté
                User user = dal.GetUser(HttpContext.User.Identity.Name);

                List<Cra> cras = dal.GetAllCras();
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
                List<Mission> missions = dal.GetAllMissions();
                List<Formation> formations = dal.GetAllFormations();
                List<Activity> activities = dal.GetAllActivities();
                ViewBag.missions = missions;
                ViewBag.formations = formations;    
                ViewBag.activities = activities;

            }
            return View();
        }

        [HttpPost]
        //Une fois qu'on appuie sur le bouton du formulaire, cette methode recupere un objet Cra
        public IActionResult CreateCra(List<DateTime> BeginDate, List<DateTime> EndDate, List<int> activities, StateEnum stateEnum, int total)
        {
            using (Dal dal = new Dal())
            {
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
                for (int i = 0; i < total - 1; i++)
                {
                    // On recupere l'id de l'activity actuelle
                    // Il servira pour la suite, pour creer la CraActivity

                    int activityId = dal.GetActivityById(activities[i]).Id;

                    //On cree le CraActivity qui relie l'Activity et le Cra
                    //On cree le Cra avant cette methode car on a besoin de l'id du Cra
                    CraActivity newCraActivity = new CraActivity
                    {
                        CraId = craId,
                        ActivityId = activityId
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
                //On recupere le CraActivity afin de pouvoir recuperer l'Activity reliee au Cra
                CraActivity craActivity = dal.GetCraActivityByCraId(id);
                //On recupère les Activity reliees au même Cra 
                List<Activity> activities = dal.GetAllActivityByCraId(id).ToList();

                //On vérifie si le Cra existe en bdd
                if (cra != null)
                {
                    SalarieViewModel svm = new SalarieViewModel { Cra = cra, CraActivity =  craActivity, Activities = activities};
                    return View(svm);
                }
            }
                //Si il n'existe pas, on retourne sur la vue Index
                return RedirectToAction("IndexSalarie");
        }

        
    }
}

