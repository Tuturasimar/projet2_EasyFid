using System;
using Microsoft.EntityFrameworkCore;
using Projet2_EasyFid.Models;

namespace Projet2_EasyFid.Data
{
	public class BddContext : DbContext
	{

        public DbSet<User> Users { get; set; }
        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Les paramètres du serveur changent en fonction des configurations personnelles
            optionsBuilder.UseMySql("server=localhost;port=8889;user id=root;password=root;database=easyFid"); // connexion trévor
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
                new UserData { Id = 1, Lastname = "Xuxu", Firstname = "Xaxa", Birthday = new DateTime(2018, 12, 4), Email = "xxxx@gmail.com" },
                new UserData { Id = 2, Lastname = "Watson", Firstname = "Bobby", Birthday = new DateTime(2015, 5, 28), Email = "bobby.watson@gmail.com" },
                new UserData { Id = 3, Lastname = "Multipass", Firstname = "Lilou", Birthday = new DateTime(2019, 6, 18), Email = "lilou@gmail.com" }
                );

            // Dans la table Users
            this.Users.AddRange(
                new User { Id = 1, Login = "xxxxx", Password = "ppppp", CreationDate = DateTime.Now, CompanyId = 1, UserDataId = 1 },
                new User { Id = 2, Login = "Bob", Password = "ppppp", CreationDate = DateTime.Now, CompanyId = 1, UserDataId = 2, ManagerId=1 },
                new User { Id = 3, Login = "Lilou", Password = "ppppp", CreationDate = DateTime.Now, CompanyId = 1, UserDataId = 3, ManagerId=1 }
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

