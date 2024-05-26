using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Projet_Client.Sockets
{
    internal class SocketSingleton
    {
        private static readonly object lockObject = new object();

        public static Socket clientSocket;

        private SocketSingleton() { }

        public static Socket GetInstance()
        {
            if (clientSocket == null)
            {
                lock (lockObject)
                {
                    if (clientSocket == null)
                    {
                        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        return clientSocket;
                    }
                }
            }
            return clientSocket;
        }

        public static void Disconnect(Socket cs)
        {
            if (cs != null && cs.Connected)
            {
                cs.Shutdown(SocketShutdown.Both);
                cs.Close();
            }
        }

        public static void Connect(Socket cs)
        {
            if (!cs.Connected)
            {
                cs.Connect("localhost", 1234);
            }
        }
    }
}
