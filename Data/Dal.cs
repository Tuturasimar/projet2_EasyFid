using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projet2_EasyFid.Data
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

		public List<Cra> GetAllCras()
		{
			return _bddContext.Cras.ToList();
		}

		//Methode pour creer un cra et qui nous retourne son Id
		public int CreateCra(int id, DateTime createdAt, StateEnum stateCra, int? userId)
		{
			//On instancie un nouveau Cra
			Cra cra = new Cra() { CreatedAt = createdAt, StateCra = stateCra, UserId = userId };
			//On l'ajoute à la liste des cras
			_bddContext.Cras.Add(cra);
			//On sauvegarde les changements 
			_bddContext.SaveChanges();
			return cra.Id;
		}

		//Methode pour modifier un cra
		public void UpdateCra(int id, StateEnum stateCra) {
			Cra cra = _bddContext.Cras.Find(id);
			if (cra != null)
			{
				cra.Id = id;
				cra.StateCra = stateCra;
				_bddContext.SaveChanges();
			}
		}

		public void Dispose()
		{
			_bddContext.Dispose();
		}
	}
}

