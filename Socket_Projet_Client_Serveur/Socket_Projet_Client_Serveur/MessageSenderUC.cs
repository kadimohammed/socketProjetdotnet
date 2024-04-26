using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Socket_Projet_Client
{
    public partial class MessageSenderUC : UserControl
    {
        public MessageSenderUC()
        {
            InitializeComponent();
        }

        
        public string Message
        {
            get => Messagelabel.Text;
            set => Messagelabel.Text = value;
        }

        public string DateTimeMessage
        {
            get => DateTimelabel.Text;
            set => DateTimelabel.Text = value;
        }


        
        public Image Image_user
        {
            get => Image_guna2CirclePictureBox10.Image;
            set => Image_guna2CirclePictureBox10.Image = value;
        }

    }
}
