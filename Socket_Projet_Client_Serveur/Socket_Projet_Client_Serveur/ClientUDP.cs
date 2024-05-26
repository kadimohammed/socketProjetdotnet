using NAudio.Wave;
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Socket_Projet_Client
{
    internal class ClientUDP : IDisposable
    {
        private readonly UdpClient udpClient;
        private readonly IPEndPoint serverEndPoint;
        private readonly WaveInEvent waveIn;

        public ClientUDP(string serverIp, int serverPort)
        {
            udpClient = new UdpClient();
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

            waveIn = new WaveInEvent();
            waveIn.BufferMilliseconds = 50;
            waveIn.WaveFormat = new WaveFormat(8000, 16, 1);

            waveIn.DataAvailable += (sender, e) =>
            {
                byte[] buffer = new byte[e.BytesRecorded];
                Buffer.BlockCopy(e.Buffer, 0, buffer, 0, e.BytesRecorded);
                try
                {
                    udpClient.Send(buffer, buffer.Length, serverEndPoint);
                    Console.WriteLine("Données audio envoyées. Taille : " + buffer.Length + " octets.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur d'envoi : " + ex.Message);
                }
            };

            waveIn.StartRecording();
        }

        public void Dispose()
        {
            waveIn?.StopRecording();
            udpClient?.Close();
        }
    }
}
