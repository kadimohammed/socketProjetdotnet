namespace Socket_Projet_Client
{
    partial class ContactUC
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lbl_notification = new Guna.UI2.WinForms.Guna2CircleButton();
            lbl_dateconnect = new Label();
            lbl_msg = new Label();
            lbl_name = new Label();
            image = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            notifText = new Guna.UI2.WinForms.Guna2HtmlLabel();
            ((System.ComponentModel.ISupportInitialize)image).BeginInit();
            SuspendLayout();
            // 
            // lbl_notification
            // 
            lbl_notification.BackColor = Color.Transparent;
            lbl_notification.DisabledState.BorderColor = Color.DarkGray;
            lbl_notification.DisabledState.CustomBorderColor = Color.DarkGray;
            lbl_notification.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            lbl_notification.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            lbl_notification.FillColor = Color.Fuchsia;
            lbl_notification.FocusedColor = Color.Fuchsia;
            lbl_notification.Font = new Font("Segoe UI", 3.75F, FontStyle.Bold, GraphicsUnit.Point);
            lbl_notification.ForeColor = Color.Black;
            lbl_notification.ImageSize = new Size(0, 0);
            lbl_notification.Location = new Point(223, 31);
            lbl_notification.Margin = new Padding(0);
            lbl_notification.Name = "lbl_notification";
            lbl_notification.ShadowDecoration.CustomizableEdges = customizableEdges1;
            lbl_notification.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            lbl_notification.Size = new Size(20, 20);
            lbl_notification.TabIndex = 0;
            // 
            // lbl_dateconnect
            // 
            lbl_dateconnect.AutoSize = true;
            lbl_dateconnect.BackColor = Color.Transparent;
            lbl_dateconnect.Font = new Font("Franklin Gothic Medium Cond", 7.2F, FontStyle.Regular, GraphicsUnit.Point);
            lbl_dateconnect.ForeColor = Color.Silver;
            lbl_dateconnect.Location = new Point(189, 8);
            lbl_dateconnect.Name = "lbl_dateconnect";
            lbl_dateconnect.Size = new Size(57, 14);
            lbl_dateconnect.TabIndex = 21;
            lbl_dateconnect.Text = "16/02/2024";
            // 
            // lbl_msg
            // 
            lbl_msg.AutoSize = true;
            lbl_msg.BackColor = Color.Transparent;
            lbl_msg.Font = new Font("Franklin Gothic Medium Cond", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lbl_msg.ForeColor = Color.Gray;
            lbl_msg.Location = new Point(67, 26);
            lbl_msg.Name = "lbl_msg";
            lbl_msg.Size = new Size(25, 16);
            lbl_msg.TabIndex = 20;
            lbl_msg.Text = "non";
            // 
            // lbl_name
            // 
            lbl_name.AutoSize = true;
            lbl_name.BackColor = Color.Transparent;
            lbl_name.Font = new Font("Franklin Gothic Medium Cond", 7.8F, FontStyle.Regular, GraphicsUnit.Point);
            lbl_name.ForeColor = Color.Silver;
            lbl_name.Location = new Point(60, 8);
            lbl_name.Name = "lbl_name";
            lbl_name.Size = new Size(51, 15);
            lbl_name.TabIndex = 19;
            lbl_name.Text = "Jane Lopez";
            // 
            // image
            // 
            image.BackColor = Color.Transparent;
            image.ImageFlip = Guna.UI2.WinForms.Enums.FlipOrientation.Horizontal;
            image.ImageRotate = 0F;
            image.Location = new Point(10, 9);
            image.Margin = new Padding(3, 2, 3, 2);
            image.Name = "image";
            image.ShadowDecoration.Color = Color.Fuchsia;
            image.ShadowDecoration.CustomizableEdges = customizableEdges2;
            image.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            image.Size = new Size(42, 42);
            image.SizeMode = PictureBoxSizeMode.StretchImage;
            image.TabIndex = 22;
            image.TabStop = false;
            // 
            // notifText
            // 
            notifText.BackColor = Color.Fuchsia;
            notifText.Font = new Font("Segoe UI", 6F, FontStyle.Regular, GraphicsUnit.Point);
            notifText.ForeColor = Color.Transparent;
            notifText.Location = new Point(230, 34);
            notifText.Name = "notifText";
            notifText.Size = new Size(7, 13);
            notifText.TabIndex = 23;
            notifText.Text = "0";
            // 
            // ContactUC
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(26, 32, 47);
            Controls.Add(notifText);
            Controls.Add(image);
            Controls.Add(lbl_notification);
            Controls.Add(lbl_dateconnect);
            Controls.Add(lbl_msg);
            Controls.Add(lbl_name);
            Margin = new Padding(3, 2, 3, 2);
            Name = "ContactUC";
            Size = new Size(258, 59);
            Click += ContactUC_Click;
            MouseEnter += ContactUC_MouseEnter;
            MouseLeave += ContactUC_MouseLeave;
            ((System.ComponentModel.ISupportInitialize)image).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2CircleButton lbl_notification;
        private Label lbl_dateconnect;
        private Label lbl_msg;
        private Label lbl_name;
        private Guna.UI2.WinForms.Guna2CirclePictureBox image;
        private Guna.UI2.WinForms.Guna2HtmlLabel notifText;
    }
}
