using Newtonsoft.Json;
using Socket_Projet_Server.Classes;
using Socket_Projet_Server.Mappers;
using Socket_Projet_Server.Models;
using Socket_Projet_Server.Repository;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Socket_Projet_Server
{
    public class ThreadClient :  IDisposable
    {
        private Socket clientSocket;
        NetworkStream networkStream;
        BinaryFormatter formatter;
        object receivedObject;


        public ThreadClient(Socket socket)
        {
            lock (typeof(ThreadClient))
            {
                clientSocket = socket;
                networkStream = new NetworkStream(clientSocket);
                formatter = new BinaryFormatter();
            }
        }

        public void Run()
        {
            try
            {
                while (true)
                {

                    receivedObject = formatter.Deserialize(networkStream);
                    switch (receivedObject)
                    {
                        case LoginCl request:
                            Console.WriteLine("Tâche de login :" + request.Telephone);
                            Utilisateur user = UsersRepository.GetUserByTelephoneAndPassword(request.Telephone, request.Password);
                            if (user != null)
                            {
                                try
                                {
                                    LoginCl loginCl = UtilisateurMapper.GetLoginClFromUtilisateur(user);
                                    Console.WriteLine("Client bien Connecter");
                                    formatter.Serialize(networkStream, loginCl);
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
                            else
                            {
                                Console.WriteLine("Client ne pas Connecter");
                                formatter.Serialize(networkStream, new LoginCl { Id = -1 });
                            }
                            break;
                        case AjouterContactCL request:

                            Console.WriteLine("Tâche d'ajouter un Contact :");
                            Utilisateur userContact = UsersRepository.GetUserByTelephone(request.ContactTelephone);
                            Utilisateur newuser = new Utilisateur();
                            newuser.Id = -1;
                            string message_ajout_contact = "";
                            if (userContact != null)
                            {
                                try
                                {
                                    message_ajout_contact = UsersRepository.AddContact(request.UtilisateurId, userContact.Id);
                                }
                                catch (IOException)
                                {
                                    Console.WriteLine("Erreur d'entrée/sortie lors de la communication avec le client.");
                                    message_ajout_contact = "Erreur d'entrée/sortie lors de la communication avec le client.";
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Une erreur s'est produite lors du traitement de la demande du client : " + ex.Message);
                                    message_ajout_contact = "Une erreur s'est produite lors du traitement de la demande du client : ";
                                    break;
                                }

                                newuser = UsersRepository.GetUserById(request.UtilisateurId);
                                

                            }
                            else
                            {
                                message_ajout_contact = "Ce Contact n'exist pas !!!";
                            }
                            formatter.Serialize(networkStream, UtilisateurMapper.GetLoginClFromUtilisateur(newuser));
                            formatter.Serialize(networkStream, message_ajout_contact);
                            break;
                        default:
                            Console.WriteLine("Type de requête non pris en charge.");
                            break;
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
