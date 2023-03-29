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
        // Verify that the email exists in the database
        bool emailExists = CheckEmailExists(email);

        if (emailExists)
        {
            // Generate a unique password reset token
            string token = GeneratePasswordResetToken();

            // Store the token in the database
            StorePasswordResetToken(email, token);

            // Send the email with a link to reset the password
            SendEmail(email, token);
        }
        else
        {
            // Handle the case where the email doesn't exist in the database
        }
    }

    public static User GetUserByEmail(BddContext _bddContext, string email)
    {
        return _bddContext.Users.SingleOrDefault(u => u.UserData.Email == email);
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
        // Generate a new GUID
        Guid guid = Guid.NewGuid();

        // Convert the GUID to a string and return it
        return guid.ToString();
    }

    private void StorePasswordResetToken(string email, string token)
    {
        using (BddContext _bddContext = new BddContext())
        {
            User user = ForgotPasswordServices.GetUserByEmail(_bddContext, email);
            if (user != null)
            {
                // Update the user's password reset token
                user.PasswordResetToken = token;
                user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(3);
                _bddContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Invalid email address.");
            }
        }
    }

    private void SendEmail(string email, string token)
    {
        var fromAddress = new MailAddress("test.easyfid@gmail.com");
        var fromPassword = "xhrouikhiofiwmao";
        var toAddress = new MailAddress(email);
        string subject = "Reset Password";
        string body = $"Please click the following link to reset your password: https://example.com/reset_password?token={token}";

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

