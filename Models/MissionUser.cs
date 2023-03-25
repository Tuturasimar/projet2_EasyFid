using System;
namespace Projet2_EasyFid.Models
{
    public class MissionUser
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int MissionId { get; set; }
        public Mission Mission { get; set; }
    }
}

