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
        public DbSet<Formation> Formations { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<ActivityDate> ActivityDates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Les paramètres du serveur changent en fonction des configurations personnelles

            //optionsBuilder.UseMySql("server=localhost;port=8889;user id=root;password=root;database=easyFid"); // connexion trévor
            optionsBuilder.UseMySql("server=localhost;user id=root;password=rrrrr;database=projet2"); //connexion Laura
            //optionsBuilder.UseMySql("server=localhost;user id=root;password=rrrrrrrr;database=UserData"); //connexion Louis

            //optionsBuilder.UseMySql("server=localhost;user id=root;password=root;database=easyFid"); //connexion Seb


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
                new User { Id = 1, Login = "xxxxx", Password = Dal.EncodeMD5("ppppp"), CreationDate = DateTime.Now, CompanyId = 1, UserDataId = 1, JobEnum = JobEnum.Developpeur },
                new User { Id = 2, Login = "Bob", Password = Dal.EncodeMD5("ppppp"), CreationDate = DateTime.Now, CompanyId = 1, UserDataId = 2, ManagerId = 1, JobEnum = JobEnum.ChefDeProjet },
                new User { Id = 3, Login = "Lilou", Password = Dal.EncodeMD5("ppppp"), CreationDate = DateTime.Now, CompanyId = 1, UserDataId = 3, JobEnum = JobEnum.Administrateur },
                new User { Id = 4, Login = "test", Password = Dal.EncodeMD5("ppppp"), CreationDate = DateTime.Now, CompanyId = 1, UserDataId = 4, JobEnum = JobEnum.Commercial }
                );
                
            //Dans la tables cras
            this.Cras.AddRange(
                new Cra { Id = 1, CreatedAt = new DateTime(2020, 06, 20), UpdatedAt = new DateTime(2020, 06, 27), StateCra = StateEnum.VALIDATED, UserId = 1},
                new Cra { Id = 2, CreatedAt = DateTime.Now, StateCra = StateEnum.VALIDATED, UserId = 2},
                new Cra { Id = 3, CreatedAt = DateTime.Now, UpdatedAt = new DateTime(2021, 03, 01), StateCra = StateEnum.CREATED, UserId = 1 }
                );

            //Dans la table Missions
            this.Missions.AddRange(
                new Mission {Id = 1, Name = "Sanofi", MissionStart = new DateTime(2020, 06, 10), MissionEnd = new DateTime(2020, 06, 24), Tjm = 630 , MissionType = MissionTypeEnum.FORFAIT},
                new Mission {Id = 2, Name = "Firmenich", MissionStart = new DateTime(2020, 02, 01), MissionEnd = new DateTime(2021, 01, 01), Tjm = 670, MissionType = MissionTypeEnum.FORFAIT},
                new Mission { Id = 3, Name = "RechercheContrat", MissionStart = new DateTime(2022, 09, 01), Tjm = 450, MissionType = MissionTypeEnum.INTERCONTRAT},
                new Mission { Id = 4, Name = "Total", MissionStart = new DateTime(2022, 10, 01), Tjm = 500, MissionType = MissionTypeEnum.FORFAIT },
                new Mission { Id = 5, Name = "Vivendi", MissionStart = new DateTime(2022, 07, 05), MissionEnd = new DateTime(2021, 08, 12), Tjm = 500, MissionType = MissionTypeEnum.FORFAIT },
                new Mission { Id = 6, Name = "Renault", MissionStart = new DateTime(2022, 06, 15), MissionEnd = new DateTime(2021, 06, 15),Tjm = 490, MissionType = MissionTypeEnum.FORFAIT }
                );

            //Dans la table Formation
            this.Formations.AddRange(
                new Formation { Id = 1, Name ="Formation Incendie", FormationStatus = FormationStatusEnum.GIVEN, LocationFormation = LocationFormationEnum.EXTERN},
                new Formation { Id = 2, Name = "Formation nouveau logiciel", FormationStatus = FormationStatusEnum.FOLLOWED, LocationFormation = LocationFormationEnum.INTERN }
                );

            //Dans la table Absence
            this.Absences.AddRange(
                new Absence { Id = 1, AbsenceType = AbsenceTypeEnum.DISEASE},
                new Absence { Id = 2, AbsenceType = AbsenceTypeEnum.HOLIDAY }
                );

            //Dans la table Activities
            this.Activities.AddRange(
                new Activity {Id = 1, LabelActivity = "Sanofi", MissionId = 1},
                new Activity {Id = 2,  LabelActivity = "Firmenich", MissionId = 2},
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
                new CraActivity { Id = 1,  CraId = 1, ActivityId =  1 },
                new CraActivity { Id =2, CraId = 2, ActivityId = 2}
                );


            //Dans la table ActivityDate
            this.ActivityDates.AddRange(
                new ActivityDate { Id = 1, BeginDate = new DateTime(2022, 04, 05), EndDate = new DateTime (2022, 04, 10), CraActivityId = 1},
                new ActivityDate { Id = 2, BeginDate = new DateTime(2023, 03, 21), CraActivityId = 2 },
                new ActivityDate { Id =3, BeginDate = new DateTime(2022, 04, 11), EndDate = new DateTime(2022, 04, 16), CraActivityId = 1}
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
                new Notification { MessageContent = "Cra refusée, il manque des données sur les jours du 12 au 14 février", ClassContext = "danger", UserId = 1 }
                );

            // Sauvegarde des données dans la BDD
            this.SaveChanges();

        }
    }
}


