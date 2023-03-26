using System;
namespace Projet2_EasyFid.Models
{
	// Classe qui regroupe les attributions des missions pour un utilisateur
	public class MissionUser
	{
		public int Id { get; set; }

		public int UserId { get; set; }
		public User User { get; set; }

		public int MissionId { get; set; }
		public Mission Mission { get; set; }
	}
}

