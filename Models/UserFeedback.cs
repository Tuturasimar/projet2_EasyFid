using System;
using System.ComponentModel.DataAnnotations;

namespace Projet2_EasyFid.Models
{
	// Classe qui regroupe l'ensemble des notes données par l'utilisateur dans le cadre de ses missions
	public class UserFeedback
	{
		public int Id { get; set; }
		public string Comment { get; set; }

		// Rajouter des RegEx pour forcer la note de 0 à 5
		[RegularExpression(@"^[0-5]$", ErrorMessage ="La note doit être comprise entre 0 et 5.")]
		public int GradeMission { get; set; }
        [RegularExpression(@"^[0-5]$", ErrorMessage = "La note doit être comprise entre 0 et 5.")]
        public int GradeManager { get; set; }
        [RegularExpression(@"^[0-5]$", ErrorMessage = "La note doit être comprise entre 0 et 5.")]
        public int GradeClientRelation { get; set; }
        [RegularExpression(@"^[0-5]$", ErrorMessage = "La note doit être comprise entre 0 et 5.")]
        public int GradeUserComfort { get; set; }
	}
}

