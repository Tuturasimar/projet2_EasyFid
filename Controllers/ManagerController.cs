using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Projet2_EasyFid.Data;
using Projet2_EasyFid.Models;
using Projet2_EasyFid.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Projet2_EasyFid.Controllers
{
    // Controller qui va gérer les méthodes Manager (require authentification Manager)
    public class ManagerController : Controller
    {
        // GET: /<controller>/

        public IActionResult Index()
        {
            return View();
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
                    if(mission != null)
                    {
                        return View("Error");
                    }
                    return View(mission);
                }
            }
            return View();
        }
    }
}

