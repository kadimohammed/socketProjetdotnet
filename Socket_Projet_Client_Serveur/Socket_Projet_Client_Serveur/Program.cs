using SocketsProject;

namespace Socket_Projet_Client_Serveur
{
    internal static class Program
    {
        public static Login login = new Login();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(login);
        }
    }
}