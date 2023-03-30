using System;
using System.ComponentModel.DataAnnotations;
using Projet2_EasyFid.Models;

namespace Projet2_EasyFid.Models
{
    public class PasswordResetViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public DateTime? PasswordResetTokenExpiration { get; set; }
    }
}
