
using System.Net;
using System.Net.Sockets;

namespace Socket_Projet_Server
{
    public class Program
    {
        //public static List<Socket> List_Sockets_Client = new List<Socket>();
        public static Dictionary<int,Socket> Dict_Sockets_Client = new Dictionary<int, Socket>();
        public static void Main(string[] args)
        {
            /*
            int serverPort = 1222;

            // Démarrer le serveur dans un thread séparé
            Thread serverThread = new Thread(() =>
            {
                using (var server = new UdpServer(serverPort))
                {
                    Console.WriteLine("Serveur démarré sur le port " + serverPort + ". En attente de connexions...");
                    Console.ReadLine();
                }
            });
            serverThread.Start();*/


            // Utilisation
            Socket serverSocketUdp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint endPointudp = new IPEndPoint(IPAddress.Any, 1252);

            // Liaison du socket à l'adresse et au port spécifiés
            serverSocketUdp.Bind(endPointudp);
            Console.WriteLine("Serveur UDP démarré...");

            // Création de l'instance du gestionnaire de clients UDP
            UdpClientHandler clientHandler = new UdpClientHandler(serverSocketUdp);

            // Démarrage d'un thread pour gérer les clients de manière asynchrone
            Task.Run(() => clientHandler.HandleClientsAsync());







            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 1234);
            serverSocket.Bind(endPoint);
            serverSocket.Listen();
            Console.WriteLine("Serveur TCP démarré...");

            try
            {
                while (true)
                {
                    Socket clientSocket = serverSocket.Accept();
                    Console.WriteLine("nouveux Client Connecter");
                    //List_Sockets_Client.Add(clientSocket);
                    ThreadClient th = new ThreadClient(clientSocket);
                    Thread t = new Thread(new ThreadStart(th.Run));
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une exception s'est produite : " + ex.Message);
            }
            finally
            {
                serverSocket.Close();
            }

        }
    }
}
