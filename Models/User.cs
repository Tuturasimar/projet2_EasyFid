using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projet2_EasyFid.Data.Enums;

namespace Projet2_EasyFid.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ce champ doit être rempli.")]
        [MinLength(3, ErrorMessage ="Le login doit avoir plus de 3 caractères.")]
        [MaxLength(20 , ErrorMessage ="Le login ne peut excéder 20 caractères.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Ce champ doit être rempli.")]
        [MaxLength(50)]
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public JobEnum JobEnum { get; set; }

        public int UserDataId { get; set; }
        public UserData UserData { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int? ManagerId { get; set; }
        public User Manager { get; set; }
        public string UserIdentityName { get; internal set; }
        public string PasswordResetToken { get; internal set; }
        public DateTime PasswordResetTokenExpiration { get; internal set; }
    }
}

