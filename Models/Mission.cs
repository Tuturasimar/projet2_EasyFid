using System;
using System.ComponentModel.DataAnnotations;
using Projet2_EasyFid.Data.Enums;

namespace Projet2_EasyFid.Models
{
	public class Mission
	{
		public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }
        // public string Adress;

        [Required(ErrorMessage = "Veuillez indiquer la date de début de la Misison")]
        public DateTime MissionStart { get; set; }

		//On ajoute un ? car la mission peut etre en cours et n'a pas forcement une date de fin
		public DateTime? MissionEnd { get; set; }
		public float Tjm { get; set; }
		public MissionTypeEnum MissionType { get; set; }
	}
}

