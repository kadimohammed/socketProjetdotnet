
using Socket_Projet_Client.Outiles;
using Socket_Projet_Server.Models;
using SocketsProject;



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
            get => int.Parse(lbl_notification.Text);
            set => lbl_notification.Text = value.ToString();
        }

        public string Message
        {
            get => lbl_msg.Text;
            set => lbl_msg.Text = value;
        }

        public string DateConnection
        {
            get => lbl_dateconnect.Text;
            set => lbl_dateconnect.Text = value;
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


            allMessages = allMessages.OrderByDescending(msg => msg.SendDate).ToList();

            foreach (var message in allMessages)
            {
                if (message.SenderId == user.Id && message.ReceiverId == Id)
                {
                    MessageSenderUC msgSender = new MessageSenderUC();
                    msgSender.Message = message.Content;
                    msgSender.DateTimeMessage = message.SendDate.ToString();
                    msgSender.Dock = DockStyle.Right;
                    msgSender.Image_user = MyUtility.GetImageFromByte(Login.user.Photo);
                    Login.f1.Messages_flowLayoutPanel2.Controls.Add(msgSender);
                }
                else if (message.SenderId == Id && message.ReceiverId == user.Id)
                {
                    MessageReceverUC msgReceiver = new MessageReceverUC();
                    msgReceiver.Message = message.Content;
                    msgReceiver.DateTimeMessage = message.SendDate.ToString();
                    msgReceiver.Dock = DockStyle.Left;
                    msgReceiver.Image_user = Image;
                    Login.f1.Messages_flowLayoutPanel2.Controls.Add(msgReceiver);
                }


            }


        }




    }
}
