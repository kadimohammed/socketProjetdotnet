
using Guna.UI2.WinForms;
using Socket_Projet_Client.Outiles;
using Socket_Projet_Client.Sockets;
using Socket_Projet_Server.Classes;
using Socket_Projet_Server.Models;
using SocketsProject;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;



namespace Socket_Projet_Client
{
    public partial class ContactUC : UserControl
    {
        public static int Receiver;
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get => lbl_name.Text;
            set => lbl_name.Text = value;
        }
        public int Notification
        {
            get => int.Parse(notifText.Text);
            set => notifText.Text = value.ToString();
        }

        public string Message
        {
            get => lbl_msg.Text;
            set
            {
                if (value.Length > 10)
                {
                    lbl_msg.Text = value.Substring(0, 10) + " ...";
                }
                else
                {
                    lbl_msg.Text = value;
                }
            }
        }

        public string DateConnection
        {
            get => lbl_dateconnect.Text;
            set
            {
                DateTime dateValue;
                if (DateTime.TryParse(value, out dateValue))
                {
                    if (dateValue.Date == DateTime.Now.Date)
                    {
                        lbl_dateconnect.Text = dateValue.ToString("HH:mm");
                    }
                    else if (dateValue.Date.AddDays(1) == DateTime.Now.Date)
                    {
                        lbl_dateconnect.Text = "Hier, " + dateValue.ToString("HH:mm");
                    }
                    else
                    {
                        lbl_dateconnect.Text = dateValue.ToString("dd/MM/yyyy HH:mm");
                    }
                }
                else
                {
                    lbl_dateconnect.Text = "";
                }
            }

        }

        public Image Image
        {
            get => image.Image;
            set => image.Image = value;
        }

        public bool handleMouseLeave { get; set; } = true;

        public ContactUC()
        {
            InitializeComponent();
            cacherNotification();
        }


        public void cacherNotification()
        {
            this.lbl_notification.Hide();
            this.notifText.Hide();
        }

        public void AfficherNotification()
        {
            this.lbl_notification.Show();
            this.notifText.Show();
        }

        private void ContactUC_MouseEnter(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(17, 22, 32);
            ForeColor = SystemColors.ControlText;
        }



        private void ContactUC_MouseLeave(object sender, EventArgs e)
        {
            if (handleMouseLeave)
            {
                BackColor = Color.FromArgb(26, 32, 47);
                ForeColor = SystemColors.ControlText;
            }
        }

       

        public void ContactUC_Click(object sender, EventArgs e)
        {
            ClearImageBoxes();

            Login.f1.Messages_flowLayoutPanel2.HorizontalScroll.Enabled = false;
            Login.f1.Messages_flowLayoutPanel2.AutoScrollMinSize = new Size(0,0);


            this.cacherNotification();
            this.Notification = 0;
            Form1.contact_selected = this;
            Receiver = Id;

            foreach (ContactUC uc in Form1.contactList)
            {
                uc.BackColor = Color.FromArgb(26, 32, 47);
                uc.ForeColor = SystemColors.ControlText;
                uc.handleMouseLeave = true;
            }
            BackColor = Color.FromArgb(17, 22, 32);
            ForeColor = SystemColors.ControlText;
            handleMouseLeave = false;

            Login.f1.label16.Text = Name;
            Login.f1.label30.Text = Name;
            Login.f1.guna2CirclePictureBox13.Image = Image;
            Login.f1.guna2CirclePictureBox14.Image = Image;
            Login.f1.guna2CirclePictureBox13.Refresh();
            Login.f1.guna2CirclePictureBox14.Refresh();






            Login.f1.Messages_flowLayoutPanel2.Controls.Clear();
            LoginCl user = Login.user;
            List<Socket_Projet_Server.Models.Message> allMessages = new List<Socket_Projet_Server.Models.Message>();


            if (user != null && user.MessagesSent != null)
            {
                allMessages.AddRange(user.MessagesSent);
            }

            if (user != null && user.MessagesReceived != null)
            {
                allMessages.AddRange(user.MessagesReceived);
            }

            allMessages = allMessages.OrderByDescending(m => m.SendDate).ToList();

            MessageSenderUC msgSender;
            MessageReceverUC msgReceiver;
            int cmpteur_afficher_9_image = 1;
            foreach (var message in allMessages)
            {
                if (message.SenderId == user.Id && message.ReceiverId == Id && message.Content != "imagecontent")
                {
                    msgSender = new MessageSenderUC();
                    msgSender.Message = message.Content;
                    msgSender.Dock = DockStyle.Right;
                    msgSender.Image_user = MyUtility.GetImageFromByte(Login.user.Photo);

                    // Vérifier la date d'envoi du message
                    if (message.SendDate.Date == DateTime.Now.Date)
                    {
                        msgSender.DateTimeMessage = message.SendDate.ToString("HH:mm");
                    }
                    else if (message.SendDate.Date.AddDays(1) == DateTime.Now.Date)
                    {
                        msgSender.DateTimeMessage = "Hier,  " + message.SendDate.ToString("HH:mm");
                    }
                    else
                    {
                        msgSender.DateTimeMessage = message.SendDate.ToString("dd/MM/yyyy HH:mm");
                    }

                    Login.f1.Messages_flowLayoutPanel2.Controls.Add(msgSender);
                }
                else if (message.SenderId == Id && message.ReceiverId == user.Id && message.Content != "imagecontent")
                {
                    msgReceiver = new MessageReceverUC();
                    msgReceiver.Message = message.Content;
                    msgReceiver.Dock = DockStyle.Left;
                    msgReceiver.Image_user = Image;

                    if (message.SendDate.Date == DateTime.Now.Date)
                    {
                        msgReceiver.DateTimeMessage = message.SendDate.ToString("HH:mm");
                    }
                    else if (message.SendDate.Date == DateTime.Now.Date.AddDays(-1))
                    {
                        msgReceiver.DateTimeMessage = "Hier,   " + message.SendDate.ToString("HH:mm");
                    }
                    else
                    {
                        msgReceiver.DateTimeMessage = message.SendDate.ToString("dd/MM/yyyy HH:mm");
                    }

                    Login.f1.Messages_flowLayoutPanel2.Controls.Add(msgReceiver);
                }
                else if (message.Image != null)
                {
                    if (((message.SenderId == user.Id && message.ReceiverId == Id) || (message.SenderId == Id && message.ReceiverId == user.Id)) && message.Content == "imagecontent")
                    {
                        byte[] photoBytes = message.Image;
                        SetPictureBoxImage(cmpteur_afficher_9_image, photoBytes);
                        cmpteur_afficher_9_image++;
                    }
                }
                
            }










            try
            {
                Login.f1.enligneCircleButton9.Visible = false;
                Login.f1.activenowlabel1.Visible = false;
                Login.f1.activenowlabel2.Visible = false;
                Socket clientSocket = SocketSingleton.GetInstance();
                SocketSingleton.Connect(clientSocket);

                EnLigneCL enligne = new EnLigneCL();
                enligne.UtilisateurId = Id;

                NetworkStream networkStream = new NetworkStream(clientSocket);
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(networkStream, enligne);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une ereur s'est produite. Veuillez réessayer.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            


            /*
            Thread thread11 = new Thread(() =>
            {
                try
                {
                    Login.f1.Invoke((Action)(() =>
                    {
                        Login.f1.enligneCircleButton9.Visible = false;
                        Login.f1.activenowlabel1.Visible = false;
                        Login.f1.activenowlabel2.Visible = false;
                    }));

                    Socket clientSocket = SocketSingleton.GetInstance();
                    SocketSingleton.Connect(clientSocket);

                    EnLigneCL enligne = new EnLigneCL();
                    enligne.UtilisateurId = Id;

                    NetworkStream networkStream = new NetworkStream(clientSocket);
                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(networkStream, enligne);
                }
                catch (Exception ex)
                {
                    // Utilisez Invoke pour mettre à jour l'interface utilisateur à partir d'un thread de fond
                    Login.f1.Invoke((Action)(() =>
                    {
                        MessageBox.Show("Une erreur s'est produite. Veuillez réessayer.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });

            thread11.Start();
            */







        }



        private void SetPictureBoxImage(int pictureBoxNumber, byte[] photoBytes)
        {
            switch (pictureBoxNumber)
            {
                case 1:
                    Login.f1.guna2PictureBox1.Image = MyUtility.GetImageFromByte(photoBytes);
                    break;
                case 2:
                    Login.f1.guna2PictureBox2.Image = MyUtility.GetImageFromByte(photoBytes);
                    break;
                case 3:
                    Login.f1.guna2PictureBox3.Image = MyUtility.GetImageFromByte(photoBytes);
                    break;
                case 4:
                    Login.f1.guna2PictureBox4.Image = MyUtility.GetImageFromByte(photoBytes);
                    break;
                case 5:
                    Login.f1.guna2PictureBox5.Image = MyUtility.GetImageFromByte(photoBytes);
                    break;
                case 6:
                    Login.f1.guna2PictureBox6.Image = MyUtility.GetImageFromByte(photoBytes);
                    break;
                case 7:
                    Login.f1.guna2PictureBox7.Image = MyUtility.GetImageFromByte(photoBytes);
                    break;
                case 8:
                    Login.f1.guna2PictureBox8.Image = MyUtility.GetImageFromByte(photoBytes);
                    break;
                case 9:
                    Login.f1.guna2PictureBox9.Image = MyUtility.GetImageFromByte(photoBytes);
                    break;
                default:
                    break;
            }
        }




        // Définissez une méthode pour réinitialiser le contenu de tous les PictureBox
        public void ClearImageBoxes()
        {
            // Réinitialiser le contenu de chaque PictureBox individuellement
            Login.f1.guna2PictureBox1.Image = new Bitmap(1, 1); // Réinitialiser avec une image vide
            Login.f1.guna2PictureBox2.Image = new Bitmap(1, 1); // Réinitialiser avec une image vide
            Login.f1.guna2PictureBox3.Image = new Bitmap(1, 1); // Réinitialiser avec une image vide
            Login.f1.guna2PictureBox4.Image = new Bitmap(1, 1); // Réinitialiser avec une image vide
            Login.f1.guna2PictureBox5.Image = new Bitmap(1, 1); // Réinitialiser avec une image vide
            Login.f1.guna2PictureBox6.Image = new Bitmap(1, 1); // Réinitialiser avec une image vide
            Login.f1.guna2PictureBox7.Image = new Bitmap(1, 1); // Réinitialiser avec une image vide
            Login.f1.guna2PictureBox8.Image = new Bitmap(1, 1); // Réinitialiser avec une image vide
            Login.f1.guna2PictureBox9.Image = new Bitmap(1, 1); // Réinitialiser avec une image vide
        }







    }
}
