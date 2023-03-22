using Projet2_EasyFid.Data.Enums;

namespace Projet2_EasyFid.Models
{
    public class Formation
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public FormationStatusEnum FormationStatus { get; set; }
        public LocationFormationEnum LocationFormation { get; set; }

        //Pour creer une relation one one entre Activity et Formation 
        public Activity Activity { get; set; }
    }
}
