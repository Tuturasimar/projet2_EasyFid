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

		public static Mission GetMissionById (BddContext _bddcontext, int id)
		{
			return _bddcontext.Missions.SingleOrDefault(m => m.Id == id);
		}

		//Methode pour créer un Cra et qui retourne son Id 
        public static int CreateCra(BddContext _bddContext, Cra cra ) 
		{
			_bddContext.Cras.Add(cra);
			_bddContext.SaveChanges();
			return cra.Id;
		}


		public static Activity GetActivityById (BddContext _bddContext,  int id)
		{
			return _bddContext.Activities.SingleOrDefault(a => a.Id == id);
		}

		public static ActivityDate GetActivityDateById (BddContext _bddContext, int id)
		{
			return _bddContext.ActivityDates.SingleOrDefault(ad => ad.Id == id);
		}

		//Methode qui creee une nouvelle activité dans la BDD et qui nous retourne son Id
		public static int CreateActivityDate(BddContext _bddContext, ActivityDate activityDate)
		{
			_bddContext.ActivityDates.Add(activityDate);
			_bddContext.SaveChanges();
			return activityDate.Id;
		}

		//Methode qui creee une nouvelle CraActivity dans la BDD et qui nous retourne son Id
		//Cette table permet de relier l'Activity et le Cra grace a des clefs etrangeres 
		public static int CreateCraActivity(BddContext _bddContext, CraActivity craActivity)
		{
			_bddContext.CraActivities.Add(craActivity);
			_bddContext.SaveChanges();
			return craActivity.Id;
		}
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

