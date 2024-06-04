using Socket_Projet_Server.Models;

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
