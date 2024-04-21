using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Projet_Server.Models
{
    [Serializable]
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime SendDate { get; set; } = DateTime.Now;
        public Utilisateur Sender { get; set; }
        public Utilisateur Receiver { get; set; }
        public byte[]? Image { get; set; } 
        public byte[]? Audio { get; set; }
        public byte[]? Video { get; set; }
    }
}
