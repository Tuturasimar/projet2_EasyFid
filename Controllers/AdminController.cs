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
    }
}

