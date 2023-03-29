using System;
using Microsoft.EntityFrameworkCore;
using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;

namespace Projet2_EasyFid.Data
{
    public class BddContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<Cra> Cras { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<CraActivity> CraActivities { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<MissionUser>MissionUsers { get; set; }
        public DbSet<Formation> Formations { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<ActivityDate> ActivityDates { get; set; }


        public DbSet<Statistic> Statistics { get; set; }




        public DbSet<UserFeedback> UserFeedbacks { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Les paramètres du serveur changent en fonction des configurations personnelles


            //optionsBuilder.UseMySql("server=localhost;port=8889;user id=root;password=root;database=easyFid"); // connexion trévor
            //optionsBuilder.UseMySql("server=localhost;user id=root;password=rrrrr;database=projet2"); //connexion Laura

            //optionsBuilder.UseMySql("server=localhost;user id=root;password=rrrrrrrr;database=UserData"); //connexion Louis
            optionsBuilder.UseMySql("server=localhost;user id=root;password=root;database=easyFid"); //connexion Seb

        }
        
        public void InitialiseDb()
        {
            // Suppression et création de la BDD
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();

            // Ajout des données factices \\

            // Dans la table Companies
            this.Companies.Add(new Company { Id = 1, Name = "Isika", Adress = "25 rue de la Boustifaille 75000 Paris", TotalCA = 200000 });

            // Dans la table UserDatas
            this.UserDatas.AddRange(

                new UserData { Id = 1, Lastname = "Xuxu", Firstname = "Xaxa", Birthday = new DateTime(2018, 12, 4), Email = "xaxa@isika.com"},
                new UserData { Id = 2, Lastname = "Watson", Firstname = "Bobby", Birthday = new DateTime(2015, 5, 28), Email = "bobby@isika.com" },
                new UserData { Id = 3, Lastname = "Multipass", Firstname = "Lilou", Birthday = new DateTime(2019, 6, 18), Email = "lilou@isika.com" },
                new UserData { Id = 4, Lastname = "JeSuisManager", Firstname = "Henry", Birthday = new DateTime(2011, 6, 18), Email = "manager@gmail.com" }
                );

            // Dans la table Users
            this.Users.AddRange(
                new User { Id = 1, Login = "xxxxx", Password = Dal.EncodeMD5("ppppp"), CreationDate = DateTime.Now, CompanyId = 1, UserDataId = 1, ManagerId = 4, JobEnum = JobEnum.Developpeur },
                new User { Id = 2, Login = "Bob", Password = Dal.EncodeMD5("ppppp"), CreationDate = DateTime.Now, CompanyId = 1, UserDataId = 2, ManagerId = 4, JobEnum = JobEnum.ChefDeProjet },
                new User { Id = 3, Login = "Lilou", Password = Dal.EncodeMD5("ppppp"), CreationDate = DateTime.Now, CompanyId = 1, UserDataId = 3, JobEnum = JobEnum.Administrateur },
                new User { Id = 4, Login = "Kevin", Password = Dal.EncodeMD5("ppppp"), CreationDate = DateTime.Now, CompanyId = 1, UserDataId = 4, JobEnum = JobEnum.Commercial }
                );
                
            //Dans la tables cras
            this.Cras.AddRange(

                new Cra { Id = 1, CreatedAt = new DateTime(2022, 06, 20), UpdatedAt = new DateTime(2022, 06, 27), StateCra = StateEnum.VALIDATED, UserId = 1 },
                new Cra { Id = 2, CreatedAt = new DateTime(2022, 05, 12), UpdatedAt= new DateTime(2022, 05, 23), StateCra = StateEnum.VALIDATED, UserId = 2 },
                new Cra { Id = 3, CreatedAt = new DateTime(2022, 06, 19), UpdatedAt = new DateTime(2022, 06, 27), StateCra = StateEnum.VALIDATED, UserId = 2 },
                new Cra { Id = 4, CreatedAt = new DateTime(2023, 03, 18), UpdatedAt = new DateTime(2023, 03, 25), StateCra = StateEnum.INHOLD, UserId = 1 },
                new Cra { Id = 5, CreatedAt = new DateTime(2023, 03, 10), UpdatedAt = new DateTime(2023, 03, 27), StateCra = StateEnum.DRAFT, UserId = 2 }

                );

            //Dans la table Missions
            this.Missions.AddRange(
                new Mission { Id = 1, Name = "Sanofi", MissionStart = new DateTime(2022, 06, 10), MissionEnd = new DateTime(2022, 06, 24), Tjm = 630 , MissionType = MissionTypeEnum.FORFAIT, MissionState = MissionStateEnum.FINISHED},
                new Mission { Id = 2, Name = "Firmenich", MissionStart = new DateTime(2022, 02, 01), MissionEnd = new DateTime(2022, 07, 01), Tjm = 670, MissionType = MissionTypeEnum.FORFAIT, MissionState = MissionStateEnum.FINISHED},
                new Mission { Id = 3, Name = "RechercheContrat", MissionStart = new DateTime(2022, 09, 01), MissionEnd = new DateTime(2050,12,25), Tjm = 450, MissionType = MissionTypeEnum.INTERCONTRAT, MissionState = MissionStateEnum.ACTIVE},
                new Mission { Id = 4, Name = "Total", MissionStart = new DateTime(2022, 03, 01), Tjm = 500, MissionEnd = new DateTime(2023,03,10), MissionType = MissionTypeEnum.FORFAIT, MissionState = MissionStateEnum.ACTIVE},
                new Mission { Id = 5, Name = "Vivendi", MissionStart = new DateTime(2023, 02, 20), MissionEnd = new DateTime(2024, 08, 12), Tjm = 500, MissionType = MissionTypeEnum.FORFAIT, MissionState = MissionStateEnum.ACTIVE},
                new Mission { Id = 6, Name = "Renault", MissionStart = new DateTime(2023, 03, 01), MissionEnd = new DateTime(2024, 06, 15),Tjm = 490, MissionType = MissionTypeEnum.FORFAIT, MissionState = MissionStateEnum.ACTIVE }
                );

            //Dans la table Formation
            this.Formations.AddRange(
                new Formation { Id = 1, Name ="Formation Incendie", NbOfDays=1, FormationStatus = FormationStatusEnum.GIVEN, LocationFormation = LocationFormationEnum.EXTERN,Description="Formation réglementaire visant à sensibiliser les salariés au risque incendie" },
                new Formation { Id = 2, Name = "Formation nouveau logiciel",NbOfDays=4, FormationStatus = FormationStatusEnum.FOLLOWED, LocationFormation = LocationFormationEnum.INTERN,Description="Formation JEE permettant d'améliorer une application de gestion du compte rendu d'activité"  }
                );

            //Dans la table Absence
            this.Absences.AddRange(
                new Absence { Id = 1, AbsenceType = AbsenceTypeEnum.DISEASE},
                new Absence { Id = 2, AbsenceType = AbsenceTypeEnum.HOLIDAY }
                );

            //Dans la table Activities
            this.Activities.AddRange(
                new Activity { Id = 1, LabelActivity = "Sanofi", MissionId = 1},
                new Activity { Id = 2, LabelActivity = "Firmenich", MissionId = 2},
                new Activity { Id = 3, LabelActivity = "Maladie", AbsenceId = 1},
                new Activity { Id = 4, LabelActivity = "Congé", AbsenceId = 2},
                new Activity { Id = 5, LabelActivity = "Formation Incendie", FormationId = 1 },
                new Activity { Id = 6, LabelActivity = "Formation nouveau logiciel",  FormationId = 2 },
                new Activity { Id = 7, LabelActivity = "Recherche Contrat", MissionId = 3 },
                new Activity { Id = 8, LabelActivity = "Total", MissionId = 4 },
                new Activity { Id = 9, LabelActivity = "Vivendi", MissionId = 5 },
                new Activity { Id = 10, LabelActivity = "Renault", MissionId = 6 }
                );

             //Ajout des liens entre des cles etrangeres (cra et activity) dans la table CraActivity
             this.CraActivities.AddRange(
                new CraActivity { Id = 1, CraId = 1, ActivityId = 1 },
                new CraActivity { Id = 2, CraId = 1, ActivityId = 3 },
                new CraActivity { Id = 3, CraId = 2, ActivityId = 2 },
                new CraActivity { Id = 4, CraId = 3, ActivityId = 2 },
                new CraActivity { Id = 5, CraId = 4, ActivityId = 4 },
                new CraActivity { Id = 6, CraId = 4, ActivityId = 7 },
                new CraActivity { Id = 7, CraId = 4, ActivityId = 8 },
                new CraActivity { Id = 8, CraId = 5, ActivityId = 8 },
                new CraActivity { Id = 9, CraId = 5, ActivityId = 10 }
                );

            //Dans la table ActivityDate
            this.ActivityDates.AddRange(
                new ActivityDate { Id = 1, BeginDate = new DateTime(2022, 06, 10), EndDate = new DateTime(2022, 06, 24), CraActivityId = 1 },
                new ActivityDate { Id = 2, BeginDate = new DateTime(2022, 06, 01), EndDate = new DateTime(2022, 06, 09), CraActivityId = 2 },
                new ActivityDate { Id = 3, BeginDate = new DateTime(2022, 06, 25), EndDate = new DateTime(2022, 06, 30), CraActivityId = 2 },
                new ActivityDate { Id = 4, BeginDate = new DateTime(2022, 05, 01), EndDate = new DateTime(2022, 05, 31), CraActivityId = 3 },
                new ActivityDate { Id = 5, BeginDate = new DateTime(2022, 06, 01), EndDate = new DateTime(2022, 06, 30), CraActivityId = 4 },
                new ActivityDate { Id = 6, BeginDate = new DateTime(2023, 03, 11), EndDate = new DateTime(2023, 03, 20), CraActivityId = 5 },
                new ActivityDate { Id = 7, BeginDate = new DateTime(2023, 03, 21), EndDate = new DateTime(2023, 03, 31), CraActivityId = 6 },
                new ActivityDate { Id = 8, BeginDate = new DateTime(2023, 03, 01), EndDate = new DateTime(2023, 03, 10), CraActivityId = 7 },
                new ActivityDate { Id = 9, BeginDate = new DateTime(2023, 03, 01), EndDate = new DateTime(2023, 03, 10), CraActivityId = 8 },
                new ActivityDate { Id = 10, BeginDate = new DateTime(2023, 03, 11), EndDate = new DateTime(2023, 03, 31), CraActivityId = 9 }
                );

            // Ajout de liens entre des clés étrangères (user et role) dans la table RoleUsers
            this.RoleUsers.AddRange(
                new RoleUser { UserId = 1, RoleType= RoleTypeEnum.SALARIE},
                new RoleUser { UserId = 1, RoleType = RoleTypeEnum.MANAGER },
                new RoleUser { UserId = 2, RoleType = RoleTypeEnum.SALARIE },
                new RoleUser { UserId = 3, RoleType = RoleTypeEnum.ADMIN },
                new RoleUser { UserId = 4, RoleType = RoleTypeEnum.MANAGER}
                );

            // Dans la table Notifications
            this.Notifications.AddRange(
                new Notification { MessageContent = "Cra validé avec succès", ClassContext = "success", UserId = 1 },
                new Notification { MessageContent = "Cra refusé, il manque des données sur les jours du 12 au 14 février", ClassContext = "danger", UserId = 1 }
                );

            // Dans la table UserFeedback
            this.UserFeedbacks.AddRange(
                new UserFeedback {Id = 1, Comment = "RAS", GradeClientRelation = 3, GradeManager = 2, GradeMission = 4, GradeUserComfort = 5},
                new UserFeedback {Id = 2, Comment = "Relation tendue avec le client. Bonne écoute de mon manager", GradeClientRelation = 1, GradeManager = 5, GradeMission = 3, GradeUserComfort = 3},
                new UserFeedback {Id = 3, Comment = "Mission pénible sans aucune structure", GradeClientRelation = 2, GradeManager = 4, GradeMission = 0, GradeUserComfort = 1},
                new UserFeedback {Id = 4, Comment = "Mission difficile, client sans considération", GradeClientRelation = 0, GradeManager = 5, GradeMission = 0, GradeUserComfort = 1}
                );

            // Dans la table MissionUser
            this.MissionUsers.AddRange(
                new MissionUser { Id = 1, UserId = 1, MissionId = 1, MissionState= MissionStateEnum.FINISHED, UserFeedbackId = 1 },
                new MissionUser { Id = 2, UserId = 2, MissionId = 2, MissionState= MissionStateEnum.FINISHED, UserFeedbackId = 2 },
                new MissionUser { Id = 3, UserId = 1, MissionId = 4, MissionState= MissionStateEnum.ACTIVE, UserFeedbackId = 3 },
                new MissionUser { Id = 4, UserId = 2, MissionId = 4, MissionState= MissionStateEnum.ACTIVE, UserFeedbackId = 4 },
                new MissionUser { Id = 5, UserId = 2, MissionId = 6, MissionState= MissionStateEnum.ACTIVE}
                );

            // Dans la table MissionUser
            this.Statistics.AddRange(
                new Statistic { Id = 1, Ca = 35000, Facturation = 25000, CJM = 150, Merge = 45, TjmRegie = 1000, TACE = 85,Date= new DateTime(2020, 06, 24),Label="Absent", MissionId = 1 },
                new Statistic { Id = 2, Ca = 28000, Facturation = 28000, CJM = 130, Merge = 47, TjmRegie = 1100, TACE = 86, Date= new DateTime(2021, 01, 01), Label = "present", MissionId = 2 },
                new Statistic { Id = 3, Ca = 23000, Facturation = 23000, CJM = 145, Merge = 43, TjmRegie = 950, TACE = 84, Date = new DateTime(2021, 08, 15), Label = "present", MissionId = 3 },
                new Statistic { Id = 4, Ca = 30000, Facturation = 26000, CJM = 160, Merge = 46, TjmRegie = 1050, TACE = 89, Date = new DateTime(2021, 09, 12), Label = "present", MissionId = 4 },
                new Statistic { Id = 5, Ca = 31000, Facturation = 31000, CJM = 155, Merge = 48, TjmRegie = 950, TACE = 83, Date=new DateTime(2021, 08, 12), Label = "present", MissionId = 5 },
                new Statistic { Id = 6, Ca = 24000, Facturation = 20000, CJM = 132, Merge = 44, TjmRegie = 1000, TACE = 84, Date=new DateTime(2021, 06, 15), Label = "present", MissionId = 6 }
                ) ;


            // Sauvegarde des données dans la BDD
            this.SaveChanges();

        }
    }
}


