using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Projet2_EasyFid.Data.Enums;

namespace Projet2_EasyFid.Models
{
    // Classe qui regroupera tous les attributs relatif au CRA
	public class Cra
    {
        public int Id { get; set; }

        [Display(Name = "Date de création du Cra : ")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date de modification du Cra : ")]
        public DateTime? UpdatedAt { get; set; }

        //Pour creer une clef etrangere dans la table cra vers la table User
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public StateEnum StateCra { get; set; }

    }
}

