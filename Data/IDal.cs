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
		int CreateMission(int id,string name,DateTime missionStart,DateTime missionEnd,float tjm,MissionTypeEnum missionType);
		void UpdateMission(Mission mission);
	}
}

