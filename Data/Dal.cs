using System;
using Projet2_EasyFid.Models;
using System.Collections.Generic;
using Projet2_EasyFid.Data.Services;
using System.Security.Cryptography;
using System.Linq;
using Projet2_EasyFid.Data.Enums;
using System.Text;
using Microsoft.VisualBasic;
using Projet2_EasyFid.ViewModels;

namespace Projet2_EasyFid.Data
{
    public class Dal : IDal
    {
        private BddContext _bddContext;
        public Dal()
        {
            _bddContext = new BddContext();
        }
        public List<Statistic> GetAllStatistics()
        {
            return StatisticsServices.GetAllStatistics(_bddContext);
        }

        public List<Cra> GetAllCras()
        {
            return _bddContext.Cras.ToList();
        }

        public List<Cra> GetAllCrasByUserId(int id)
        {
            return CraServices.GetAllCrasByUserId(_bddContext, id);
        }

        public List<Mission> GetAllMissions()
        {
            return MissionServices.GetAllMissions(_bddContext);
        }
        public List<Formation> GetAllFormations()
        {
            return FormationServices.GetAllFormations(_bddContext);
        }

        public void SetUserIdNullOnDelete(Cra cra)
        {
            CraServices.SetUserIdNullOnDelete(_bddContext, cra);
        }

        public int CreateCra(Cra cra)
        {
            return CraServices.CreateCra(_bddContext, cra);
        }
        public int CreateMission(Mission mission)
        {
            return MissionServices.CreateMission(_bddContext, mission);
        }

        public int CreateFormation(Formation formation)
        {
            return FormationServices.CreateFormation(_bddContext, formation);
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

        //public User GetMissionById(int id)
        //{
        //    return MissionServices.GetMissionById(_bddContext, id);
        //}

        // Méthode pour authentifier un utilisateur (vérification du login et du mdp)
        public User Authentifier(string login, string password)
        {
            string encryptedPassword = EncodeMD5(password);
            User user = this._bddContext.Users.FirstOrDefault(u => u.Login == login && u.Password == encryptedPassword);
            return user;
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

        public List<User> GetAllUsersByManagerId(int id)
        {
            return UserServices.GetAllUsersByManagerId(_bddContext, id);
        }

        public User GetUserById(int id)
        {
            return UserServices.GetUserById(_bddContext, id);
        }

        public User GetUserByUserDataId(int id)
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
            return UserServices.GetAllRolesById(_bddContext, id);
        }


        public Boolean checkUserRole(int userId, RoleTypeEnum role)
        {
            return UserServices.GetAllRolesById(_bddContext, userId).Find(r => r.RoleType == role) != null;
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

        public void SetManagerIdNullOnDelete(User user)
        {
            UserServices.SetManagerIdNullOnDelete(_bddContext, user);
        }

        public void DeleteAllRoleUsersByUserId(int idUser)
        {
            UserServices.DeleteAllRoleUsersByUserId(_bddContext, idUser);
        }

        public void CreateActivity(Activity activity)
        {
            ActivityServices.CreateActivity(_bddContext, activity);
        }
        public void DeleteUserById(int id)
        {
            UserServices.DeleteUserById(_bddContext, id);
        }

        public void DeleteUserDataById(int id)
        {
            UserServices.DeleteUserDataById(_bddContext, id);
        }

        
        //public List<Formation> GetAllFormations()
        //{
        //    return CraServices.GetAllFormations(_bddContext);
        //}
        

        public void UpdateFormation(Formation formation)
        {
            this._bddContext.Formations.Update(formation);
            this._bddContext.SaveChanges();
        }
        public List <Activity> GetAllActivities()
        {
            return CraServices.GetAllActivities(_bddContext);
        }

        
        public Mission GetMissionById(int id)
        {
            return CraServices.GetMissionById(_bddContext, id);
        }

        public List<MissionUser> GetAllActiveMissionsByUserId(int id)
        {
            return MissionServices.GetAllActiveMissionsByUserId(_bddContext, id);
        }


        public Activity GetActivityById(int id)
        {
            return CraServices.GetActivityById(_bddContext, id);
        }

        public ActivityDate GetActivityDateById (int id)
        {
            return CraServices.GetActivityDateById(_bddContext, id);
        }

        public int CreateActivityDate (ActivityDate activityDate)
        {
            return CraServices.CreateActivityDate(_bddContext, activityDate);
        }

        public int CreateCraActivity (CraActivity craActivity)
        {
            return CraServices.CreateCraActivity(_bddContext, craActivity);
        }

        public Cra GetCraById(int id)
        {
            return CraServices.GetCraById(_bddContext, id);
        }

        
        public CraActivity GetCraActivityByCraId(int id)
        {
            return CraServices.GetCraActivityByCraId(_bddContext, id);
        }


        public List<Activity> GetAllActivityByCraId(int id)
        {
            return CraServices.GetAllActivityByCraId (_bddContext, id);
        }

        public List<ActivityDate> GetAllActivityDateByCraId(int id)
        {
            return ActivityServices.GetAllActivityDateByCraId(_bddContext, id);
        }

        //Pour reucperer tous les BeginDate d'une ActivityDate
        //Pas utile pour l'instant, à voir pour la suite, je laisse en commentaire pour l'instant
        /*
        public List<DateTime> GetBeginDate(int id)
        {
            return CraServices.GetBeginDate(_bddContext, id);
        }
        */

        public UserFeedback GetUserFeedbackById(int id)
        {
            return UserServices.GetUserFeedbackById(_bddContext, id);
        }

        public void ModifyUserFeedback(UserFeedback userFeedback)
        {
            UserServices.ModifyUserFeedback(_bddContext,userFeedback);
        }

        public int CreateUserFeedback(UserFeedback userFeedback)
        {
            return UserServices.CreateUserFeedback(_bddContext, userFeedback);
        }

        public void ModifyMissionUser(MissionUser missionUser)
        {
            MissionServices.ModifyMissionUser(_bddContext, missionUser);
        }

        public MissionUser GetMissionUserById(int id)
        {
            return MissionServices.GetMissionUserById(_bddContext, id);
        }


        public List<Mission> GetAllMissionUserByUserId(int id)
        {
            return CraServices.GetAllMissionUserByUserId(_bddContext, id);
        }


        public List<Activity> GetAllActivityByUserId(int id)
        {
            return CraServices.GetAllActivityByUserId(_bddContext, id);
        }

        public List<Activity> GetAllFormationAndAbsence()
        {
            return CraServices.GetAllFormationAndAbsence(_bddContext);
        }

        public bool CheckActivityDateComptability(List<DateTime> BeginDate, List<DateTime> EndDate, List<int> activities, User user)
        {
            return ActivityServices.CheckActivityDateComptability(_bddContext, BeginDate, EndDate, activities, user);
        }

        public List<Notification> GetAllNotificationsByUserId(int id)
        {
            return NotificationServices.GetAllNotificationsByUserId(_bddContext, id);
        }


        public List<Cra> GetAllInHoldAndValidatedCrasByUserId(int id)
        {
            return CraServices.GetAllInHoldAndValidatedCrasByUserId(_bddContext, id);
        }

        public List<CraActivity> GetAllCraActivityByCraId(int id)
        {
            return CraServices.GetAllCraActivityByCraId(_bddContext, id);
        }



        public List<ActivityDate> GetAllActivityDateByActivityIdAndCraId(int idActivity, int idCra)
        {
            return ActivityServices.GetAllActivityDateByActivityIdAndCraId(_bddContext, idActivity, idCra);
        }

        public void ModifyCra(Cra cra)
        {
            CraServices.ModifyCra(_bddContext,cra);
        }

        public void CreateNotification(Notification notification)
        {
            NotificationServices.CreateNotification(_bddContext,notification);
        }

        public void DeleteNotification(Notification notification)
        {
            NotificationServices.DeleteNotification(_bddContext, notification);
        }

        public Notification GetNotificationById(int id)
        {
            return NotificationServices.GetNotificationById(_bddContext, id);
        }

        public Activity GetActivityByMissionId(int id)
        {
            return ActivityServices.GetActivityByMissionId(_bddContext,id);
        }

        public List<Activity> GetAllAbsenceAndFormation()
        {
            return ActivityServices.GetAllAbsenceAndFormation(_bddContext);
        }

        public void DeleteCraActivity(CraActivity craActivity)
        {
            CraServices.DeleteCraActivity(_bddContext, craActivity);
        }

        public void DeleteActivityDate(ActivityDate activityDate)
        {
            ActivityServices.DeleteActivityDate(_bddContext,activityDate);
        }

        public List<User> GetAllUsersButNotTheAdmin(int id)
        {
            return UserServices.GetAllUsersButNotTheAdmin(_bddContext, id);
        }
    }
}

