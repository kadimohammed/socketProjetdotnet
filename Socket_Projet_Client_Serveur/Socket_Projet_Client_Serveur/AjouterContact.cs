using Microsoft.VisualBasic.ApplicationServices;
using Socket_Projet_Client.Sockets;
using Socket_Projet_Server.Classes;
using Socket_Projet_Server.Models;
using SocketsProject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Socket_Projet_Client
{
    public partial class AjouterContact : Form
    {
        public AjouterContact()
        {
            InitializeComponent();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AjouterContactButton_Click(object sender, EventArgs e)
        {
            try
            {
                string telephone = TelephoneTextBox.Text;

                // Connexion au serveur si nécessaire
                Socket clientSocket = SocketSingleton.GetInstance();
                SocketSingleton.Connect(clientSocket);

                AjouterContactCL utilisateurToSend = new AjouterContactCL { ContactTelephone = telephone, UtilisateurId = Login.user.Id };
                NetworkStream networkStream = new NetworkStream(clientSocket);

                // Création du BinaryFormatter
                BinaryFormatter formatter = new BinaryFormatter();

                // Envoi de l'utilisateur au serveur
                formatter.Serialize(networkStream, utilisateurToSend);

                // Réception de l'utilisateur depuis le serveur
                LoginCl newuser = (LoginCl)formatter.Deserialize(networkStream);
                if(newuser != null && newuser.Id != -1)
                {
                    Login.user = newuser;
                }

                string message = (string)formatter.Deserialize(networkStream);

                if (message != "")
                {
                    this.Hide();
                    MessageBox.Show(message, "Erreur Ajout d'un Contact", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Numéro de téléphone Nexist pas. Veuillez réessayer.", "Erreur Ajout d'un Contact", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une ereur s'est produite. Veuillez réessayer.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
