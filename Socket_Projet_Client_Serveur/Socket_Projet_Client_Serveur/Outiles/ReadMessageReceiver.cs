using Socket_Projet_Client.Sockets;
using Socket_Projet_Server.Classes;
using SocketsProject;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Message = Socket_Projet_Server.Models.Message;

namespace Socket_Projet_Client.Outiles
{
    public class ReadMessageReceiver
    {
        private static object receivedObject;
        public static void ReceiveMessages()
        {
            try
            {
                string message;
                Socket clientSocket = SocketSingleton.GetInstance();
                SocketSingleton.Connect(clientSocket);
                NetworkStream networkStream = new NetworkStream(clientSocket);
                BinaryFormatter formatter = new BinaryFormatter();

                bool lireMessages = true;

                while (lireMessages)
                {
                    receivedObject = formatter.Deserialize(networkStream);

                    switch (receivedObject)
                    {
                        case AjouterContactResp request:

                            if (request.loginCl != null && request.loginCl.Id != -1)
                            {

                                // Utiliser Invoke pour accéder à l'interface utilisateur en toute sécurité.
                                Login.f1.Invoke((MethodInvoker)delegate {
                                    Login.user = request.loginCl;

                                    // Réinitialiser et recharger la liste des contacts.
                                    Form1.contactList = new List<ContactUC>();
                                    Login.f1.loadcontact();

                                    // Effacer et ajouter les contacts dans flowLayoutPanel1.
                                    Login.f1.flowLayoutPanel1.Controls.Clear();
                                    foreach (ContactUC c in Form1.contactList)
                                    {
                                        Login.f1.flowLayoutPanel1.Controls.Add(c);
                                    }
                                });

                            }

                            if (request.Messsage != "")
                            {
                                MessageBox.Show(request.Messsage, "Erreur Ajout d'un Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Numéro de téléphone Nexist pas. Veuillez réessayer.", "Erreur Ajout d'un Contact", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            break;
                            
                        case ModifierFullNameResp request:

                            if (request.Messsage == "FullName Bien Modifier.")
                            {
                                Login.f1.Invoke((MethodInvoker)delegate {
                                    Login.user.FullName = request.NewName;
                                    Login.f1.formChild.EditNameButton.Visible = true;
                                    Login.f1.formChild.guna2Button1.Visible = false;
                                    Login.f1.formChild.EditNameTextBox.Enabled = false;
                                    MessageBox.Show(request.Messsage, "Modifier Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                });
                            }
                            else
                            {
                                MessageBox.Show(request.Messsage, "Erreur Modifier Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            break;

                        case ModifierInfosResp request:

                            if (request.Messsage == "Infos Bien Modifier.")
                            {
                                Login.f1.Invoke((MethodInvoker)delegate {
                                    Login.user.Infos = request.NewInfos;
                                    Login.f1.formChild.EditInfosButton.Visible = true;
                                    Login.f1.formChild.guna2Button2.Visible = false;
                                    Login.f1.formChild.EditInfosTextBox.Enabled = false;
                                    MessageBox.Show(request.Messsage, "Modifier Infos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                });
                            }
                            else
                            {
                                MessageBox.Show(request.Messsage, "Erreur Modifier Infos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            break;

                        case MessageRecuCL request:
                            if (Form1.contact_selected.Id == request.SenderId)
                            {
                                Login.f1.Invoke((MethodInvoker)delegate {
                                    MessageReceverUC msgRecever = new MessageReceverUC();
                                    msgRecever.Message = request.Content;
                                    msgRecever.DateTimeMessage = DateTime.Now.ToString("HH:mm");
                                    Form1.contact_selected.DateConnection = DateTime.Now.ToString("");
                                    Form1.contact_selected.Message = request.Content;
                                    msgRecever.Dock = DockStyle.Left;
                                    msgRecever.Image_user = Form1.contact_selected.Image;
                                    Login.f1.Messages_flowLayoutPanel2.Controls.Add(msgRecever);
                                    Login.f1.Messages_flowLayoutPanel2.Controls.SetChildIndex(msgRecever, 0);
                                });
                            }
                            else
                            {
                                
                                foreach (var contact in Form1.contactList)
                                {
                                    if (contact.Id == request.SenderId)
                                    {
                                        Login.f1.Invoke((MethodInvoker)delegate {
                                            contact.Message = request.Content;
                                        });
                                        Login.f1.Invoke(new Action(() =>
                                        {
                                            contact.AfficherNotification();
                                            contact.Notification = contact.Notification + 1;
                                        }));
                                        break;
                                        
                                    }
                                }
                            }
                            Message m = new Message();
                            m.SenderId = request.SenderId;
                            m.ReceiverId = request.ReceiverId;
                            m.Content = request.Content;
                            Login.user.MessagesReceived.Add(m);
                            break;
                        case DeconnexionCL request:
                            if (request.etat)
                            {
                                lireMessages = false;
                            }
                            break;
                        case EnLigneCL request:
                            if (request.EnLigne)
                            {
                                // Vérifiez si l'appel est nécessaire depuis un thread différent
                                if (Login.f1.InvokeRequired)
                                {
                                    // Utilisez Invoke pour effectuer l'appel sur le thread UI
                                    Login.f1.Invoke(new Action(() =>
                                    {
                                        Login.f1.enligneCircleButton9.Visible = true;
                                        Login.f1.activenowlabel1.Visible = true;
                                        Login.f1.activenowlabel2.Visible = true;
                                    }));
                                }
                                else
                                {
                                    // Si vous êtes déjà sur le thread UI, effectuez directement les changements
                                    Login.f1.enligneCircleButton9.Visible = true;
                                    Login.f1.activenowlabel1.Visible = true;
                                    Login.f1.activenowlabel2.Visible = true;
                                }
                            }

                            break;
                        default:
                            Console.WriteLine("Type de requête non pris en charge.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite. Veuillez réessayer.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
