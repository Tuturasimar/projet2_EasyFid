using Projet2_EasyFid.Models;
using System;
namespace Projet2_EasyFid.Data.Services
{
	public static class ActivityServices
	{
		public static void CreateActivity(BddContext _bddContext, Activity activity)
		{
			_bddContext.Activities.Add(activity);
			_bddContext.SaveChanges();
		}
	}
}

