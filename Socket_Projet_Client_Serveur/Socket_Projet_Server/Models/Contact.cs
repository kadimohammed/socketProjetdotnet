using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Projet_Server.Models
{
    [Serializable]
    public class Contact
    {
        [Key, Column(Order = 0)]
        public int UtilisateurId { get; set; }
        [ForeignKey("UtilisateurId")]
        public Utilisateur Utilisateur { get; set; }

        [Key, Column(Order = 1)]
        public int ContactUserId { get; set; }
        [ForeignKey("ContactUserId")]
        public Utilisateur ContactUser { get; set; }
    }












}
