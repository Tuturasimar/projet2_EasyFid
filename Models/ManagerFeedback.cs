using System;
namespace Projet2_EasyFid.Models
{
	// Classe qui contient les attributs pour les impressions du manager (et du client)
	public class ManagerFeedback
	{
		public int Id { get; set; }

		public string Comment { get; set; }
		public string Opportunities { get; set; }
		public int GradeSatisfaction { get; set; }

	}
}

