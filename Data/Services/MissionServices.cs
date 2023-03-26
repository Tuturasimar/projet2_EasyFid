using Microsoft.EntityFrameworkCore;
using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;
using System.Collections.Generic;
using System.Linq;

namespace Projet2_EasyFid.Data.Services
{
    public class MissionServices
    {
        public static List<Mission> GetAllMissions(BddContext _bddContext)
        {
            return _bddContext.Missions.ToList();
        }
        public static int CreateMission(BddContext _bddContext, Mission mission)
        {
            _bddContext.Missions.Add(mission);
            _bddContext.SaveChanges();
            return mission.Id;
        }
        //public static User GetMissionById(BddContext _bddContext, int id)
        //{
        //    // Le Include permet ici de récupérer les données du UserData (qui est lié à User par une clé étrangère)
        //    // Sans Include, impossible de récupérer certaines données en faisant User.Userdata.FirstName, par exemple.
        //    //Mission mission = _bddContext.Missions.Include(m => m.Activity).Include(m => m.Name).Include(m => m.Mana).SingleOrDefault(u => u.Id == id);
        //    //return mission;
        //}

        public static List<MissionUser> GetAllActiveMissionsByUserId(BddContext _bddContext,int id)
        {
           return _bddContext.MissionUsers.Include(m => m.Mission).Where(m => m.UserId == id && m.MissionState == MissionStateEnum.ACTIVE).ToList();
        }

        
    }
}
