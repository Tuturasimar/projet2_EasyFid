using Projet2_EasyFid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Projet2_EasyFid.Data.Services
{
	public static class CraServices
	{
		
		public static List<Mission> GetAllMissions( BddContext _bddContext )
			{
				return _bddContext.Missions.ToList();
			}

		public static List <Formation> GetAllFormations (BddContext _bddContext )
		{
			return _bddContext.Formations.ToList();
		}
	}
}

