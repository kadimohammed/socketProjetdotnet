using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Projet_Server.Classes
{
    [Serializable]
    public class ModifierInfosResp
    {
        public string Messsage
        {
            get; set;
        }
        public string NewInfos { get; set; }
    }
}
