using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Projet_Server.Classes
{
    [Serializable]
    public class ModifierInfosCl
    {
        public int UtilisateurId { get; set; }
        public string NewInfos
        {
            get; set;
        }
    }
}
