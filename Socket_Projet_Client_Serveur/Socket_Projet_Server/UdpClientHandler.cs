using NAudio.Wave;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public class UdpClientHandler
{
    private readonly Socket serverSocket;
    public static ConcurrentDictionary<int, EndPoint> dictClientEndPoints;
    private readonly WaveOutEvent waveOut;
    private readonly BufferedWaveProvider bufferedWaveProvider;
    private const int BufferSize = 65507;
    private readonly TimeSpan MaxBufferDuration = TimeSpan.FromSeconds(100);

    public UdpClientHandler(Socket serverSocket)
    {
        this.serverSocket = serverSocket;
        dictClientEndPoints = new ConcurrentDictionary<int, EndPoint>();
        waveOut = new WaveOutEvent();
        bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1))
        {
            BufferDuration = MaxBufferDuration
        };
        waveOut.Init(bufferedWaveProvider);
        waveOut.Play();
    }

    public async Task HandleClientsAsync()
    {
        try
        {
            byte[] buffer = new byte[BufferSize];
            EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                SocketReceiveFromResult result = await serverSocket.ReceiveFromAsync(new ArraySegment<byte>(buffer), SocketFlags.None, clientEndPoint);
                int bytesRead = result.ReceivedBytes;
                clientEndPoint = result.RemoteEndPoint;

                if (!dictClientEndPoints.Values.Contains(clientEndPoint))
                {
                    int clientId = dictClientEndPoints.Count + 1;
                    dictClientEndPoints[clientId] = clientEndPoint;
                }

                if (bufferedWaveProvider.BufferedDuration < MaxBufferDuration)
                {
                    bufferedWaveProvider.AddSamples(buffer, 0, bytesRead);
                    //Console.WriteLine("Données audio reçues du client. Taille : " + bytesRead + " octets.");
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
                    //Console.WriteLine("Données audio diffusées au client : " + clientEndPoint);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        RemoveClientByEndPoint(clientEndPoint);
                        Console.WriteLine("Client déconnecté : " + clientEndPoint);
                    }
                    else
                    {
                        Console.WriteLine("Erreur lors de l'envoi des données au client : " + ex.Message);
                    }
                }
            }
        }
    }

    private void RemoveClientByEndPoint(EndPoint endPoint)
    {
        foreach (var clientEntry in dictClientEndPoints)
        {
            if (clientEntry.Value.Equals(endPoint))
            {
                dictClientEndPoints.TryRemove(clientEntry.Key, out _);
                break;
            }
        }
    }
}
