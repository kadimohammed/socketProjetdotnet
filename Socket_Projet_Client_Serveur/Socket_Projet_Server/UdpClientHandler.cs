using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NAudio.Wave;

namespace Socket_Projet_Server
{
    public class UdpClientHandler
    {
        private Socket serverSocket;
        private Dictionary<int, EndPoint> dict_clientEndPoint;
        private readonly WaveOutEvent waveOut;
        private readonly BufferedWaveProvider bufferedWaveProvider;

        public UdpClientHandler(Socket serverSocket)
        {
            this.serverSocket = serverSocket;
            dict_clientEndPoint = new Dictionary<int, EndPoint>();
            waveOut = new WaveOutEvent();
            bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1));
            waveOut.Init(bufferedWaveProvider);
            waveOut.Play(); // Commencer la lecture audio
        }

        public void HandleClients()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[65507]; // Taille maximale pour un datagramme UDP
                    EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0); // Point de terminaison du client
                    int bytesRead = serverSocket.ReceiveFrom(buffer, ref clientEndPoint);

                    // Ajoute les données audio au BufferedWaveProvider
                    bufferedWaveProvider.AddSamples(buffer, 0, bytesRead);
                    Console.WriteLine("Données audio reçues du client. Taille : " + bytesRead + " octets.");
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Une exception de socket s'est produite : " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une exception s'est produite : " + ex.Message);
            }
        }
    }
}
