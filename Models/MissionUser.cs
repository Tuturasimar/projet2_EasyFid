using System;
using Projet2_EasyFid.Data.Enums;

namespace Projet2_EasyFid.Models
{

	// Classe qui regroupe les attributions des missions pour un utilisateur
	public class MissionUser
	{
		public int Id { get; set; }

		public MissionStateEnum MissionState {get;set;}

		public int UserId { get; set; }
		public User User { get; set; }

		public int MissionId { get; set; }
		public Mission Mission { get; set; }

		public int? UserFeedbackId { get; set; }
		public UserFeedback UserFeedback { get; set; }
	}

}

