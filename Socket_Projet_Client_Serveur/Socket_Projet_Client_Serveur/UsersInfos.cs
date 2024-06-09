using Socket_Projet_Client.Outiles;
using Socket_Projet_Client.Sockets;
using Socket_Projet_Server.Classes;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace SocketsProject
{
    public partial class UsersInfos : Form
    {

        public UsersInfos()
        {
            
            InitializeComponent();

            guna2Button1.Visible = false;
            guna2Button2.Visible = false;
            EditNameTextBox.Text = Login.user.FullName;
            EditInfosTextBox.Text = Login.user.Infos;
            Telephonelabel.Text = Login.user.Telephone;
            byte[] photoBytes = Login.user.Photo;
            UserPicture.Image = MyUtility.GetImageFromByte(photoBytes);

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void DeconnexionButton_Click(object sender, EventArgs e)
        {
            try
            {
                DeconnexionCL deconnexion = new DeconnexionCL();
                deconnexion.IdUser = Login.user.Id;
                deconnexion.etat = false;
                Socket clientSocket = SocketSingleton.GetInstance();
                SocketSingleton.Connect(clientSocket);

                NetworkStream networkStream = new NetworkStream(clientSocket);
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(networkStream, deconnexion);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une ereur s'est produite. Veuillez réessayer.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditNameButton_Click(object sender, EventArgs e)
        {
            EditNameButton.Visible = false;
            guna2Button1.Visible = true;
            EditNameTextBox.Enabled = true;
            guna2Button1.Text = EditNameTextBox.Text.Length.ToString() + "/25";
        }


        private void guna2Button1_MouseEnter(object sender, EventArgs e)
        {
            guna2Button1.Text = "Terminé";
        }

        private void guna2Button1_MouseLeave(object sender, EventArgs e)
        {
            guna2Button1.Text = EditNameTextBox.Text.Length.ToString()+"/25";
        }

        private void EditNameTextBox_TextChanged(object sender, EventArgs e)
        {
            guna2Button1.Text = EditNameTextBox.Text.Length.ToString() + "/25";
        }

        private void EditInfosButton_Click(object sender, EventArgs e)
        {
            EditInfosButton.Visible = false;
            guna2Button2.Visible = true;
            EditInfosTextBox.Enabled = true;
            guna2Button2.Text = EditInfosTextBox.Text.Length.ToString() + "/139";
        }

        private void guna2Button2_MouseEnter(object sender, EventArgs e)
        {
            guna2Button2.Text = "Terminé";
        }

        private void guna2Button2_MouseLeave(object sender, EventArgs e)
        {
            guna2Button2.Text = EditInfosTextBox.Text.Length.ToString() + "/139";
        }

        private void EditInfosTextBox_TextChanged(object sender, EventArgs e)
        {
            guna2Button2.Text = EditInfosTextBox.Text.Length.ToString() + "/139";
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string newName = EditNameTextBox.Text;

                Socket clientSocket = SocketSingleton.GetInstance();
                SocketSingleton.Connect(clientSocket);

                ModifierNameCl utilisateurToSend = new ModifierNameCl { UtilisateurId = Login.user.Id, NewName = newName, };
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



        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                string newInfos = EditInfosTextBox.Text;

                // Connexion au serveur si nécessaire
                Socket clientSocket = SocketSingleton.GetInstance();
                SocketSingleton.Connect(clientSocket);

                ModifierInfosCl utilisateurToSend = new ModifierInfosCl { UtilisateurId = Login.user.Id, NewInfos = newInfos, };
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
