using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using Socket_Projet_Client.Sockets;
using Socket_Projet_Server.Models;
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

namespace SocketsProject
{
    public partial class Login : Form
    {
        public static Utilisateur user;
        public static Form1 f1;
        Socket clientSocket;


        public Login()
        {
            InitializeComponent();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //bool a = UsersRepository.InsererUtilisateurAvecImage("0444444444", "kadi", "1234", "C:\\Users\\MO KADI\\Desktop\\Med kadi\\myimg.png", "mo kadi piratage");
            try
            {
                // recuperer telephone et password
                string Telephone = TelephoneTextBox.Text;
                string Password = PasswordTextBox.Text;


                // Connexion au serveur si nécessaire
                clientSocket = SocketSingleton.GetInstance();
                SocketSingleton.Connect(clientSocket);

                Utilisateur utilisateurToSend = new Utilisateur { Telephone = Telephone, Password = Password };
                NetworkStream networkStream = new NetworkStream(clientSocket);

                // Création du BinaryFormatter
                BinaryFormatter formatter = new BinaryFormatter();

                // Envoi de l'utilisateur au serveur
                formatter.Serialize(networkStream, utilisateurToSend);

                // Réception de l'utilisateur depuis le serveur
                user = (Utilisateur)formatter.Deserialize(networkStream);

                if (user != null && user.UserID != -1)
                {
                    f1 = new Form1();
                    f1.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Numéro de téléphone ou mot de passe incorrect. Veuillez réessayer.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Une ereur s'est produite. Veuillez réessayer.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
