using System;
using System.ComponentModel.DataAnnotations;

namespace Projet2_EasyFid.Models
{
	public class UserData
	{
        public int Id { get; set; }

        [Required(ErrorMessage = "Renseignez votre nom.")]
        [MaxLength(25, ErrorMessage = "Ce champ contient 25 caractères au maximum.")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Renseignez votre prénom.")]
        [MaxLength(25, ErrorMessage = "Ce champ contient 25 caractères au maximum.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage ="Renseignez votre date de naissance.")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Renseignez votre adresse mail.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage ="Veuillez renseigner une adresse email valide.")]
        public string Email { get; set; }

        //creation d'une relation one one entre la table User et la table UserData
        public User user { get; set; }

    }
}

