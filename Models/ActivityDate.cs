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

        //clef etrangere vers la table Activity 
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
    }
}
