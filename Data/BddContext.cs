using System;
using static Projet2_EasyFid.Models.BddContext;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Projet2_EasyFid.Models
{

    public class BddContext : DbContext

    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<Company> Companies { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user id=root;password=rrrrrrrr;database=UserData");
        }

        public void InitializeDb()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();

            this.Users.AddRange(
            new User { Id = 1, Login = "seb@bemytech.com", Password = Dal.EncodeMD5("seb1"), UserDataId = 1, CompanyId = 1 },
            new User { Id = 2, Login = "laura@bemytech.com", Password = Dal.EncodeMD5("laura1"), UserDataId = 2, CompanyId = 1 }
            );
            this.UserDatas.AddRange(
            new UserData { Id = 1, Lastname = "MONTIEL", Firstname = "Sebastien", Birthday = new DateTime(1995, 3, 17) },
            new UserData { Id = 2, Lastname = "LAGRANGE", Firstname = "Laura", Birthday = new DateTime(1990, 6, 10) }
            );

            this.Companies.AddRange(
            new Company { Id = 1, Name = "BeMyTech", Adress = "Paris 75015", TotalCA = 123000 }
            );

            this.SaveChanges();
        }

    }

}


