﻿
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Socket_Projet_Server.Models;

namespace Socket_Projet_Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
