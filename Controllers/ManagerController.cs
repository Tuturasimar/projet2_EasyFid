using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Projet2_EasyFid.Data;
using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;
using Projet2_EasyFid.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet2_EasyFid.Controllers
{
    // Controller qui va gérer les méthodes Manager (require authentification Manager)
    public class ManagerController : Controller
    {

        

        // GET: /<controller>/
        //Affichage de l'ensemble des missions
        public IActionResult Index()
        {
            using Dal dal =new Dal();
            {
                //Récupération des missions que l'on stocke dans une liste
                List<Mission>missions=dal.GetAllMissions();
                MissionListViewModel missionList  = new MissionListViewModel { Missions = missions };
                return View(missionList);
            }

        }

        [Produces("application/json")]
        public IActionResult GetAllStatistics()
        {
            try
            {
                var statistics = new BddContext().Statistics.OrderBy(s=>s.Date).ToList();
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
                Mission mission = dal.GetAllMissions().Where(m=>m.Id ==id).FirstOrDefault();
                    if(mission == null)
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

                using (IDal dal = new Dal())
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
                    return RedirectToAction("UpdateMission", new { @id = mission.Id });
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
                    return RedirectToAction("UpdateFormation", new { @id = formation.Id });
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
        public IActionResult CreateFormation()
        {
            
            return View();
        }

        [HttpPost]
        //Une fois qu'on appuie sur le bouton du formulaire, cette methode recupere un objet Mission
        public IActionResult CreateMission( Mission mission)
        {
            using (Dal dal = new Dal())
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
            
            //Pour retourner sur la page d'affichage des mission
            return RedirectToAction("Index");
            
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
            return RedirectToAction("Index");

        }



    }
}

