using System;
using Projet2_EasyFid.Models;
using System.Collections.Generic;
using Projet2_EasyFid.Data.Services;
using System.Security.Cryptography;
using System.Linq;
using Projet2_EasyFid.Data.Enums;
using System.Text;

namespace Projet2_EasyFid.Data
{
	public class Dal : IDal
	{
		private BddContext _bddContext;
		public Dal()
		{
			_bddContext = new BddContext();
		}


		public List<Cra> GetAllCras()
		{
			return _bddContext.Cras.ToList();
		}
        //Je retourne la liste des missions
        //public List<Mission> GetAllMissions() 
        //{
        //    return _bddContext.Missions.ToList();
        //}


        public int CreateCra ( Cra cra )
        {
            return CraServices.CreateCra( _bddContext, cra );  
            }
        public int CreateMission(Mission mission)
        {
            return MissionServices.CreateMission(_bddContext,mission);
        }

        public int CreateMission(int id,string name,DateTime missionStart,DateTime missionEnd,float tjm,MissionTypeEnum missionType)
        {
            //Instanciation dela nouvelle mission
            Mission mission = new Mission() { Name = name, MissionStart = missionStart, MissionEnd = missionEnd, Tjm = tjm, MissionType = missionType };
            _bddContext.Missions.Add(mission);
            _bddContext.SaveChanges();
            return mission.Id;
        }

		//Methode pour modifier un cra
		public void UpdateCra(int id, StateEnum stateCra) {
			Cra cra = _bddContext.Cras.Find(id);
			if (cra != null)
			{
				cra.Id = id;
				cra.StateCra = stateCra;
				_bddContext.SaveChanges();
			}
		}

        //Methode pour modifier une mission
        public void UpdateMission(Mission mission)
        {
            this._bddContext.Missions.Update(mission);
            this._bddContext.SaveChanges();
        }

        public User GetMissionById(int id)
        {
            return MissionServices.GetMissionById(_bddContext, id);
        }

        // Méthode pour authentifier un utilisateur (vérification du login et du mdp)
        public User Authentifier(string login, string password)
        {
            string encryptedPassword = EncodeMD5(password);
            User user = this._bddContext.Users.FirstOrDefault(u => u.Login == login && u.Password == encryptedPassword);
            return user;
        }
        public List<Mission> GetAllMissions()
        {
            return CraServices.GetAllMissions(_bddContext);
        }

        // Récupère l'utilisateur actuellement authentifié
        public User GetUser(string idStr)
        {
            int id;
            if (int.TryParse(idStr, out id))
            {
                return UserServices.GetUserById(_bddContext, id);
            }
            return null;
        }

        // Méthode pour encrypter un mot de passe
        public static string EncodeMD5(string encryptedPassword)
        {
            string encryptedPasswordSel = "Choix" + encryptedPassword + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(encryptedPasswordSel)));
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
        
        public User GetUserByUserDataId (int id)
        {
            return UserServices.GetUserByUserDataId(_bddContext, id);
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

        public List<RoleUser> GetAllRolesById(int id)
        {
            return UserServices.GetAllRolesById(_bddContext,id);
        }

        public void CreateRoleUser (RoleUser roleUser)
        {
            UserServices.CreateRoleUser(_bddContext, roleUser);
        }

        public List<Company> GetAllCompanies()
        {
            return UserServices.GetAllCompanies(_bddContext);
        }

        public List<UserData> GetAllUserDatas()
        {
            return UserServices.GetAllUserDatas(_bddContext);
        }

        public List<UserData> GetAllManagerUserDatas(int? idUser)
        {
            return UserServices.GetAllManagerUserDatas(_bddContext, idUser);
        }

        public void DeleteAllRoleUsersByUserId(int idUser)
        {
            UserServices.DeleteAllRoleUsersByUserId(_bddContext, idUser);
        }

        

        public List<Formation> GetAllFormations()
        {
            return CraServices.GetAllFormations(_bddContext);
        }

        public List <Activity> GetAllActivities()
        {
            return CraServices.GetAllActivities(_bddContext);
        }

        /*
        public Mission GetMissionById(int id)
        {
            return CraServices.GetMissionById(_bddContext, id);
        }
        */
    }
}

