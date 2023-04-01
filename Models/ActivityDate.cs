using System;
using System.ComponentModel.DataAnnotations;

namespace Projet2_EasyFid.Models
{
    public class ActivityDate
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Renseignez la date de début de l'activité.")]
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        [Required(ErrorMessage = "Renseignez la date de fin de l'activité.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public bool? HalfTime { get; set; }

        //clef etrangere vers la table CraActivity
        public int CraActivityId { get; set; }
        public virtual CraActivity CraActivity { get; set;}

    }
}
