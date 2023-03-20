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
    }
}

