using Projet2_EasyFid.Data;
using Projet2_EasyFid.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

public class ForgotPasswordServices
{
    public void SendPasswordResetEmail(string email)
    {
        // Vérifie que l'email existe dans la base de données
        bool emailExists = CheckEmailExists(email);

        if (emailExists)
        {
            // Générer un jeton de réinitialisation de mot de passe unique
            string token = GeneratePasswordResetToken();

            // Stocker le jeton dans la base de données
            StorePasswordResetToken(email, token);

            // Envoyer l'email avec un lien pour réinitialiser le mot de passe
            SendEmail(email, token);
        }
        else
        {
            // Gérer le cas où l'email n'existe pas dans la base de données
        }
    }

    public static User GetUserByEmail(BddContext _bddContext, string email)
    {
        return _bddContext.Users.SingleOrDefault(u => u.UserData.Email == email);
    }

    public static User GetUserByResetToken(string token)
    {
        using (BddContext _bddContext = new BddContext())
        {
            return _bddContext.Users.SingleOrDefault(u => u.PasswordResetToken == token && u.PasswordResetTokenExpiration > DateTime.UtcNow);
        }
    }

    private bool CheckEmailExists(string email)
    {
        using (BddContext _bddContext = new BddContext())
        {
            User user = ForgotPasswordServices.GetUserByEmail(_bddContext, email);
            return user != null;
        }
    }

    private string GeneratePasswordResetToken()
    {
        // Générer un nouveau GUID
        Guid guid = Guid.NewGuid();

        // Convertir le GUID en chaîne et le renvoyer
        return guid.ToString();
    }

    private void StorePasswordResetToken(string email, string token)
    {
        using (BddContext _bddContext = new BddContext())
        {
            User user = ForgotPasswordServices.GetUserByEmail(_bddContext, email);
            if (user != null)
            {
                // Mettre à jour le jeton de réinitialisation de mot de passe de l'utilisateur
                user.PasswordResetToken = token;
                user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(3);
                _bddContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Adresse e-mail invalide.");
            }
        }
    }

    private void SendEmail(string email, string token)
    {
        var fromAddress = new MailAddress("testing.easyfid@gmail.com");
        var fromPassword = "";
        var toAddress = new MailAddress(email);
        string subject = "Réinitialisation du mot de passe";
        string body = $"Veuillez cliquer sur le lien suivant pour réinitialiser votre mot de passe : https://localhost:5001/PasswordReset?token={token}";

        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };
        using (var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body
        })
        {
            smtp.Send(message);
        }
    }

}

