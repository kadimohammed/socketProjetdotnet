using Newtonsoft.Json;
using Socket_Projet_Server.Models;
using Socket_Projet_Server.Repository;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Socket_Projet_Server
{
    public class MonThread :  IDisposable
    {
        private Socket clientSocket;

        public MonThread(Socket socket)
        {
            lock (typeof(MonThread))
            {
                clientSocket = socket;
            }
        }

        public void Run()
        {
            try
            {
                using (NetworkStream networkStream = new NetworkStream(clientSocket))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    while (true)
                    {
                        try
                        {
                            // Réception de l'utilisateur depuis le client
                            Utilisateur receivedUser = (Utilisateur)formatter.Deserialize(networkStream);
                            string telephone = receivedUser.Telephone;
                            string password = receivedUser.Password;

                            // Récupération de l'utilisateur depuis la base de données
                            Utilisateur user = UsersRepository.GetUserByTelephoneAndPassword(telephone, password);

                            // Envoi de l'utilisateur au client
                            if (user != null)
                            {
                                Console.WriteLine("Client bien Connecter");
                                formatter.Serialize(networkStream, user);
                            }
                            else
                            {
                                Console.WriteLine("Client ne pas Connecter");
                                formatter.Serialize(networkStream, new Utilisateur { Id = -1 });
                            }

                        }
                        catch (IOException)
                        {
                            Console.WriteLine("Erreur d'entrée/sortie lors de la communication avec le client.");
                            break; 
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Une erreur s'est produite lors du traitement de la demande du client : " + ex.Message);
                            break;
                        }
                    }
                }

            }
            catch (SocketException)
            {
                Console.WriteLine("La connexion avec le client a été réinitialisée ou fermée.");
            }
            catch (IOException ex)
            {
                Console.WriteLine("Une erreur d'entrée/sortie s'est produite: " + ex.Message);
            }
            finally
            {              
                Dispose();
                Console.WriteLine("Client DeConnecter");
            }
        }

        public void Dispose()
        {
            try
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (SocketException) { }
        }
    }
}
