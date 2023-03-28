using System;
using System.Collections.Generic;
using System.Linq;
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

		public static List<Notification> GetAllNotificationsByUserId(BddContext _bddContext, int id)
		{
			return _bddContext.Notifications.Where(n => n.UserId == id).ToList();

		}

		public static Notification GetNotificationById(BddContext _bddContext, int id)
		{
			return _bddContext.Notifications.SingleOrDefault(n => n.Id == id);
		}
	}
}

