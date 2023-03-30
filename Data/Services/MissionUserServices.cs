using Projet2_EasyFid.Models;
using System.Collections.Generic;
using System.Linq;

namespace Projet2_EasyFid.Data.Services
{
    public class MissionUserServices
    {
        public static List<MissionUser> GetAllMissionUsers(BddContext _bddContext)
        {
            return _bddContext.MissionUsers.ToList();
        }
        public static int CreateMissionUser(BddContext _bddContext, MissionUser missionUser)
        {
            _bddContext.MissionUsers.Add(missionUser);
            _bddContext.SaveChanges();
            return missionUser.Id;
        }
        public static void UpdateMissionUser(BddContext _bddContext, MissionUser missionUser)
        {
            // Update permet de mettre à jour directement le bon User dans la table (grâce à l'id sans doute)
            _bddContext.MissionUsers.Update(missionUser);
            _bddContext.SaveChanges();
        }
    }
}
