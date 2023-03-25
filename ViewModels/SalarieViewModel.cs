using Projet2_EasyFid.Models;
using System.Collections.Generic;

namespace Projet2_EasyFid.ViewModels
{
    public class SalarieViewModel
    {
        public List<Cra> Cras { get; set; }
        public List<Activity> Activities { get; set; }

        public Cra Cra { get; set; }    
        public Activity Activity { get; set; }
        public CraActivity CraActivity { get; set; }
        public ActivityDate ActivityDate { get; set; }
        

    }
}
