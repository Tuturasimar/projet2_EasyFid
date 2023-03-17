using System;
using System.ComponentModel.DataAnnotations;

namespace Projet2_EasyFid.Models
{
    // Classe pour notre entreprise (BeMyTech)
	public class Company
	{
        public int Id { get; set; }

        [MaxLength(25)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Adress { get; set; }

        public float TotalCA { get; set; }
    }
}

