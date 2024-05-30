using NAudio.Wave;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class AudioHandler
{
    private Socket serverSocket;
    private BufferedWaveProvider bufferedWaveProvider;
    private EndPoint clientEndPoint;
    byte[] buffer;

    SocketReceiveFromResult result;

    public AudioHandler(Socket serverSocket, BufferedWaveProvider bufferedWaveProvider, EndPoint clientEndPoint)
    {
        this.serverSocket = serverSocket;
        this.bufferedWaveProvider = bufferedWaveProvider;
        this.clientEndPoint = clientEndPoint;
    }

    public async Task StartHandling()
    {
        await Task.Run(async () =>
        {
            while (true)
            {
                buffer = new byte[65507];
                var result = await serverSocket.ReceiveFromAsync(new ArraySegment<byte>(buffer), SocketFlags.None, clientEndPoint);
                int bytesRead = result.ReceivedBytes;
                bufferedWaveProvider.AddSamples(buffer, 0, bytesRead);
                _ = BroadcastAudioAsync(buffer, bytesRead, clientEndPoint);
            }
        });
    }

    private async Task BroadcastAudioAsync(byte[] buffer, int bytesRead, EndPoint senderEndPoint)
    {
        foreach (var clientEndPoint in UdpClientHandler.dictClientEndPoints.Values)
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
