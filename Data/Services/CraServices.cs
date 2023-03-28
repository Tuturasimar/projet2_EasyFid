using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;
using Projet2_EasyFid.ViewModels;
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
	

		public static List<Cra> GetAllCrasByUserId(BddContext _bddContext, int id)
		{
			List<Cra> cras = _bddContext.Cras.Where(c => c.UserId == id).ToList();
			return cras;
		}

		public static List<Cra> GetAllInHoldAndValidatedCrasByUserId(BddContext _bddContext, int id)
		{
			return _bddContext.Cras.Include(c => c.User.UserData).Where(c => c.UserId == id && (c.StateCra == StateEnum.INHOLD || c.StateCra == StateEnum.VALIDATED)).ToList();
		}

		public static void SetUserIdNullOnDelete(BddContext _bddContext, Cra cra)
		{
			_bddContext.Cras.Update(cra);
			_bddContext.SaveChanges();
		}

		public static Cra GetCraById(BddContext _bddContext, int id)
		{
			Cra cra = _bddContext.Cras.Include(c => c.User).Include(c => c.User.UserData).SingleOrDefault(c => c.Id == id);
			return cra;
		}

		public static CraActivity GetCraActivityByCraId(BddContext _bddContext, int id)
		{
			CraActivity craActivity = _bddContext.CraActivities.Include(ca => ca.Activity).Where(c => c.CraId == id).SingleOrDefault(ca => ca.Id == id);
			return craActivity;
		}

		
		public static List<Activity> GetAllActivityByCraId (BddContext _bddContext, int id)
		{
			//Ici on fait une jointure entre deux tables, la table CraActivity et la table Activity
			var query = from ca in _bddContext.CraActivities
						join a in _bddContext.Activities on ca.ActivityId equals a.Id //jointure entre CraActivity et Activity
						where ca.CraId == id //On recupere les Activity qui ont le meme CraId (donc le même Cra)
						select a;
			return query.ToList(); //On recupère une liste d'Activity
		}
        
        public static List<ActivityDate> GetAllActivityDateByCraId (BddContext _bddContext, int id)
		{
			//Ici on fait une jointure entre 2 tables
			var query = from ad in _bddContext.ActivityDates
						join ca in _bddContext.CraActivities on ad.CraActivityId equals ca.Id //jointure entre ActivityDate et CraActivity
						where ca.CraId == id //On recupere les ActivityDate qui ont le même CraId (donc le même Cra)
						select ad;
			return query.ToList();
		}

        //Pour reucperer tous les BeginDate d'une ActivityDate
        //Pas utile pour l'instant, à voir pour la suite, je laisse en commentaire pour l'instant
        /*
        public static List<DateTime> GetBeginDate (BddContext _bddContext, int id)
		{
            //Ici on fait une jointure entre 2 tables
            var query = from ad in _bddContext.ActivityDates
                        join ca in _bddContext.CraActivities on ad.CraActivityId equals ca.Id //jointure entre ActivityDate et CraActivity
                        where ca.CraId == id //On recupere les ActivityDate qui ont le même CraId (donc le même Cra)
                        select ad.BeginDate;
            return query.ToList();
        }
		*/

        public static List<MissionUser> GetAllMissionUserByUserId(BddContext _bddContext, int id)
        {
			//Ici on fait une jointure entre 3 tables 
			var query = from mu in _bddContext.MissionUsers
						join m in _bddContext.Missions on mu.MissionId equals m.Id //jointure entre les tables MissionUser et Missions
						join u in _bddContext.Users on mu.UserId equals u.Id //jointure entre les tables MissionUser et User 
                        where mu.UserId == id && mu.MissionState == MissionStateEnum.ACTIVE
                        select mu;
			return query.ToList();
        }

		/* Methode Pierre
        public static List<UserMissionViewModel> GetAllActivityByUserId(BddContext _bddContext, int id)
        {
			//Ici on fait une jointure entre 4 tables 
			var query = from a in _bddContext.Activities
						join f in _bddContext.Formations on a.FormationId equals f.Id //Jointure entre les tables Activity et Formation
						join abs in _bddContext.Absences on a.AbsenceId equals abs.Id //Jointure entres les tables Activity et Absence
						join m in _bddContext.Missions on a.MissionId equals m.Id //Jointure entre les tables Activity et Mission ==> recupère les 6 Missions disponibles
						join mu in _bddContext.MissionUsers on m.MissionUsersId equals mu.Id //Jointure entre les tables Mission et MissionUser ==> recupère les 4 missions attribuées au User 2
						join u in _bddContext.Users on mu.UserId equals u.Id //jointure entre les tables MissionUser et User ==> Recupère aussi les 4 missions
						where mu.UserId == id && mu.MissionState == MissionStateEnum.ACTIVE //recupère plus que 3 missions car l'une d'entre elle n'a pas le bon State
						select new {a, f, abs};
            var res = query.ToList();
			List<UserMissionViewModel> myList = new List<UserMissionViewModel>();
			foreach (var item in res)
			{
				myList.Add(new UserMissionViewModel { Absence = item.abs, Activity = item.a, Formation = item.f });
			}
			return myList;
        }
		*/

		






    }

}

