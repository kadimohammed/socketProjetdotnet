using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Projet_Server.Classes
{
    [Serializable]
    public class DeconnexionCL
    {
        public int IdUser { get; set; }
        public bool etat { get; set; }
    }
}
