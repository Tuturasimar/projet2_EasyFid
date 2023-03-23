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
        public IActionResult CreateCra(ActivityDate activityDate, int activities, StateEnum stateEnum)
        {
            using (Dal dal = new Dal())
            {

                //On recupere l'id de l'Activity
                int activityId = dal.GetActivityById(activities).Id;

                //On cree l'ActivityDate 
                ActivityDate newActivityDate = new ActivityDate
                {
                    
                };

            //On cree un nouveau Cra qui recupere la date de creation et de modification, ainsi que le status du Cra
            Cra newCra = new Cra
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StateCra = stateEnum

            };
                //On recupere l'id du nouveau Cra grace à la méthode CreateCra
                int CraId = dal.CreateCra(newCra);
                
            }

            //Pour retourner sur la page d'affichade des cras
            return RedirectToAction("IndexSalarie");
        }


    }
}

