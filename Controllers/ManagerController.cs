﻿using System;
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
                User user = dal.GetUser(HttpContext.User.Identity.Name);
                //Récupération des missions que l'on stocke dans une liste
                List<User> users = dal.GetAllUsersByManagerId(user.Id);
                List<Cra> crasForManager = new List<Cra>();
                foreach(User userCra in users)
                {
                    crasForManager.AddRange(dal.GetAllInHoldAndValidatedCrasByUserId(userCra.Id));
                }

                CraListViewModel craList  = new CraListViewModel { Cras = crasForManager };
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
            using(Dal dal = new Dal())
            {
                Cra cra = dal.GetCraById(id);

                if(cra != null && cra.StateCra == StateEnum.INHOLD)
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

                if(cra != null && cra.StateCra == StateEnum.INHOLD)
                {
                    cra.StateCra = StateEnum.DRAFT;
                    dal.ModifyCra(cra);
                    Notification notif = new Notification { MessageContent = "ERREUR CRA - " +notification.MessageContent, ClassContext = "danger", UserId = (int)cra.UserId };
                    dal.CreateNotification(notif);

                    return RedirectToAction("Index");
                }
                return View("Error");
            }
        }

        public IActionResult CraToValidation(int id)
        {
            using(Dal dal = new Dal())
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

        //ma methode de modification d'une mission
        public IActionResult UpdateMission(int id)
        {

            if (id != 0)
            {

                using (Dal dal = new Dal())
                {
                    //je recherche l'ID qui est egal au parametre que m'a transmis l'utilisateur
                    Mission mission = dal.GetMissionById(id);
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

