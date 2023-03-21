namespace Projet2_EasyFid.Models
{
    public class CraActivity
    {
        //Table qui relie la table Activity et la table Cra (relation plyusieurs à plusieurs)
        //Une activity peut être reliee a plusieurs Cra et un Cra peut etre relie a plusieurs Activity
        public int Id { get; set; }

        //On cree une clef etrangere vers la table Cra
        public int CraId { get; set; }
        public virtual Cra Cra { get; set; }    

        //On cree une clef etrangere vers la table Activity
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
    }
}
