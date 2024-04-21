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
        Socket clientSocket;
        public static List<ContactUC> contactList;
        MyContext _context;
        private bool OpenFormInfo = false;
        UsersInfos formChild;


        public Form1()
        {
            InitializeComponent();
            _context = ContextFactory.getContext();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            loadcontact();

            if (contactList != null && contactList.Count > 0)
            {
                contactList[0].ContactUC_Click(sender, e);
            }

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

        private void UserPicture_Click(object sender, EventArgs e)
        {
            if(OpenFormInfo)
            {
                formChild.Close();
                OpenFormInfo = false;
            }
            else
            {
                formChild = new UsersInfos();
                // Obtenir les dimensions du formulaire parent
                int parentWidth = this.Width;
                int parentHeight = this.Height;

                // Obtenir les dimensions de la fenêtre enfant
                int childWidth = formChild.Width;
                int childHeight = formChild.Height;

                // Calculer la position de la fenêtre enfant dans le coin inférieur gauche du formulaire parent
                int childX = this.Left + 10;
                int childY = this.Bottom - childHeight - 50;

                // Positionner la fenêtre enfant
                formChild.StartPosition = FormStartPosition.Manual;
                formChild.Location = new Point(childX, childY);

                // Afficher la fenêtre enfant
                formChild.Show();
                OpenFormInfo = true;
            }
        }

        private void UserPicture_MouseEnter(object sender, EventArgs e)
        {
            UserPicture.ShadowDecoration.Enabled = true;
        }

        private void UserPicture_MouseLeave(object sender, EventArgs e)
        {
            UserPicture.ShadowDecoration.Enabled = false;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            new AjouterContact().ShowDialog();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            string textderecherche = guna2TextBox1.Text;
            if (textderecherche != "")
            {
                loadcontact(guna2TextBox1.Text);
            }
            else
            {
                loadcontact();
            }
            

            if (contactList != null && contactList.Count > 0)
            {
                contactList[0].ContactUC_Click(sender, e);
            }

            foreach (ContactUC c in contactList)
            {
                flowLayoutPanel1.Controls.Add(c);
            }
        }











        // function ///////////////////////////////////////////////////////////////


        public void loadcontact(string TextRecherche)
        {

            LoginCl user = Login.user;

            if (user != null && user.Contacts != null)
            {
                contactList = new List<ContactUC>();
                foreach (var contact in user.Contacts)
                {
                    ContactUC contactUC = new ContactUC();
                    contactUC.Name = contact.ContactUser.FullName;
                    contactUC.Notification = 0;

                    if (contact.ContactUser.FullName.Contains(TextRecherche))
                    {
                        if (user != null && contact != null && contact.ContactUser != null &&
                            user.MessagesSent != null && contact.ContactUser.MessagesReceived != null &&
                            contact.ContactUser.MessagesSent != null && user.MessagesReceived != null)
                        {
                            var allMessages = user.MessagesSent
                                .Concat(contact.ContactUser.MessagesReceived)
                                .Concat(contact.ContactUser.MessagesSent)
                                .Concat(user.MessagesReceived);

                            var lastMessage = allMessages
                                .Where(m => (m.SenderId == user.Id && m.ReceiverId == contact.ContactUser.Id) ||
                                            (m.SenderId == contact.ContactUser.Id && m.ReceiverId == user.Id))
                                .OrderByDescending(m => m.SendDate)
                                .FirstOrDefault();


                            contactUC.Message = lastMessage?.Content ?? "";
                            contactUC.DateConnection = lastMessage?.SendDate.ToShortDateString() ?? "";
                        }
                        else
                        {
                            contactUC.Message = "";
                            contactUC.DateConnection = "";
                        }

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
        }




        public void loadcontact()
        {

            LoginCl user = Login.user;

            if (user != null && user.Contacts != null)
            {
                contactList = new List<ContactUC>();
                foreach (var contact in user.Contacts)
                {
                    ContactUC contactUC = new ContactUC();

                    if (user != null && contact != null && contact.ContactUser != null &&
                        user.MessagesSent != null && contact.ContactUser.MessagesReceived != null &&
                        contact.ContactUser.MessagesSent != null && user.MessagesReceived != null)
                    {
                        var allMessages = user.MessagesSent
                            .Concat(contact.ContactUser.MessagesReceived)
                            .Concat(contact.ContactUser.MessagesSent)
                            .Concat(user.MessagesReceived);

                        var lastMessage = allMessages
                            .Where(m => (m.SenderId == user.Id && m.ReceiverId == contact.ContactUser.Id) ||
                                        (m.SenderId == contact.ContactUser.Id && m.ReceiverId == user.Id))
                            .OrderByDescending(m => m.SendDate)
                            .FirstOrDefault();

                        contactUC.Name = contact.ContactUser.FullName;
                        contactUC.Notification = 0;
                        contactUC.Message = lastMessage?.Content ?? "";
                        contactUC.DateConnection = lastMessage?.SendDate.ToShortDateString() ?? "";
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
        }


    }
}
