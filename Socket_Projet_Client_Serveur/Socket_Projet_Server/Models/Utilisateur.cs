﻿namespace Socket_Projet_Server.Models
{

    [Serializable]
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Telephone { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public byte[]? Photo { get; set; } 
        public string? Infos { get; set; } 
        public IList<Contact>? Contacts { get; set; } = new List<Contact>();
        public IList<Message> MessagesSent { get; set; }
        public IList<Message> MessagesReceived { get; set; }
    }

}
