using Guna.UI2.WinForms;
using System.Text;

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
            set
            {
                int messageLength = value.Length;
                const int characterWidth = 10;
                 int maxCharactersPerLine = 30;
                const int lineHeight = 24;

                double numberOfLines = Math.Ceiling((double)messageLength / maxCharactersPerLine);

                if (numberOfLines <= 1)
                {
                    
                    int messageWidth = messageLength * characterWidth;
                    if (messageLength == 1)
                    {
                        messageLength = 2;
                    }
                    
                     messageWidth = (messageLength * 7) + 100;
                    


                    BackMessage_guna2GradientPanel7.Size = new Size(messageWidth, 50);
                    Messagelabel.Size = new Size(messageWidth, lineHeight);
                    Messagelabel.Text = value;
                    BackMessage_guna2GradientPanel7.Left = this.Width - BackMessage_guna2GradientPanel7.Width -16;

                    guna2Panel7.Location = new Point(Messagelabel.Location.X + messageWidth - 62, guna2Panel7.Location.Y);

                }
                else
                {
                    maxCharactersPerLine = 44;
                    numberOfLines = Math.Ceiling((double)messageLength / maxCharactersPerLine);
                    if (numberOfLines == 1)
                    {
                        numberOfLines = 2;
                    }
                    int totalHeight = (int)(numberOfLines * lineHeight);
                    BackMessage_guna2GradientPanel7.Height = totalHeight;
                    Messagelabel.Width = (maxCharactersPerLine * characterWidth);

                    StringBuilder formattedMessage = new StringBuilder();
                    int currentLineLength = 0;
                    StringBuilder currentWord = new StringBuilder();

                    foreach (char currentChar in value)
                    {
                        if (currentChar == ' ' || currentChar == '\n')
                        {
                            if (currentLineLength + currentWord.Length > maxCharactersPerLine)
                            {
                                formattedMessage.Append('\n');
                                currentLineLength = 0;
                            }

                            formattedMessage.Append(currentWord);
                            currentLineLength += currentWord.Length;

                            currentWord.Clear();

                            if (currentChar == '\n')
                            {
                                formattedMessage.Append('\n');
                                currentLineLength = 0;
                            }
                            else
                            {
                                formattedMessage.Append(currentChar);
                                currentLineLength++;
                            }
                        }
                        else
                        {
                            currentWord.Append(currentChar);
                        }
                    }

                    // Ajouter le dernier mot si nécessaire
                    if (currentWord.Length > 0)
                    {
                        if (currentLineLength + currentWord.Length > maxCharactersPerLine)
                        {
                            formattedMessage.Append('\n');
                        }
                        formattedMessage.Append(currentWord);
                    }

                    Messagelabel.Text = formattedMessage.ToString();

                }

                this.Height = BackMessage_guna2GradientPanel7.Height + 40;
                int newYPositionHight = BackMessage_guna2GradientPanel7.Top + BackMessage_guna2GradientPanel7.Height + 5;
                int newYPositionwidth = BackMessage_guna2GradientPanel7.Left + 5;
                DateTimelabel.Top = newYPositionHight;
                DateTimelabel.Left = newYPositionwidth;


                int labelX = (BackMessage_guna2GradientPanel7.Width - Messagelabel.Width) / 2;
                int labelY = (BackMessage_guna2GradientPanel7.Height - Messagelabel.Height) / 2;
                Messagelabel.Location = new Point(labelX, labelY);
            }
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
