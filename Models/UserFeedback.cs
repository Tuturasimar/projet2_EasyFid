using System;
namespace Projet2_EasyFid.Models
{
	// Classe qui regroupe l'ensemble des notes données par l'utilisateur dans le cadre de ses missions
	public class UserFeedback
	{
		public int Id { get; set; }

		public string Comment { get; set; }

		// Rajouter des RegEx pour forcer la note de 0 à 5
		public int GradeMission { get; set; }
		public int GradeManager { get; set; }
		public int GradeClientRelation { get; set; }
		public int GradeUserComfort { get; set; }
	}
}

