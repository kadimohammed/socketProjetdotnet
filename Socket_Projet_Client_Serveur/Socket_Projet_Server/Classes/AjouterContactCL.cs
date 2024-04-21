using Socket_Projet_Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Projet_Server.Classes
{
    [Serializable]
    public class AjouterContactCL
    {
        public int UtilisateurId { get; set; }
        public string ContactTelephone { get; set; }
    }
}
