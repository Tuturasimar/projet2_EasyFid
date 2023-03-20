using System;
using Projet2_EasyFid.Data.Enums;

namespace Projet2_EasyFid.Models
{
	public class RoleUser
	{
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        //public int RoleId { get; set; }
        //public Role Role { get; set; }

        public RoleTypeEnum RoleType { get; set; }
    }
}

