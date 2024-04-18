using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using Socket_Projet_Client;
using Socket_Projet_Client.Sockets;
using Socket_Projet_Server.Factory;
using Socket_Projet_Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SocketsProject
{
    public partial class Form1 : Form
    {
        private List<Form> childFormsList = new List<Form>();
        Socket clientSocket;
        public static  List<ContactUC> contactList;
        MyContext _context;


        public void loadcontact()
        {
            
            Utilisateur user = Login.user;
            if (user != null && user.Contacts != null)
            {
                contactList = new List<ContactUC>();
                foreach (var contact in user.Contacts)
                {
                    ContactUC contactUC = new ContactUC();
                    contactUC.Name = contact.ContactUser.FullName;
                    contactUC.Notification = 0;
                    contactUC.DateConnection = DateTime.Now.ToShortDateString();


                    byte[] photoBytes = contact.ContactUser.Photo;

                    if (photoBytes != null && photoBytes.Length > 0)
                    {
                        try
                        {
                            using (MemoryStream ms = new MemoryStream(photoBytes))
                            {
                                contactUC.Image = Image.FromStream(ms);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine("Erreur lors de la conversion des données de l'image : " + ex.Message);
                        }
                    }
                    else
                    {
                        contactUC.Image = null;
                    }

                    contactList.Add(contactUC);

                }
            }   
        }

        public Form1()
        {
            InitializeComponent();
            _context = ContextFactory.getContext();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            flowLayoutPanel1.Controls.Clear();

            loadcontact();

            foreach (ContactUC c in contactList)
            {
                flowLayoutPanel1.Controls.Add(c);
            }



            if (Login.user != null)
            {
                byte[] photoBytes = Login.user.Photo;

                if (photoBytes != null && photoBytes.Length > 0)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(photoBytes))
                        {
                            UserPicture.Image = Image.FromStream(ms);
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine("Erreur lors de la conversion des données de l'image : " + ex.Message);
                    }
                }
                else
                {
                    UserPicture.Image = null;
                }
            }

        }



        private void guna2GradientPanel1_MouseEnter(object sender, EventArgs e)
        {
            ((Guna.UI2.WinForms.Guna2GradientPanel)sender).ShadowDecoration.Color = Color.DeepPink;
        }

        private void guna2GradientPanel1_MouseLeave(object sender, EventArgs e)
        {
            ((Guna.UI2.WinForms.Guna2GradientPanel)sender).ShadowDecoration.Color = Color.FromArgb(17, 22, 32);
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in childFormsList.ToList())
            {
                childForm.Close();
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

            foreach (Form childForm in childFormsList.ToList())
            {
                childForm.Close();
            }

        }

        private void UserPicture_Click(object sender, EventArgs e)
        {
            UsersInfos formChild = new UsersInfos();
            childFormsList.Add(formChild);
            // Obtenir les dimensions du formulaire parent
            int parentWidth = this.Width;
            int parentHeight = this.Height;

            // Obtenir les dimensions de la fenêtre enfant
            int childWidth = formChild.Width;
            int childHeight = formChild.Height;

            // Calculer la position de la fenêtre enfant dans le coin inférieur gauche du formulaire parent
            int childX = this.Left + 10;
            int childY = this.Bottom - childHeight - 10;

            // Positionner la fenêtre enfant
            formChild.StartPosition = FormStartPosition.Manual;
            formChild.Location = new Point(childX, childY);

            // Afficher la fenêtre enfant
            formChild.Show();
        }

        private void UserPicture_MouseEnter(object sender, EventArgs e)
        {
            UserPicture.ShadowDecoration.Enabled = true;
        }

        private void UserPicture_MouseLeave(object sender, EventArgs e)
        {
            UserPicture.ShadowDecoration.Enabled = false;
        }

        private void guna2CircleButton6_Click(object sender, EventArgs e)
        {
            /*
            string message = messageTextBox2.Text;
            if (message != "")
            {
                try
                {
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
            */
        }


        

        
    }
}
