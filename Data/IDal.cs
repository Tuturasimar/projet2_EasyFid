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

		void UpdateCra(int id, StateEnum stateCra);


		List<Mission> GetAllMissions();
        List<Formation> GetAllFormations();

        //int DeleteMission(int id);
        void UpdateMission(Mission mission);
        void UpdateFormation(Formation formation);
    

		int CreateMission(Mission mission);
		//int DeleteMission(int id);

	}

}

