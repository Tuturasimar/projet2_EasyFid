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
        // Vérifier que le jeton n'est pas vide ou null
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Le jeton ne peut pas être vide ou null.");
        }

        using (BddContext _bddContext = new BddContext())
        {
            // Récupere l'utilisateur avec le jeton de réinitialisation de mot de passe spécifié
            User user = _bddContext.Users.SingleOrDefault(u => u.PasswordResetToken == token);

            if (user == null)
            {
                // Gére le cas où le jeton est invalide ou a expiré
                return BadRequest("Jeton invalide ou expiré.");
            }

            // Vérifie si le jeton a expiré
            if (DateTime.UtcNow > user.PasswordResetTokenExpiration)
            {
                return BadRequest("Le jeton a expiré.");
            }

            // Renvoyer une vue qui permet à l'utilisateur de réinitialiser son mot de passe
            return View(new PasswordResetViewModel { Token = token });
        }
    }

    // Action pour traiter la demande de réinitialisation de mot de passe
    [HttpPost]
    public IActionResult Post(PasswordResetViewModel model)
    {
        // Vérifie si le modèle est valide
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        using (BddContext _bddContext = new BddContext())
        {
            // Récupere l'utilisateur avec le jeton de réinitialisation de mot de passe spécifié
            User user = _bddContext.Users.SingleOrDefault(u => u.PasswordResetToken == model.Token);

            if (user == null)
            {
                return BadRequest("Jeton invalide ou expiré.");
            }

            // Vérifier si le jeton a expiré
            if (DateTime.UtcNow > user.PasswordResetTokenExpiration)
            {
                return BadRequest("Le jeton a expiré.");
            }

            // Mets a jour: mot de passe et jeton de réinitialisation de mot de passe
            user.Password = Dal.EncodeMD5(model.Password);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiration = null;
            _bddContext.SaveChanges();

            // Rediriger vers une page qui indique que le mot de passe a été réinitialisé
            return RedirectToAction("PasswordResetConfirmation");
        }
    }

    // Action pour afficher la vue de confirmation de réinitialisation de mot de passe
    [HttpGet]
    [Route("PasswordResetConfirmation")]
    public IActionResult PasswordResetConfirmation()
    {
        return View();
    }
}
