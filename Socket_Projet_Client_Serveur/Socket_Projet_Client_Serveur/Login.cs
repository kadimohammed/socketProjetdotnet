using Socket_Projet_Client.Sockets;
using Socket_Projet_Server.Models;
using Socket_Projet_Server.Repository;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace SocketsProject
{
    public partial class Login : Form
    {
        public static LoginCl user;
        public static Form1 f1;
        public static Socket clientSocket;


        public Login()
        {
            InitializeComponent();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            //bool c = UsersRepository.InsererUtilisateurAvecImage("5", "pitos", "5", "C:\\Users\\MO KADI\\Desktop\\Nouveau dossier\\korg.jpg", "dddd");
            //bool a = UsersRepository.InsererUtilisateurAvecImage("6", "pitos","6", "C:\\Users\\MO KADI\\Desktop\\Med kadi\\p2.jpg", "aaaaaa");
            //bool b = UsersRepository.InsererUtilisateurAvecImage("3", "camavinga", "3", "C:\\Users\\MO KADI\\Desktop\\Med kadi\\p3.jpg", "bbbbbbb");
            //bool f = UsersRepository.InsererUtilisateurAvecImage("5", "Lolo", "5", "C:\\Users\\MO KADI\\Desktop\\Med kadi\\p4.jpg", "llllllll");  

            //UsersRepository.UpdateUserPhoto(1, "C:\\Users\\MO KADI\\Desktop\\photos\\mokadi.jpg");

            try
            {
                guna2Button1.Enabled = false;
                // Récupérer le téléphone et le mot de passe
                string Telephone = TelephoneTextBox.Text;
                string Password = PasswordTextBox.Text;

                // Connexion au serveur si nécessaire
                clientSocket = SocketSingleton.GetInstance();
                SocketSingleton.Connect(clientSocket);

                LoginCl utilisateurToSend = new LoginCl { Telephone = Telephone, Password = Password };

                // Envoi de l'utilisateur au serveur de manière asynchrone
               
                NetworkStream networkStream = new NetworkStream(clientSocket);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(networkStream, utilisateurToSend);







                user = (LoginCl)formatter.Deserialize(networkStream);
        

                if (user != null && user.Id != -1)
                {
                    // Si l'utilisateur est authentifié avec succès, afficher la nouvelle forme
                    f1 = new Form1();
                    f1.Show();
                    this.Hide();
                }
                else
                {
                    // Afficher un message d'erreur si l'authentification échoue
                    MessageBox.Show("Numéro de téléphone ou mot de passe incorrect. Veuillez réessayer.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                // Gérer toute exception qui pourrait survenir pendant le processus d'authentification
                MessageBox.Show("Une erreur s'est produite. Veuillez réessayer.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                guna2Button1.Enabled = true;
            }

        }

    }
}
