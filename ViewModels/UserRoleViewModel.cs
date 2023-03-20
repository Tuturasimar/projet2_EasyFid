using System;
using System.Collections.Generic;
using Projet2_EasyFid.Models;

namespace Projet2_EasyFid.ViewModels
{
	public class UserRoleViewModel
	{
		public User User { get; set; }
		public List<RoleUser> RolesUser { get; set; }
		public List<Role> Roles { get; set; }
	}
}

