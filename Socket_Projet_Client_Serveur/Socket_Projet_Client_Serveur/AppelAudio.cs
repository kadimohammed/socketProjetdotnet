using NAudio.Wave;
using Socket_Projet_Client.Sockets;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Socket_Projet_Client
{
    public partial class AppelAudio : Form
    {
        private WaveInEvent waveIn;
        private UdpClient udpClient;
        private IPEndPoint serverEndPoint;
        private WaveOutEvent waveOut;
        private BufferedWaveProvider bufferedWaveProvider;
        Thread receiveThread;
        private bool isRecording = false;
        private bool connected = true;

        public AppelAudio()
        {
            InitializeComponent();


            StartLoadingIndicator();

            // Initialiser le client UDP
            udpClient = UdpClientSingleton.GetInstance();
            serverEndPoint = UdpClientSingleton.GetInstanceEndPoint();

            // Envoyer les informations de contact au serveur
            //string appel_client = "AppelClient" + Login.user.Id + "-" + 7;//Form1.contact_selected.Id;
            //byte[] data = Encoding.UTF8.GetBytes(appel_client);
            //udpClient.Send(data, data.Length, serverEndPoint);


            // Initialiser la capture audio
            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(8000, 16, 1)
            };
            waveIn.BufferMilliseconds = 20; // Réduire la taille du tampon de capture à 50 millisecondes
            waveIn.DataAvailable += WaveIn_DataAvailable;

            waveIn.StartRecording();

            // Initialiser la lecture audio
            waveOut = new WaveOutEvent();
            bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1))
            {
                BufferDuration = TimeSpan.FromSeconds(20) // Augmenter la durée du tampon à 30 secondes
            };
            waveOut.Init(bufferedWaveProvider);
            waveOut.Play();

            receiveThread = new Thread(ReceiveAudio);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        private void SendContactInfo(string contact)
        {
            try
            {
                byte[] message = Encoding.UTF8.GetBytes(contact);
                udpClient.Send(message, message.Length, serverEndPoint);
                MessageBox.Show("Contact information sent to the server.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An exception occurred while sending contact information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                udpClient.Send(e.Buffer, e.BytesRecorded, serverEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An exception occurred while sending audio data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveAudio()
        {
            try
            {
                byte[] buffer;
                while (connected)
                {
                    buffer = udpClient.Receive(ref serverEndPoint);
                    bufferedWaveProvider.AddSamples(buffer, 0, buffer.Length);

                    if (bufferedWaveProvider.BufferedDuration > TimeSpan.FromSeconds(25))
                    {
                        bufferedWaveProvider.ClearBuffer();
                        MessageBox.Show("Buffer overflow detected, clearing buffer.", "Buffer Overflow", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show("A socket exception occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An exception occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            waveIn.StopRecording();
            connected = false;
            this.Close();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            waveIn.StopRecording();
            connected = false;
            this.Close();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Microphone();
        }

        private void Microphone()
        {
            if (isRecording)
            {
                waveIn.StartRecording();
                isRecording = false;
                guna2CircleButton1.Image = Properties.Resources.microphone_enregistreur;
            }
            else
            {
                waveIn.StopRecording();
                isRecording = true;
                guna2CircleButton1.Image = Properties.Resources.microphonecoper;
            }
        }




        /// /////////////////////////////////////////////////////////////////////////////////////

        private CancellationTokenSource _cancellationTokenSource;

        private async void StartLoadingIndicator()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            string baseText = "Appel en cours ";
            string[] dots = { "", ".", "..", "..." };
            int dotIndex = 0;

            while (!token.IsCancellationRequested)
            {
                string textToShow = baseText + dots[dotIndex];
                dotIndex = (dotIndex + 1) % dots.Length;

                // Invoke to update the UI thread safely
                if (label1.InvokeRequired)
                {
                    label1.Invoke(new Action(() => label1.Text = textToShow));
                }
                else
                {
                    label1.Text = textToShow;
                }

                await Task.Delay(500); // Wait for 500 milliseconds
            }
        }

        private void StopLoadingIndicator()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                label1.Text = "Loading"; // Reset to initial state
            }
        }



    }
}
