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
            return _bddContext.Users.Include(u => u.UserData).Include(u => u.Company).ToList();
        }

        public static List<User> GetAllUsersByManagerId (BddContext _bddContext, int id)
        {
            return _bddContext.Users.Where(u => u.ManagerId == id).ToList();
        }

		public static User GetUserById(BddContext _bddContext, int id)
		{
            // Le Include permet ici de récupérer les données du UserData (qui est lié à User par une clé étrangère)
            // Sans Include, impossible de récupérer certaines données en faisant User.Userdata.FirstName, par exemple.
            User user = _bddContext.Users.Include(u => u.UserData).Include(u=>u.Company).Include(u=>u.Manager).SingleOrDefault(u => u.Id == id);
            return user;
        }

        public static UserData GetUserDataById(BddContext _bddContext, int id)
        {
            return _bddContext.UserDatas.SingleOrDefault(u => u.Id == id);
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

        public static List<UserData> GetAllManagerUserDatas(BddContext _bddContext, int? idUser)
        {
            // On va réaliser ici une jointure entre 3 tables (User, UserData et RoleUser) car nous avons besoin de données d'une table
            // tout en devant utiliser des conditions utiles dans d'autres tables
            var query = from u in _bddContext.Users
                        join uD in _bddContext.UserDatas on u.UserDataId equals uD.Id   // jointure entre UserData et User
                        join r in _bddContext.RoleUsers on u.Id equals r.UserId         // jointure entre RoleUser et User
                        where r.RoleType == RoleTypeEnum.MANAGER                        // Quand le RoleType est uniquement MANAGER
                        where u.Id != idUser // On rajoute cette condition dans le cas où le profil d'un manager est modifié. Il ne faut pas qu'un manager puisse être son propre manager.                             
                        select uD; 
            return query.ToList(); // On récupère une liste de UserData
        }

        public static void DeleteAllRoleUsersByUserId(BddContext _bddContext, int userId)
        {
            // On récupère l'ensemble des roles dans la table RoleUser grâce à l'Id de l'utilisateur
            List<RoleUser> rolesToDelete = UserServices.GetAllRolesById(_bddContext,userId);
            foreach (RoleUser role in rolesToDelete)
            {
                // On boucle sur chacun de ses roles pour les supprimer
                _bddContext.RoleUsers.Remove(role);
                _bddContext.SaveChanges();
            }
        }

        public static void DeleteUserById(BddContext _bddContext, int id)
        {
            User userToDelete = UserServices.GetUserById(_bddContext,id);
            _bddContext.Users.Remove(userToDelete);
            _bddContext.SaveChanges();
        }

        public static void DeleteUserDataById(BddContext _bddContext, int id)
        {
            UserData userDataToDelete = UserServices.GetUserDataById(_bddContext,id);
            _bddContext.UserDatas.Remove(userDataToDelete);
            _bddContext.SaveChanges();
        }

        public static void SetManagerIdNullOnDelete(BddContext _bddContext, User user)
        {
            user.ManagerId = null;
            _bddContext.Users.Update(user);
            _bddContext.SaveChanges();
        }

        public static UserFeedback GetUserFeedbackById(BddContext _bddContext,int id)
        {
            return _bddContext.UserFeedbacks.SingleOrDefault(u => u.Id == id);
        }

        public static void ModifyUserFeedback(BddContext _bddContext, UserFeedback userFeedback)
        {
            _bddContext.UserFeedbacks.Update(userFeedback);
            _bddContext.SaveChanges();
        }

        public static int CreateUserFeedback(BddContext _bddContext, UserFeedback userFeedback)
        {
            _bddContext.UserFeedbacks.Add(userFeedback);
            _bddContext.SaveChanges();
            return userFeedback.Id;
        }
    }
}

