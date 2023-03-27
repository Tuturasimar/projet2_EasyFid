using Projet2_EasyFid.Models;
using System;
using System.Collections.Generic;

using System.Linq;

namespace Projet2_EasyFid.Data.Services
{
	public class StatisticsServices
	{
        public static List<Statistic> GetAllStatistics(BddContext _bddContext)
        {
            return _bddContext.Statistics.ToList();
        }
    }
}

