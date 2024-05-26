using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NAudio.Wave;

namespace Socket_Projet_Server
{
    internal class UdpServer : IDisposable
    {
        private readonly UdpClient udpClient;
        private readonly List<IPEndPoint> clients;
        private readonly WaveOutEvent waveOut;
        private readonly BufferedWaveProvider bufferedWaveProvider;
        private Thread listenThread;
        private bool disposed = false;

        // Déclaration de clientEndPoint
        private IPEndPoint clientEndPoint;

        public UdpServer(int port)
        {
            udpClient = new UdpClient(port);
            clients = new List<IPEndPoint>();

            waveOut = new WaveOutEvent();
            bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1));
            waveOut.Init(bufferedWaveProvider);
            waveOut.Play();

            listenThread = new Thread(Listen);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void Listen()
        {
            try
            {
                while (true)
                {
                    // Initialisation de clientEndPoint à chaque réception
                    clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receivedData = udpClient.Receive(ref clientEndPoint);

                    if (!clients.Contains(clientEndPoint))
                    {
                        clients.Add(clientEndPoint);
                        Console.WriteLine("Nouveau client connecté : " + clientEndPoint);
                    }

                    // Lecture des données audio
                    bufferedWaveProvider.AddSamples(receivedData, 0, receivedData.Length);
                    Console.WriteLine("Données audio reçues du client. Taille : " + receivedData.Length + " octets.");

                    // Répartir les données audio aux autres clients
                    foreach (var client in clients)
                    {
                        if (!client.Equals(clientEndPoint))
                        {
                            udpClient.Send(receivedData, receivedData.Length, client);
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Erreur Socket: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur: " + ex.Message);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    waveOut?.Dispose();
                    udpClient?.Close();
                }

                disposed = true;
            }
        }
    }
}
