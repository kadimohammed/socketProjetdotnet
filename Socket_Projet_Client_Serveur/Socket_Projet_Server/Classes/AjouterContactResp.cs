using Socket_Projet_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Projet_Server.Classes
{
    [Serializable]
    public class AjouterContactResp
    {
        public string Messsage
        {
            get; set;
        }

        public LoginCl loginCl { get; set; }
    }
}
