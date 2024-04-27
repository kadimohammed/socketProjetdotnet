using Guna.UI2.WinForms;
using Socket_Projet_Client.Sockets;
using Socket_Projet_Server.Classes;
using Socket_Projet_Server.Mappers;
using Socket_Projet_Server.Models;
using Socket_Projet_Server.Repository;
using SocketsProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

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

                while (true)
                {
                    receivedObject = formatter.Deserialize(networkStream);

                    switch (receivedObject)
                    {
                        case AjouterContactResp request:

                            if (request.loginCl != null && request.loginCl.Id != -1)
                            {
                                Login.user = request.loginCl;
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
                            MessageReceverUC msgRecever = new MessageReceverUC();
                            msgRecever.Message = request.Content;
                            msgRecever.DateTimeMessage = DateTime.Now.ToString();
                            msgRecever.Dock = DockStyle.Right;
                            msgRecever.Image_user = MyUtility.GetImageFromByte(Login.user.Photo);
                            Login.f1.Invoke((MethodInvoker)delegate {
                                Login.f1.Messages_flowLayoutPanel2.Controls.Add(msgRecever);
                                Login.f1.Messages_flowLayoutPanel2.Controls.SetChildIndex(msgRecever, 0);
                            });
                            
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
