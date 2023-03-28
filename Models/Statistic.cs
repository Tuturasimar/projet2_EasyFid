using System;
namespace Projet2_EasyFid.Models
{
	// Classe qui regroupe les statistiques d'un CRA
	public class Statistic
	{
	
		
			public int Id { get; set; }	
			public float CA { get; set; }
			public float Facturation { get; set; }
			public float CJM { get; set; }
			public float Merge { get; set; }
			public float TjmRegie { get; set; }
			public float TACE { get; set; }
			public int? MissionId { get; set; }
			public virtual Mission Mission { get; set; }


    }
}

