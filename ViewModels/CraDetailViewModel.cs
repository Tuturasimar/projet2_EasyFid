using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Projet2_EasyFid.Models;

namespace Projet2_EasyFid.ViewModels
{
	public class CraDetailViewModel
	{
		[Display(Name = "Activité")]
		public Activity Activity { get; set; }
		public List<ActivityDate> ActivityDates {get;set;}
	}
}

