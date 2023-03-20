using Microsoft.EntityFrameworkCore;
using Projet2_EasyFid.Controllers;
using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Projet2_EasyFid.Data
{
	public class BddContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Cra> Cras { get; set; }

        //Methode pour se connecter a la base de donnees
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user id=root;password=rrrrr;database=projet2");
        }

        //Methode pour initialiser la base de donnees
        public void InitializeDb()
        {
            //On supprime la database si elle existe déjà
            this.Database.EnsureDeleted();  
            //On creee la database
            this.Database.EnsureCreated();

            //Ajout de cras dans la table Cra de la base de données 
            this.Cras.AddRange(
                new Cra { CreatedAt = DateTime.Now, UpdatedAt = new DateTime(2022, 03, 01), StateCra = StateEnum.CREATED, UserId = 1},
                new Cra { CreatedAt = DateTime.Now, StateCra = StateEnum.VALIDATED, UserId = 2},
                new Cra { CreatedAt = DateTime.Now, UpdatedAt = new DateTime(2021, 03, 01), StateCra = StateEnum.CREATED, UserId = 1 }
                );

            //Ajout d'User pour tester la liaison avec la table Cra
            this.Users.AddRange(
                new User { Id = 1, Login = "lolo74", Password = "lolo", CreationDate = new DateTime(2020, 03, 01) },
                new User { Id = 2, Login = "tutu74", Password = "tutu", CreationDate = new DateTime(2022, 10, 12) }

                );

            //Sauvegarde des changements 
            this.SaveChanges();

        }
    }
}

