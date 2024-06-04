using NAudio.Wave;
using Socket_Projet_Client.Sockets;
using System.Net;
using System.Net.Sockets;

namespace Socket_Projet_Client.Outiles
{
    public class AudioHandlerClient
    {
        private UdpClient udpClient;
        private IPEndPoint serverEndPoint;
        private BufferedWaveProvider bufferedWaveProvider;
        private AppelAudio appelAudioForm;
        private bool enappel = false;

        public AudioHandlerClient()
        {
            udpClient = UdpClientSingleton.GetInstance();
            serverEndPoint = UdpClientSingleton.GetInstanceEndPoint();
            bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1));
            // Démarrer le thread de réception audio
            Task.Run(() => ReceiveAudio());
        }

        private void ReceiveAudio()
        {
            try
            {
                while (!enappel)
                {
                    byte[] buffer = udpClient.Receive(ref serverEndPoint);
                    bufferedWaveProvider.AddSamples(buffer, 0, buffer.Length);

                    // Vérifier si des données audio ont été reçues
                    if (buffer.Length > 0)
                    {
                        appelAudioForm = new AppelAudio();
                        appelAudioForm.Show();
                        
                        enappel = true;
                    }
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
    }
}
