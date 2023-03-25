using Projet2_EasyFid.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Projet2_EasyFid.Models
{
    public class Formation
    {
        public int Id { get; set; }
        [MaxLength(20)]
        [Display(Name = "Nom de formation")]
        [Required(ErrorMessage = "Veuillez indiquer le nom de la Formation")]
        public string Name { get; set; }
        public FormationStatusEnum FormationStatus { get; set; }
        [MaxLength(20)]
        [Display(Name = "Lieu de formation")]
        [Required(ErrorMessage = "Veuillez indiquer le lieu de la formation")]
        public LocationFormationEnum LocationFormation { get; set; }
        [Required(ErrorMessage = "Veuillez indiquer la date de début de la Formation")]
        [Display(Name = "Début")]
        public DateTime FormationStart { get; set; }
        [Required(ErrorMessage = "Veuillez indiquer la date de fin de la Formation")]
        [Display(Name = "Fin")]
        public DateTime FormationEnd { get; set; }
        //Pour creer une relation one one entre Activity et Formation 
        public Activity Activity { get; set; }
    }
}
