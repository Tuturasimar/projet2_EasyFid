using Projet2_EasyFid.Data.Enums;
using Projet2_EasyFid.Models;
using System;
using System.Collections.Generic;

namespace Projet2_EasyFid.Data
{
	public interface IDal : IDisposable
	{
		void DeleteCreateDatabase();
		List<Cra> GetAllCras();

		int CreateCra (int id, DateTime createdAt, StateEnum stateCra, int? userId);

		void UpdateCra(int id, StateEnum stateCra);

	}
}

