﻿namespace Chat.DAL.Entities
{
    public class UserGroup
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
