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

		public static bool CheckActivityDateComptability(BddContext _bddContext, List<DateTime> BeginDate, List<DateTime> EndDate, List<int> activities)
		{
			List<DateTime> bDate = new List<DateTime>();
			List<DateTime> eDate = new List<DateTime>();
            bool isCompatible = true;

			if(BeginDate.Count != activities.Count-1 || EndDate.Count != activities.Count - 1)
			{
				// Créer notification sur les champs non renseignés
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
						// Créer notification sur les dates non valides
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
							// Créer notification sur les dates de mission qui se chevauchent
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
	}
}

