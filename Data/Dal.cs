using System;
using Projet2_EasyFid.Models;
using System.Collections.Generic;
using Projet2_EasyFid.Data.Services;

namespace Projet2_EasyFid.Data
{
	public class Dal : IDal
	{
		private BddContext _bddContext;
		public Dal()
		{
			_bddContext = new BddContext();
		}

        public void Dispose()
        {
            _bddContext.Dispose();
        }

        public List<User> GetAllUsers()
        {
            return UserServices.GetAllUsers(_bddContext);
        }

        public User GetUserById(int id)
        {
            return UserServices.GetUserById(_bddContext, id);
        }

        public void ModifyUser(User user)
        {
            UserServices.ModifyUser(_bddContext, user);
        }

        public int CreateUser(User user)
        {
            return UserServices.CreateUser(_bddContext, user);
        }

        public int CreateUserData(UserData userData)
        {
            return UserServices.CreateUserData(_bddContext, userData);
        }

       
    }
}

