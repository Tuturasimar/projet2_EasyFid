using System;
using System.ComponentModel.DataAnnotations;

namespace Projet2_EasyFid.Models
{
    public class ActivityDate
    {
        public int Id { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? HalfTime { get; set; }

        //clef etrangere vers la table CraActivity
        public int CraActivityId { get; set; }
        public virtual CraActivity CraActivity { get; set;}

    }
}
