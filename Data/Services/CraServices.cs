using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using Projet2_EasyFid.Data.Enums;
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

		public static List <Activity> GetAllActivities (BddContext _bddContext)
		{
			return _bddContext.Activities.ToList();
		}

		/*
		public static Mission GetMissionById (BddContext _bddContext, int id)
		{
            var query = from c in _bddContext.Cras
                        join ca in _bddContext.CraActivities on c.Id equals ca.CraId
                        join a in _bddContext.Activities on ca.ActivityId equals a.Id
						join m in _bddContext.Missions on a.MissionId equals m.Id
                        select m;
			return query;
        }
		*/

		public static int CreateCra(BddContext _bddContext, Cra cra ) 
		{
			_bddContext.Cras.Add(cra);
			_bddContext.SaveChanges();
			return cra.Id;
		}

		public static List<Cra> GetAllCrasByUserId(BddContext _bddContext, int id)
		{
			List<Cra> cras = _bddContext.Cras.Where(c => c.UserId == id).ToList();
			return cras;
		}

		public static void SetUserIdNullOnDelete(BddContext _bddContext, Cra cra)
		{
			_bddContext.Cras.Update(cra);
			_bddContext.SaveChanges();
		}

    }
}

