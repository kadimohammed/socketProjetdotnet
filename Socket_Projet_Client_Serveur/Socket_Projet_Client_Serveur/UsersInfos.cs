using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void DeconnexionButton_Click(object sender, EventArgs e)
        {
            /*
            this.Close();
            Login.f1.Close();
            Login.user = null;
            Login l = new Login();
            l.Show();

            */
        }

        private void EditNameButton_Click(object sender, EventArgs e)
        {
            EditNameButton.Visible = false;
            guna2Button1.Visible = true;
            EditNameTextBox.Enabled = true;
            guna2Button1.Text = EditNameTextBox.Text.Length.ToString() + "/25";
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            EditNameButton.Visible = true;
            guna2Button1.Visible = false;
            EditNameTextBox.Enabled = false;
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            EditInfosButton.Visible = true;
            guna2Button2.Visible = false;
            EditInfosTextBox.Enabled = false;
        }
    }
}
