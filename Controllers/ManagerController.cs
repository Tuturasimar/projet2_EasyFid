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
                ManagerViewModel managerViewModel  = new ManagerViewModel { Missions = missions };
                return View(managerViewModel);
            }
            
        }
        //ma methode de modification d'une mission
        public IActionResult UpdateMission(int id)
        {

            if (id != 0)
            {

                using (IDal dal = new Dal())
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

        //Affiche le formulaire de creation d'une mission
        public IActionResult CreateMission()
        {
            using (Dal dal = new Dal())
            {   List<Mission> missions = dal.GetAllMissions() ?? new List<Mission>();
                ViewBag.missions = missions;
            }
            return View();
        }

        [HttpPost]
        //Une fois qu'on appuie sur le bouton du formulaire, cette methode recupere un objet Mission
        public IActionResult CreateMission(int id, string name, DateTime missionStart, DateTime missionEnd, float tjm, MissionTypeEnum missionType)
        {
            using (Dal dal = new Dal())
            {

                Mission mission = new Mission
                {
                    Name = name,
                    MissionStart = missionStart,
                    MissionEnd = missionEnd,
                    Tjm = tjm,
                    MissionType = missionType
                };
                
                //Recuperation de l'id de la mission que nous venons de creer
                int missionId = mission.Id;


               
            }
              
            //Pour retourner sur la page d'affichage des mission
            return RedirectToAction("IndexManager");
            
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


    }
}

