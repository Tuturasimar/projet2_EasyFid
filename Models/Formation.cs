using Projet2_EasyFid.Data.Enums;

namespace Projet2_EasyFid.Models
{
    public class Formation
    {
        public int Id { get; set; } 
        public FormationStatusEnum FormationStatus { get; set; }
        public LocationFormationEnum LocationFormation { get; set; }
    }
}
