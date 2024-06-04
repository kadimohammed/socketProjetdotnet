using Socket_Projet_Server.Models;

namespace Socket_Projet_Server.Mappers
{
    public class UtilisateurMapper
    {
        public static LoginCl GetLoginClFromUtilisateur(Utilisateur u)
        {
            LoginCl loginCl = new LoginCl();

            loginCl.Id = u.Id;
            loginCl.Telephone = u.Telephone;
            loginCl.FullName = u.FullName;
            loginCl.Password = u.Password;
            loginCl.Photo = u.Photo;
            loginCl.Infos = u.Infos;
            loginCl.Contacts = u.Contacts;
            loginCl.MessagesSent = u.MessagesSent;
            loginCl.MessagesReceived = u.MessagesReceived;

            return loginCl;
        }
    }
}
