using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Projet_Server.Models
{

    [Serializable]
    public class Utilisateur
    {
        public int UserID { get; set; }
        public string Telephone { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public byte[] Photo { get; set; } 
        public string Infos { get; set; } 
    }

}
