using System.ComponentModel.DataAnnotations;

namespace Projet2_EasyFid.Models
{
    public class Activity
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string LabelActivity { get; set; }

        //On cree une clef etrangere vers la table Mission
        //On met des ? sur les 3 clefs etrangeres car seulement une seule clef devra etre remplie
        public int? MissionId { get; set; }
        public virtual Mission Mission { get; set; }

        //Vers la table Formation
        public int? FormationId { get; set; }
        public virtual Formation Formation { get; set; }
        //Vers la table Absence
        public int? AbsenceId { get; set; }
        public virtual Absence Absence { get; set; }    

        //creation d'une relation one one entre la table Activity et la table ActivityDate
        public ActivityDate ActivityDate { get; set; }

    }
}
