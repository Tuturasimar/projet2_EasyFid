using System;
using Projet2_EasyFid.Data.Enums;

namespace Projet2_EasyFid.Models
{
	public class Mission
	{
		public string Name { get; set; }
		// public string Adress;
		public DateTime MissionStart { get; set; }
		public DateTime MissionEnd { get; set; }
		public float Tjm { get; set; }
		public MissionType MissionType { get; set; }
	}
}

