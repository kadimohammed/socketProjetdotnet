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
    public partial class MessageReceverUC : UserControl
    {
        public MessageReceverUC()
        {
            InitializeComponent();
        }

        public string Message
        {
            get => Message_label.Text;
            set => Message_label.Text = value;
        }

        public string DateTimeMessage
        {
            get => DateTime_label.Text;
            set => DateTime_label.Text = value;
        }

        public Image Image_user
        {
            get => Image_guna2CirclePictureBox7.Image;
            set => Image_guna2CirclePictureBox7.Image = value;
        }

    }
}
