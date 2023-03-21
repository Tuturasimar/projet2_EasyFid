using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;

namespace Projet2_EasyFid.Data.Services
{
	public static class UserServices
	{

		public static List<User> GetAllUsers(BddContext _bddContext)
		{
            return _bddContext.Users.Include(u => u.UserData).ToList();
        }

		public static User GetUserById(BddContext _bddContext, int id)
		{
            // Le Include permet ici de récupérer les données du UserData (qui est lié à User par une clé étrangère)
            // Sans Include, impossible de récupérer certaines données en faisant User.Userdata.FirstName, par exemple.
            User user = _bddContext.Users.Include(u => u.UserData).SingleOrDefault(u => u.Id == id);
            return user;
        }

        public static void ModifyUser(BddContext _bddContext, User user)
        {
            // Update permet de mettre à jour directement le bon User dans la table (grâce à l'id sans doute)
            _bddContext.Users.Update(user);
            _bddContext.SaveChanges();
        }

        public static int CreateUser(BddContext _bddContext, User user)
        {
            _bddContext.Users.Add(user);
            _bddContext.SaveChanges();
            return user.Id;
        }

        public static int CreateUserData(BddContext _bddContext, UserData userData)
        {
            _bddContext.UserDatas.Add(userData);
            _bddContext.SaveChanges();
            return userData.Id;
        }

        public static List<RoleUser> GetAllRolesById(BddContext _bddContext,int id)
        {
          return _bddContext.RoleUsers.Where(u => u.UserId == id).ToList();
        }

        public static void CreateRoleUser(BddContext _bddContext, RoleUser roleUser)
        {
            _bddContext.RoleUsers.Add(roleUser);
            _bddContext.SaveChanges();
        }
       
        public static List<Company> GetAllCompanies(BddContext _bddContext)
        {
            return _bddContext.Companies.ToList();
        }

        public static List<UserData> GetAllUserDatas(BddContext _bddContext)
        {
            return _bddContext.UserDatas.ToList();
        }

        public static User GetUserByUserDataId(BddContext _bddContext, int id)
        {
            return _bddContext.Users.SingleOrDefault(u => u.UserDataId == id);
        }

        public static List<UserData> GetAllManagerUserDatas(BddContext _bddContext)
        {
            var query = from u in _bddContext.Users
                        join uD in _bddContext.UserDatas on u.UserDataId equals uD.Id
                        join r in _bddContext.RoleUsers on u.Id equals r.UserId
                        where r.RoleType == RoleTypeEnum.MANAGER
                        select uD;
            return query.ToList();
        }
    }
}

