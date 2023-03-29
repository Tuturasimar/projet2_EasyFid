using Projet2_EasyFid.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Projet2_EasyFid.Models
{
    public class Formation
    {
        public int Id { get; set; }
        [MaxLength(35)]
        [Display(Name = "Nom de formation")]
        [Required(ErrorMessage = "Veuillez indiquer le nom de la Formation")]
        public string Name { get; set; }
        public int NbOfDays { get; set; }
        public FormationStatusEnum FormationStatus { get; set; }
        [MaxLength(20)]
        [Display(Name = "Lieu de formation")]
        [Required(ErrorMessage = "Veuillez indiquer le lieu de la formation")]
        public LocationFormationEnum LocationFormation { get; set; }
        [MaxLength(100)]
        [Display(Name = "Description de la formation")]
        public string Description { get; set; }
        
        //Pour creer une relation one one entre Activity et Formation 
        public Activity Activity { get; set; }
    }
}
