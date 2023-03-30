using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Projet2_EasyFid.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projet2_EasyFid.Data.Services
{
	public static class ActivityServices
	{
		public static void CreateActivity(BddContext _bddContext, Activity activity)
		{
			_bddContext.Activities.Add(activity);
			_bddContext.SaveChanges();
		}

		public static bool CheckActivityDateComptability(BddContext _bddContext, List<DateTime> BeginDate, List<DateTime> EndDate, List<int> activities, User user)
		{
			List<DateTime> bDate = new List<DateTime>();
			List<DateTime> eDate = new List<DateTime>();
            bool isCompatible = true;

			if(BeginDate.Count != activities.Count-1 || EndDate.Count != activities.Count - 1)
			{
				Notification notification = new Notification { ClassContext = "danger", MessageContent = "Renseignez tous les champs lors de l'ajout d'activités", UserId= user.Id};
				NotificationServices.CreateNotification(_bddContext, notification);
				return false;
			}

            for (int i = 0; i <activities.Count - 1; i++)
			{
				if (isActivityAMissionById(_bddContext, activities[i]))
				{
					
					if (isDateValid(BeginDate[i], EndDate[i]))
					{
                        bDate.Add(BeginDate[i]);
                        eDate.Add(EndDate[i]);
                    }
					else
					{
                        Notification notification = new Notification { ClassContext = "danger", MessageContent = "Renseignez des dates de début et de fin cohérentes.", UserId = user.Id };
                        NotificationServices.CreateNotification(_bddContext, notification);
                        return false;
					}
					
				} ;
			}

			for(int i = 0; i<bDate.Count - 1; i++)
			{
				for (int j = i + 1; j < bDate.Count; j++)
				{
					int value = bDate[j].CompareTo(eDate[i]);
					if (value < 0)
					{
						int otherValue = bDate[i].CompareTo(eDate[j]);
						if (otherValue < 0)
						{
							isCompatible = false;
                            Notification notification = new Notification { ClassContext = "danger", MessageContent = "Renseignez des dates de missions qui ne se chevauchent pas", UserId = user.Id };
                            NotificationServices.CreateNotification(_bddContext, notification);
                            break;
						}
					}
				}		 
            }
			return isCompatible;
		}

		public static bool isActivityAMissionById(BddContext _bddContext, int id)
		{
			bool isAMission = false;

			Activity activity = _bddContext.Activities.SingleOrDefault(a => a.Id == id);
			if(activity.MissionId != null)
			{
				isAMission = true;
			}
			return isAMission;
		}

		public static bool isDateValid(DateTime beginDate, DateTime endDate)
		{
			bool isDateValid = false;

			int value = endDate.CompareTo(beginDate);
			if(value > 0)
			{
				isDateValid = true;
			}

			return isDateValid;
		}

		public static List<ActivityDate> GetAllActivityDateByCraId(BddContext _bddContext, int id)
		{
			return _bddContext.ActivityDates.Include(a => a.CraActivity.Activity).Where(a => a.CraActivity.CraId == id).ToList();
		}

		public static List<ActivityDate> GetAllActivityDateByActivityIdAndCraId(BddContext _bddContext, int idActivity, int idCra)
		{
			return _bddContext.ActivityDates.Include(a => a.CraActivity).Where(a => a.CraActivity.ActivityId == idActivity && a.CraActivity.CraId == idCra).ToList();
		}

		public static Activity GetActivityByMissionId(BddContext _bddContext, int id)
		{
			return _bddContext.Activities.SingleOrDefault(a => a.MissionId == id);
		}

		public static List<Activity> GetAllAbsenceAndFormation(BddContext _bddContext)
		{
			return _bddContext.Activities.Where(a => a.AbsenceId != null || a.FormationId != null).ToList();


		}




		public static void DeleteActivityDate(BddContext _bddContext, ActivityDate activityDate)
		{
			_bddContext.ActivityDates.Remove(activityDate);
			_bddContext.SaveChanges();
		}
	}

}

