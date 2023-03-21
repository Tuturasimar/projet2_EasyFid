namespace Projet2_EasyFid.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string LabelActivity { get; set; }

        //On cree une clef etrangere vers la table Mission
        public int MissionId { get; set; }
        public virtual Mission Mission { get; set; }

    }
}
