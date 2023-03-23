using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;

namespace Projet2_EasyFid.ViewModels
{
    public class UserViewModel
    {
        public UserRoleViewModel UserRoleViewModel { get; set; }
        public bool Authenticated { get; set; }
        public User User { get; set; }
    }
}
