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
    }
}
