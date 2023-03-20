using System;
using System.Collections.Generic;
using System.Linq;
using Projet2_EasyFid.Models;

namespace Projet2_EasyFid.Data.Services
{
	public static class UserServices
	{

		public static List<User> GetAllUsers(BddContext _bddContext)
		{
            return _bddContext.Users.ToList();
        }
    }
}

