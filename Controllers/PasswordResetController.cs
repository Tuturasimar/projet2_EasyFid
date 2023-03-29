using Microsoft.AspNetCore.Mvc;
using Projet2_EasyFid.Data;
using Projet2_EasyFid.Models;
using System;
using System.Linq;

public class PasswordResetController : Controller
{
    public IActionResult Reset(string token)
    {
        // Verify that the token exists in the database and has not expired
        User user = GetUserByToken(token);
        if (user == null)
        {
            // Handle the case where the token is invalid or has expired
            return RedirectToAction("Index", "Home");
        }
        else
        {
            // Display a form for the user to enter their new password
            PasswordResetViewModel model = new PasswordResetViewModel
            {
                Token = token
            };
            return View(model);
        }
    }

    [HttpPost]
    public IActionResult Reset(PasswordResetViewModel model)
    {
        // Verify that the token exists in the database and has not expired
        User user = GetUserByToken(model.Token);
        if (user == null)
        {
            // Handle the case where the token is invalid or has expired
            return RedirectToAction("Index", "Home");
        }
        else
        {
            // Update the user's password and clear the password reset token
            using (Dal dal = new Dal())
            {
                user.Password = Dal.EncodeMD5(model.Password);
                user.PasswordResetToken = null;
                user.PasswordResetTokenExpiration = null;
                dal.ModifyUser(user);
            }
            return RedirectToAction("Index", "Home");
        }
    }

    private User GetUserByToken(string token)
    {
        using (BddContext _bddContext = new BddContext())
        {
            User user = _bddContext.Users.SingleOrDefault(u => u.PasswordResetToken == token && u.PasswordResetTokenExpiration > DateTime.UtcNow);
            return user;
        }
    }
}
