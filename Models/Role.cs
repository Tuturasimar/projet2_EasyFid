using System;
using System.ComponentModel.DataAnnotations;
using Projet2_EasyFid.Data.Enums;

namespace Projet2_EasyFid.Models
{
	public class Role
	{
        public int Id { get; set; }

        [MaxLength(30)]
        public string JobLabel { get; set; }

        public RoleTypeEnum RoleType { get; set; }
    }
}

