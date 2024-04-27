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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une ereur s'est produite. Veuillez réessayer.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
