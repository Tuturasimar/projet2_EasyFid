using Microsoft.AspNetCore.Mvc;
using Projet2_EasyFid.Data;
using Projet2_EasyFid.Models;
using System;
using System.Linq;

[Route("[controller]")]
public class PasswordResetController : Controller
{
    [HttpGet]
    public IActionResult Get(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Token cannot be null or empty.");
        }

        using (BddContext _bddContext = new BddContext())
        {
            // Get the user with the specified password reset token
            User user = _bddContext.Users.SingleOrDefault(u => u.PasswordResetToken == token);

            if (user == null)
            {
                // Handle the case where the token is invalid or has expired
                return BadRequest("Invalid or expired token.");
            }

            // Check if the token has expired
            if (DateTime.UtcNow > user.PasswordResetTokenExpiration)
            {
                // Handle the case where the token has expired
                return BadRequest("Token has expired.");
            }

            // Return a view that allows the user to reset their password
            return View(new PasswordResetViewModel { Token = token });
        }
    }

    [HttpPost]
    public IActionResult Post(PasswordResetViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Display validation errors to the user
            return View(model);
        }

        using (BddContext _bddContext = new BddContext())
        {
            // Get the user with the specified password reset token
            User user = _bddContext.Users.SingleOrDefault(u => u.PasswordResetToken == model.Token);

            if (user == null)
            {
                // Handle the case where the token is invalid or has expired
                return BadRequest("Invalid or expired token.");
            }

            // Check if the token has expired
            if (DateTime.UtcNow > user.PasswordResetTokenExpiration)
            {
                // Handle the case where the token has expired
                return BadRequest("Token has expired.");
            }

            // Update the user's password and password reset token
            user.Password = Dal.EncodeMD5(model.Password);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiration = null;
            _bddContext.SaveChanges();

            // Redirect to a page that indicates the password has been reset
            return RedirectToAction("PasswordResetConfirmation");
        }
    }


    [HttpGet]
    [Route("PasswordResetConfirmation")]
    public IActionResult PasswordResetConfirmation()
    {
        return View();
    }
}