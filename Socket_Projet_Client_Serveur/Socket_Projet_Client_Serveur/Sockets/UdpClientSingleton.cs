using System.Net;
using System.Net.Sockets;

namespace Socket_Projet_Client.Sockets
{
    public class UdpClientSingleton
    {
        private static readonly object lockObject = new object();

        private static UdpClient udpClient = null;
        private static IPEndPoint EndPoint = null;

        private UdpClientSingleton() { }

        public static UdpClient GetInstance()
        {
            if (udpClient == null)
            {
                lock (lockObject)
                {
                    if (udpClient == null)
                    {
                        udpClient = new UdpClient();
                        return udpClient;
                    }
                }
            }
            return udpClient;
        }

        public static IPEndPoint GetInstanceEndPoint()
        {
            if (EndPoint == null)
            {
                lock (lockObject)
                {
                    if (EndPoint == null)
                    {
                        EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1252);
                        udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 0));
                        return EndPoint;
                    }
                }
            }
            return EndPoint;
        }

    }
}
