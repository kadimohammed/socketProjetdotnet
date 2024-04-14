using System.Data.SqlClient;

namespace Socket_Projet_Server.Repository
{
    public class Connexion
    {
        public static SqlConnection InitConnection()
        {
           string connectionString = "Data Source=DESKTOP-20AGV06\\SQLEXPRESS;Initial Catalog=SocketsProject;Integrated Security=True;TrustServerCertificate=true;";
           return new SqlConnection(connectionString);
        }
    }
}
