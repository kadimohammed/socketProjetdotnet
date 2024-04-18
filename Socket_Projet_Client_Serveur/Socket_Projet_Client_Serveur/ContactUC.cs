using SocketsProject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Socket_Projet_Client
{
    public partial class ContactUC : UserControl
    {
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

        private void ContactUC_Load(object sender, EventArgs e)
        {

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

        private void ContactUC_Click(object sender, EventArgs e)
        {
            foreach (ContactUC uc in Form1.contactList)
            {
                uc.BackColor = Color.FromArgb(26, 32, 47);
                uc.ForeColor = SystemColors.ControlText;
                uc.handleMouseLeave = true;
            }
            BackColor = Color.FromArgb(17, 22, 32);
            ForeColor = SystemColors.ControlText;
            handleMouseLeave = false;
            Form1.label16.Text = this.Name;
        }

    }
}
