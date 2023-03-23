using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;

namespace Projet2_EasyFid.Data.Services
{
	public static class CraServices
	{
		
		public static List<Mission> GetAllMissions(BddContext _bddContext )
			{
				return _bddContext.Missions.ToList();
			}

		public static List <Formation> GetAllFormations(BddContext _bddContext )
		{
			return _bddContext.Formations.ToList();
		}

		public static List <Activity> GetAllActivities(BddContext _bddContext)
		{
			return _bddContext.Activities.ToList();
		}

		
		/*
		public static Mission GetMissionById(BddContext _bddContext, int id)
		{
			//On va réaliser ici une jointure entre 4 tables 
			//Pour récuperer les données de la table mission, depuis la table Cra
            var query = from c in _bddContext.Cras
                        join ca in _bddContext.CraActivities on c.Id equals ca.CraId //jointure entre CraActivity et Cra
                        join a in _bddContext.Activities on ca.ActivityId equals a.Id //jointure entre CraActivity et Activity
						join m in _bddContext.Missions on a.MissionId equals m.Id //jointure entre Activity et Mission
                        select m;
			var MissionId = query.ToList();
			return MissionId.FirstOrDefault();
        }
		*/

		public static Mission GetMissionById (BddContext _bddcontext, int id)
		{
			return _bddcontext.Missions.SingleOrDefault(m => m.Id == id);
		}


        public static int CreateCra(BddContext _bddContext, Cra cra ) 
		{
			_bddContext.Cras.Add(cra);
			_bddContext.SaveChanges();
			return cra.Id;
		}
	}
}

