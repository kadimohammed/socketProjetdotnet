
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Socket_Projet_Server.Models;
using Microsoft.VisualBasic;
using System.Text;

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
            serverSocketUdp.Bind(endPointudp);
            Console.WriteLine("Serveur Udp démarré...");

            UdpClientHandler clientHandler = new UdpClientHandler(serverSocketUdp);
            Thread serverThread = new Thread(clientHandler.HandleClients);
            serverThread.Start();





            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 1234);
            serverSocket.Bind(endPoint);
            serverSocket.Listen();
            Console.WriteLine("Serveur démarré...");

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
