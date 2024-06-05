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
            bool liremessage = true;
            try
            {
                while (liremessage)
                {
                    receivedObject = formatter.Deserialize(networkStream);
                    switch (receivedObject)
                    {
                        case LoginCl request:
                            Console.WriteLine("login :" + request.Telephone+ " Password :" + request.Password);
                            Utilisateur user = UsersRepository.GetUserByTelephoneAndPassword(request.Telephone, request.Password);
                            if (user != null)
                            {
                                try
                                {
                                    LoginCl loginCl = UtilisateurMapper.GetLoginClFromUtilisateur(user);
                                    Console.WriteLine("Client id :"+ loginCl.Id+" -> "+ loginCl .FullName+ " bien Connecter");
                                    Program.Dict_Sockets_Client.Add(user.Id, clientSocket);
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

                            Console.WriteLine("Utilisateur : " + request.UtilisateurId + " : Ajouter le contact : " + request.ContactTelephone);
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
                            AjouterContactResp ajouterContactResp = new AjouterContactResp();
                            ajouterContactResp.Messsage = message_ajout_contact;
                            ajouterContactResp.loginCl = UtilisateurMapper.GetLoginClFromUtilisateur(newuser);
                            formatter.Serialize(networkStream, ajouterContactResp);
                            break;
                        case ModifierNameCl request:
                            Console.WriteLine("Mise à jour du nom complet de l'utilisateur : " + request.UtilisateurId + " nouveau nom : " + request.NewName);
                            string message_modifier_fullname = "";
                            if (request != null)
                            {
                                try
                                {
                                    message_modifier_fullname = UsersRepository.EditNameUtilisateur(request.UtilisateurId, request.NewName);
                                }
                                catch (IOException)
                                {
                                    Console.WriteLine("Erreur d'entrée/sortie lors de la communication avec le client.");
                                    message_modifier_fullname = "Erreur d'entrée/sortie lors de la communication avec le client.";
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Une erreur s'est produite lors du traitement de la demande du client : " + ex.Message);
                                    message_modifier_fullname = "Une erreur s'est produite lors du traitement de la demande du client : ";
                                    break;
                                }
                            }
                            else
                            {
                                message_modifier_fullname = "Ce Utilisateur n'exist pas !!!";
                            }
                            ModifierFullNameResp mfl = new ModifierFullNameResp();
                            mfl.Messsage = message_modifier_fullname;
                            mfl.NewName = request.NewName;
                            formatter.Serialize(networkStream, mfl);
                            break;
                        case ModifierInfosCl request:
                            Console.WriteLine("Mise à jour des informations de l'utilisateur : " + request.UtilisateurId + " nouveau informations : " + request.NewInfos);
                            string message_modifier_infos = "";
                            if (request != null)
                            {
                                try
                                {
                                    message_modifier_infos = UsersRepository.EditInfosUtilisateur(request.UtilisateurId, request.NewInfos);
                                }
                                catch (IOException)
                                {
                                    Console.WriteLine("Erreur d'entrée/sortie lors de la communication avec le client.");
                                    message_modifier_infos = "Erreur d'entrée/sortie lors de la communication avec le client.";
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Une erreur s'est produite lors du traitement de la demande du client : " + ex.Message);
                                    message_modifier_infos = "Une erreur s'est produite lors du traitement de la demande du client : ";
                                    break;
                                }
                            }
                            else
                            {
                                message_modifier_infos = "Ce Utilisateur n'exist pas !!!";
                            }
                            ModifierInfosResp mInfos = new ModifierInfosResp();
                            mInfos.Messsage = message_modifier_infos;
                            mInfos.NewInfos = request.NewInfos;
                            formatter.Serialize(networkStream, mInfos);
                            break;
                        case MessageEnvoyerCL request:
                            Console.WriteLine("Expéditeur : " + request.SenderId + " destinataire : " + request.ReceiverId + " Message : " + request.Content);
                            foreach (var socket in Program.Dict_Sockets_Client)
                            {
                                if (socket.Key == request.ReceiverId)
                                {
                                    NetworkStream networkStream1 = new NetworkStream(socket.Value);
                                    BinaryFormatter formatter1 = new BinaryFormatter();
                                    MessageRecuCL messageRecuCL = new MessageRecuCL();
                                    messageRecuCL.Content = request.Content;
                                    messageRecuCL.ReceiverId = request.ReceiverId;
                                    messageRecuCL.SenderId = request.SenderId;

                                    formatter1.Serialize(networkStream1, messageRecuCL);
                                }                           
                            }
                            MessageRepository.AddMessage(request);
                            
                            break;
                        case DeconnexionCL request:
                            Console.WriteLine("Deconnexion Client "+ request.IdUser);
                            request.etat = true;
                            formatter.Serialize(networkStream, request);
                            Program.Dict_Sockets_Client.Remove(request.IdUser);
                            liremessage = false;
                            break;
                        case EnLigneCL request:
                            Console.WriteLine("Verifier Client en ligne "+ request.UtilisateurId);
                            foreach (var socket in Program.Dict_Sockets_Client)
                            {
                                if (socket.Key == request.UtilisateurId)
                                {
                                    request.EnLigne = true;
                                }
                            }
                            formatter.Serialize(networkStream, request);
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
                Console.WriteLine("user DeConnecter");
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
