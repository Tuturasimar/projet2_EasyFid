using Projet2_EasyFid.Data.Enums;

namespace Projet2_EasyFid.Models
{
    public class Absence
    {
        public int Id { get; set; }
        public AbsenceTypeEnum AbsenceType { get; set; }

        //Pour creer une relation one one entre la table Activity et Absence
        public Activity Activity { get; set; }
    }
}
