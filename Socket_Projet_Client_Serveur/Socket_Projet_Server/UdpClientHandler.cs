using NAudio.Wave;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public class UdpClientHandler
{
    private Socket serverSocket;
    public static  ConcurrentDictionary<int, EndPoint> dictClientEndPoints;
    private readonly WaveOutEvent waveOut;
    private readonly BufferedWaveProvider bufferedWaveProvider;

    public UdpClientHandler(Socket serverSocket)
    {
        this.serverSocket = serverSocket;
        dictClientEndPoints = new ConcurrentDictionary<int, EndPoint>();
        waveOut = new WaveOutEvent();
        bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1))
        {
            BufferDuration = TimeSpan.FromSeconds(20) // Augmenter la durée du tampon à 20 secondes
        };
        waveOut.Init(bufferedWaveProvider);
        waveOut.Play(); // Commencer la lecture audio
    }

    public async Task HandleClientsAsync()
    {
        try
        {
            byte[] buffer = new byte[65507];
            EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            int bytesRead;
            SocketReceiveFromResult result;
            ArraySegment<byte> aaaa = new ArraySegment<byte>(buffer);
            while (true)
            {
                result = await serverSocket.ReceiveFromAsync(aaaa, SocketFlags.None, clientEndPoint);
                bytesRead = result.ReceivedBytes;
                clientEndPoint = result.RemoteEndPoint;

                // Vérifiez si l'EndPoint du client existe déjà dans le dictionnaire
                if (!dictClientEndPoints.Values.Contains(clientEndPoint))
                {
                    int clientId = dictClientEndPoints.Count + 1;
                    dictClientEndPoints[clientId] = clientEndPoint;
                }

                // Ajouter des échantillons au tampon si ce n'est pas plein
                if (bufferedWaveProvider.BufferedDuration < TimeSpan.FromSeconds(20))
                {
                    bufferedWaveProvider.AddSamples(buffer, 0, bytesRead);
                    Console.WriteLine("Données audio reçues du client. Taille : " + bytesRead + " octets.");
                }
                else
                {
                    Console.WriteLine("Le tampon est plein. Les données audio n'ont pas été ajoutées.");
                }
                
                await BroadcastAudioAsync(buffer, bytesRead, clientEndPoint);
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

    private async Task BroadcastAudioAsync(byte[] buffer, int bytesRead, EndPoint senderEndPoint)
    {
        foreach (var clientEndPoint in dictClientEndPoints.Values)
        {
            if (!clientEndPoint.Equals(senderEndPoint))
            {
                try
                {
                    await serverSocket.SendToAsync(new ArraySegment<byte>(buffer, 0, bytesRead), SocketFlags.None, clientEndPoint);
                    Console.WriteLine("Données audio diffusées au client : " + clientEndPoint);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Erreur lors de l'envoi des données au client : " + ex.Message);
                }
            }
        }
    }
}
