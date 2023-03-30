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

        public static Mission GetMissionById(BddContext _bddContext, int id)
        {
            // Le Include permet ici de récupérer les données du MissionUser (qui est lié à User par une clé étrangère)
            // Sans Include, impossible de récupérer certaines données en faisant User.Userdata.FirstName, par exemple.
            //Mission mission = _bddContext.Missions.Include(m => m.Activity).Include(m => m.Name).Include(m => m.Mana).SingleOrDefault(u => u.Id == id);
            //return mission;
            Mission mission = _bddContext.Missions.SingleOrDefault(m => m.Id == id);
            return mission;

        }

        public static void UpdateMission(BddContext _bddContext, Mission mission)
        {
            // Update permet de mettre à jour directement le bon User dans la table (grâce à l'id sans doute)
            _bddContext.Missions.Update(mission);
            _bddContext.SaveChanges();
        }

        public static void DeleteMissionById(BddContext _bddContext, int id)
        {
            Mission missionToDelete = MissionServices.GetMissionById(_bddContext, id);
            _bddContext.Missions.Remove(missionToDelete);
            _bddContext.SaveChanges();
        }

        //MissionUser

           public static MissionUser GetMissionUserById(BddContext _bddContext, int id)
        {
            return _bddContext.MissionUsers.SingleOrDefault(m => m.Id == id); 
        }


        public static int CreateMissionUser(BddContext _bddContext, MissionUser missionUser)
        {
            _bddContext.MissionUsers.Add(missionUser);
            _bddContext.SaveChanges();
            return missionUser.Id;
        }


        public static List<MissionUser> GetAllMissionUsers(BddContext _bddContext)
        {
            return _bddContext.MissionUsers.ToList();
        }

        public static void DeleteMissionUserById(BddContext _bddContext, int id)
        {
            MissionUser missionUserToDelete = MissionServices.GetMissionUserById(_bddContext, id);
            _bddContext.MissionUsers.Remove(missionUserToDelete);
            _bddContext.SaveChanges();
        }

        public static List<MissionUser> GetAllActiveMissionsByUserId(BddContext _bddContext,int id)
        {
           return _bddContext.MissionUsers.Include(m => m.Mission).Include(m => m.UserFeedback).Where(m => m.UserId == id && m.MissionState == MissionStateEnum.ACTIVE).ToList();
        }

        public static void ModifyMissionUser(BddContext _bddContext, MissionUser missionUser)
        {
            _bddContext.MissionUsers.Update(missionUser);
            _bddContext.SaveChanges();
        }

        

    }
}