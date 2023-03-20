using System;
namespace Projet2_EasyFid.Models
{
	public interface IDal : IDisposable
	{
        int AddUser(string nom, string password);
        User Authenticate(string nom, string password);
        User GetUser(int id);
        User GetUser(string idStr);

    }
}

