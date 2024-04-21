using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using Socket_Projet_Client.Sockets;
using Socket_Projet_Server.Models;
using Socket_Projet_Server.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketsProject
{
    public partial class Login : Form
    {
        public static LoginCl user;
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


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //bool c = UsersRepository.InsererUtilisateurAvecImage("1", "manini", "1", "C:\\Users\\MO KADI\\Desktop\\Med kadi\\myimg.png", "cccccccc");
            //bool a = UsersRepository.InsererUtilisateurAvecImage("2", "pitos","2", "C:\\Users\\MO KADI\\Desktop\\Med kadi\\p2.jpg", "aaaaaa");
            //bool b = UsersRepository.InsererUtilisateurAvecImage("3", "camavinga", "3", "C:\\Users\\MO KADI\\Desktop\\Med kadi\\p3.jpg", "bbbbbbb");
            //bool f = UsersRepository.InsererUtilisateurAvecImage("4", "manini", "4", "C:\\Users\\MO KADI\\Desktop\\Med kadi\\p4.jpg", "cccccccc");  

            try
            {
                // recuperer telephone et password
                string Telephone = TelephoneTextBox.Text;
                string Password = PasswordTextBox.Text;

                // Connexion au serveur si nécessaire
                clientSocket = SocketSingleton.GetInstance();
                SocketSingleton.Connect(clientSocket);

                LoginCl utilisateurToSend = new LoginCl { Telephone = Telephone, Password = Password };
                NetworkStream networkStream = new NetworkStream(clientSocket);

                // Création du BinaryFormatter
                BinaryFormatter formatter = new BinaryFormatter();

                // Envoi de l'utilisateur au serveur
                formatter.Serialize(networkStream, utilisateurToSend);

                // Réception de l'utilisateur depuis le serveur
                user = (LoginCl)formatter.Deserialize(networkStream);

                if (user != null && user.Id != -1)
                {
                    f1 = new Form1();
                    f1.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Numéro de téléphone ou mot de passe incorrect. Veuillez réessayer.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une ereur s'est produite. Veuillez réessayer.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
