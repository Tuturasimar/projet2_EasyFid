using Projet2_EasyFid.Models;
using System.Collections.Generic;
using System.Linq;

namespace Projet2_EasyFid.Data.Services
{
    public class FormationServices
    {
        public static List<Formation> GetAllFormations(BddContext _bddContext)
        {
            return _bddContext.Formations.ToList();
        }
        public static int CreateFormation(BddContext _bddContext, Formation formation)
        {
            _bddContext.Formations.Add(formation);
            _bddContext.SaveChanges();
            return formation.Id;
        }
        public static void UpdateFormation(BddContext _bddContext, Formation formation)
        {
            // Update permet de mettre à jour directement le bon User dans la table (grâce à l'id sans doute)
            _bddContext.Formations.Update(formation);
            _bddContext.SaveChanges();
        }
    }
}
