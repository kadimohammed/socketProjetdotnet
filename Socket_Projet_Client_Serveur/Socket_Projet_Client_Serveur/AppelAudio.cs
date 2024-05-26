using NAudio.Wave;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Socket_Projet_Client
{
    public partial class AppelAudio : Form
    {
        private WaveInEvent waveIn;
        private UdpClient udpClient;
        private IPEndPoint serverEndPoint;

        public AppelAudio()
        {
            InitializeComponent();
            // Initialisez les composants de votre formulaire ici

            // Initialiser le client UDP
            udpClient = new UdpClient();
            serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1252);

            // Initialiser la capture audio
            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(8000, 16, 1); // Format audio (8000 Hz, 16 bits, mono)
            waveIn.DataAvailable += WaveIn_DataAvailable;

            // Commencer la capture audio
            waveIn.StartRecording();
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // Envoi des données audio au serveur à chaque fois que des données sont disponibles
            try
            {
                udpClient.Send(e.Buffer, e.BytesRecorded, serverEndPoint);
                Console.WriteLine("Données audio envoyées au serveur. Taille : " + e.BytesRecorded + " octets.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une exception s'est produite lors de l'envoi des données audio : " + ex.Message);
            }
        }

        private void AppelAudio_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Arrêter la capture audio et fermer le client UDP lorsque le formulaire se ferme
            waveIn.StopRecording();
            udpClient.Close();
        }
    }
}
