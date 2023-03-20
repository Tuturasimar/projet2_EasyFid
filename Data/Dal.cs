using System;
using System.Text;
using System.Security.Cryptography;
using System.Linq;

namespace Projet2_EasyFid.Models
{

    public class Dal : IDal

    {
        private BddContext _bddContext;
        public Dal()
        {
            _bddContext = new BddContext();
        }

        public void DeleteCreateDatabase()
        {
            _bddContext.Database.EnsureDeleted();
            _bddContext.Database.EnsureCreated();
        }

        public int AddUser(string login, string password)
        {
            string encryptedPassword = EncodeMD5(password);
            User user = new User() { Login = login, Password = encryptedPassword };
            this._bddContext.Users.Add(user);
            this._bddContext.SaveChanges();
            return user.Id;
        }

        public User Authentifier(string login, string password)
        {
            string encryptedPassword = EncodeMD5(password);
            User user = this._bddContext.Users.FirstOrDefault(u => u.Login == login && u.Password == encryptedPassword);
            return user;
        }

        public User GetUser(int id)
        {
            return this._bddContext.Users.Find(id);
        }

        public User GetUser(string idStr)
        {
            int id;
            if (int.TryParse(idStr, out id))
            {
                return this.GetUser(id);
            }
            return null;
        }

        public static string EncodeMD5(string encryptedPassword)
        {
            string encryptedPasswordSel = "Choix" + encryptedPassword + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(encryptedPasswordSel)));
        }

        public User ObtenirUtilisateur(string idStr)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public User Authenticate(string nom, string password)
        {
            throw new NotImplementedException();
        }
    }
}

