using Socket_Projet_Client;
using Socket_Projet_Client.Outiles;
using Socket_Projet_Client.Sockets;
using Socket_Projet_Client_Serveur;
using Socket_Projet_Server.Classes;
using Socket_Projet_Server.Models;
using System.Data;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Message = Socket_Projet_Server.Models.Message;

namespace SocketsProject
{
    public partial class Form1 : Form
    {
        public static List<ContactUC> contactList;
        private bool OpenFormInfo = false;
        public UsersInfos formChild;
        public static ContactUC contact_selected;


        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.enligneCircleButton9.Visible = false;
            this.activenowlabel1.Visible = false;
            this.activenowlabel2.Visible = false;

            Task.Run(() =>
            {
                ReadMessageReceiver.ReceiveMessages();
                this.Invoke((MethodInvoker)delegate
                {
                    Login.user = null;
                    //formChild.Close();
                    Login.f1.Close();
                    SocketSingleton.clientSocket = null;
                    Program.login = new Login();
                    Program.login.Show();
                });
            });


            // Obtenir la valeur maximale de défilement vertical
            int maxValue = Messages_flowLayoutPanel2.VerticalScroll.Maximum - Messages_flowLayoutPanel2.VerticalScroll.LargeChange;

            // Définir la position de défilement au maximum
            Messages_flowLayoutPanel2.VerticalScroll.Value = maxValue;



            flowLayoutPanel1.Controls.Clear();

            loadcontact();

            if (contactList != null && contactList.Count > 0)
            {
                contactList[0].ContactUC_Click(sender, e);
                contact_selected = contactList[0];
            }

            foreach (ContactUC c in contactList)
            {
                flowLayoutPanel1.Controls.Add(c);
            }

            if (Login.user != null)
            {
                byte[] photoBytes = Login.user.Photo;
                UserPicture.Image = MyUtility.GetImageFromByte(photoBytes);
            }
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
            else
            {
                Messages_flowLayoutPanel2.Controls.Clear();
            }

            foreach (ContactUC c in contactList)
            {
                flowLayoutPanel1.Controls.Add(c);
            }
        }



        private void SendMesgCircleButton_Click(object sender, EventArgs e)
        {
            this.envoyerMessage();
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




        // function ///////////////////////////////////////////////////////////////


        public void loadcontact(string TextRecherche)
        {

            LoginCl user = Login.user;

            if (user != null && user.Contacts != null)
            {
                contactList = new List<ContactUC>();
                ContactUC contactUC;
                foreach (var contact in user.Contacts)
                {
                    contactUC = new ContactUC();
                    contactUC.Id = contact.ContactUser.Id;
                    contactUC.Name = contact.ContactUser.FullName;
                    contactUC.Notification = 0;

                    if (contact.ContactUser.FullName.IndexOf(TextRecherche, StringComparison.OrdinalIgnoreCase) >= 0)
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
                        contactUC.Image = MyUtility.GetImageFromByte(photoBytes);

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
                ContactUC contactUC;
                foreach (var contact in user.Contacts)
                {
                    contactUC = new ContactUC();

                    if (contact != null && contact.ContactUser != null)
                    {
                        IEnumerable<Message> allMessages = Enumerable.Empty<Message>();

                        if (user.MessagesSent != null)
                        {
                            allMessages = allMessages.Concat(user.MessagesSent);
                        }

                        if (contact.ContactUser.MessagesReceived != null)
                        {
                            allMessages = allMessages.Concat(contact.ContactUser.MessagesReceived);
                        }

                        if (contact.ContactUser.MessagesSent != null)
                        {
                            allMessages = allMessages.Concat(contact.ContactUser.MessagesSent);
                        }

                        if (user.MessagesReceived != null)
                        {
                            allMessages = allMessages.Concat(user.MessagesReceived);
                        }

                        if (allMessages.Any())
                        {

                            var lastMessage = allMessages
                                .Where(m => (m.SenderId == user.Id && m.ReceiverId == contact.ContactUser.Id) ||
                                            (m.SenderId == contact.ContactUser.Id && m.ReceiverId == user.Id))
                                .OrderByDescending(m => m.SendDate)
                                .FirstOrDefault();

                            contactUC.Id = contact.ContactUser.Id;
                            contactUC.Name = contact.ContactUser.FullName;
                            contactUC.Notification = 0;
                            contactUC.Message = lastMessage?.Content ?? "";
                            contactUC.DateConnection = lastMessage?.SendDate.ToShortDateString() ?? "";
                            byte[] photoBytes = contact.ContactUser.Photo;
                            contactUC.Image = MyUtility.GetImageFromByte(photoBytes);
                        }
                        else
                        {
                            contactUC.Id = contact.ContactUser.Id;
                            contactUC.Name = contact.ContactUser.FullName;
                            contactUC.Notification = 0;
                            contactUC.Message = "";
                            contactUC.DateConnection = "";
                            byte[] photoBytes = contact.ContactUser.Photo;
                            contactUC.Image = MyUtility.GetImageFromByte(photoBytes);
                        }

                        contactList.Add(contactUC);
                    }
                }
            }
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            AppelAudio appel = new AppelAudio();
            appel.nom.Text = contact_selected.Name;
            appel.photo.Image = contact_selected.Image;
            appel.ShowDialog();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            new Infos().ShowDialog();
        }


        private bool emogieform = false;
        Emojis em;

        private void guna2CircleButton2_MouseClick(object sender, MouseEventArgs e)
        {

            if (emogieform)
            {
                em.Close();
                emogieform = false;
            }
            else
            {
                em = new Emojis();
                em.Owner = this;
                // Obtenir les dimensions de la fenêtre enfant
                int childWidth = em.Width;
                int childHeight = em.Height;

                // Calculer la position de la fenêtre enfant dans le coin inférieur gauche du formulaire parent
                int childX = this.Left + 350;
                int childY = this.Bottom - childHeight - 50;

                // Positionner la fenêtre enfant
                em.StartPosition = FormStartPosition.Manual;
                em.Location = new Point(childX, childY);

                // Afficher la fenêtre enfant
                em.Show();
                emogieform = true;
            }



        }

        private void UserPicture_MouseClick(object sender, MouseEventArgs e)
        {
            if (OpenFormInfo)
            {
                formChild.Close();
                OpenFormInfo = false;
            }
            else
            {
                formChild = new UsersInfos();
                formChild.Owner = this;
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

        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.envoyerMessage();
            }
        }






        private void envoyerMessage()
        {
            string message = MessageTextBox.Text;
            if (!string.IsNullOrEmpty(message))
            {
                MessageSenderUC msgSender = new MessageSenderUC();
                msgSender.Message = message;
                msgSender.DateTimeMessage = DateTime.Now.ToString("HH:mm");
                msgSender.Dock = DockStyle.Right;
                msgSender.Image_user = MyUtility.GetImageFromByte(Login.user.Photo);
                Login.f1.Messages_flowLayoutPanel2.Controls.Add(msgSender);
                Login.f1.Messages_flowLayoutPanel2.Controls.SetChildIndex(msgSender, 0);
                MessageTextBox.Text = "";
                //Login.f1.Messages_flowLayoutPanel2.ScrollControlIntoView(msgSender);

                contact_selected.Message = message;


                try
                {

                    Message m = new Message();
                    m.Content = message;
                    m.ReceiverId = ContactUC.Receiver;
                    m.SenderId = Login.user.Id;
                    Login.user.MessagesSent.Add(m);



                    Socket clientSocket = SocketSingleton.GetInstance();
                    SocketSingleton.Connect(clientSocket);

                    MessageEnvoyerCL messageReceverUC = new MessageEnvoyerCL();
                    messageReceverUC.Content = message;
                    messageReceverUC.ReceiverId = ContactUC.Receiver;
                    messageReceverUC.SenderId = Login.user.Id;
                    messageReceverUC.SendDate = DateTime.Now;
                    NetworkStream networkStream = new NetworkStream(clientSocket);
                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(networkStream, messageReceverUC);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une ereur s'est produite. Veuillez réessayer.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void UserPicture_Click(object sender, EventArgs e)
        {

        }

        private void Messages_flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}
