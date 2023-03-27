using System;
using Projet2_EasyFid.Models;

namespace Projet2_EasyFid.Data.Services
{
	public static class NotificationServices
	{
		public static void CreateNotification(BddContext _bddContext, Notification notification)
		{
			_bddContext.Notifications.Add(notification);
			_bddContext.SaveChanges();
		}

		public static void DeleteNotification(BddContext _bddContext, Notification notification)
		{
			_bddContext.Notifications.Remove(notification);
			_bddContext.SaveChanges();
		}
	}
}

