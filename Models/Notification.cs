using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Projet2_EasyFid.Models
{
    public class Notification
	{
        public int Id { get; set; }

        [Display(Name = "Message de refus")]
        [MaxLength(70, ErrorMessage = "Votre message doit contenir 70 caractères maximum.")]
        [Required(ErrorMessage = "Un message doit automatiquement accompagner un refus de CRA")]
        public string MessageContent { get; set; }
        public string ClassContext { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}

