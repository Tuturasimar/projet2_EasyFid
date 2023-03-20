using System;
using System.ComponentModel.DataAnnotations;

namespace Projet2_EasyFid.Models
{
	public class UserData
	{
        public int Id { get; set; }

        [Required(ErrorMessage = "Ce champ doit être rempli.")]
        [MaxLength(25, ErrorMessage = "Ce champ contient 25 caractères au maximum.")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Ce champ doit être rempli.")]
        [MaxLength(25, ErrorMessage = "Ce champ contient 25 caractères au maximum.")]
        public string Firstname { get; set; }

        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Ce champ doit être rempli.")]
        public string Email { get; set; }

    }
}

