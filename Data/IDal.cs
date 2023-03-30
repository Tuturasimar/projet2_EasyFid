using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;
using System;
using System.Collections.Generic;

namespace Projet2_EasyFid.Data
{
	public interface IDal : IDisposable
	{

    User GetUser(string idStr);


		List<Cra> GetAllCras();

		int CreateCra (Cra cra);

        //void UpdateCra(int id, StateEnum stateCra);

        List<Mission> GetAllMissions();
        List<MissionUser> GetAllMissionUsers();
        List<Formation> GetAllFormations();

        
        void UpdateMission(Mission mission);
        void UpdateMissionUser(MissionUser missionUser);
        void UpdateFormation(Formation formation);
    



		int CreateMission(Mission mission);
        int CreateMissionUser(MissionUser missionUser);
        int CreateFormation(Formation formation);

        




    }

}

