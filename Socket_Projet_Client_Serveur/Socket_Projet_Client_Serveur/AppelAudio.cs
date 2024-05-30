using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NAudio.Wave;

namespace Socket_Projet_Client
{
    public partial class AppelAudio : Form
    {
        private WaveInEvent waveIn;
        private UdpClient udpClient;
        private IPEndPoint serverEndPoint;
        private WaveOutEvent waveOut;
        private BufferedWaveProvider bufferedWaveProvider;

        public AppelAudio()
        {
            InitializeComponent();

            // Initialiser le client UDP
            udpClient = new UdpClient();
            serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1252);

            // Initialiser la capture audio
            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(8000, 16, 1)
            };
            waveIn.BufferMilliseconds = 50; // Réduire la taille du tampon de capture à 50 millisecondes
            waveIn.DataAvailable += WaveIn_DataAvailable;

            // Commencer la capture audio
            waveIn.StartRecording();

            // Initialiser la lecture audio
            waveOut = new WaveOutEvent();
            bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1))
            {
                BufferDuration = TimeSpan.FromSeconds(20) // Augmenter la durée du tampon à 20 secondes
            };
            waveOut.Init(bufferedWaveProvider);
            waveOut.Play();

            // Démarrer le thread pour recevoir les données du serveur
            Thread receiveThread = new Thread(ReceiveAudio);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // Envoi des données audio au serveur
            try
            {
                udpClient.Send(e.Buffer, e.BytesRecorded, serverEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une exception s'est produite lors de l'envoi des données audio : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveAudio()
        {
            try
            {
                // Lier le client UDP à un point de terminaison local
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 0));
                byte[] buffer;
                while (true)
                {
                    buffer = udpClient.Receive(ref serverEndPoint);
                    bufferedWaveProvider.AddSamples(buffer, 0, buffer.Length);
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Une exception de socket s'est produite : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une exception s'est produite : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AppelAudio_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Arrêter la capture audio
            waveIn.StopRecording();

            // Fermer le client UDP
            udpClient.Close();
        }
    }
}
