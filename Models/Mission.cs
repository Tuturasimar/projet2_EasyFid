using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Projet2_EasyFid.Data.Enums;

namespace Projet2_EasyFid.Models
{
	public class Mission
	{
		public int Id { get; set; }

        [MaxLength(20)]
		[Display (Name="Nom de mission")]
        [Required(ErrorMessage = "Veuillez indiquer le nom de la Mission")]
        public string Name { get; set; }
        // public string Adress;

        [Required(ErrorMessage = "Veuillez indiquer la date de début de la Mission")]
        [Display(Name = "Début")]
        public DateTime MissionStart { get; set; }

        //On ajoute un ? car la mission peut etre en cours et n'a pas forcement une date de fin
        [Display(Name = "Fin")]
        public DateTime? MissionEnd { get; set; }
        [Display(Name = "Taux Journalier Moyen")]
        public float Tjm { get; set; }
        [Display(Name = "Type de contrat")]
        public MissionTypeEnum MissionType { get; set; }

		//pour creer une relation one one entre Mission et Activity
		//Fonctionne ==> Si on ajoute dans la table Activity une nouvelle Activité avec le meme MissionId, le premier MissionId devient null
		//Sans cette ligne on peut avoir deux Activity avec le meme MissionId
		public Activity Activity { get; set; }
        
    }
}

