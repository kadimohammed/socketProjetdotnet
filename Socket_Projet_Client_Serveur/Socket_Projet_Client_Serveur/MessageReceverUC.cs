using Guna.UI2.WinForms;
using System.Text;

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


                    BackMessage_guna2GradientPanel6.Size = new Size(messageWidth, 50);
                    Message_label.Size = new Size(messageWidth, lineHeight);
                    Message_label.Text = value;
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
                    BackMessage_guna2GradientPanel6.Height = totalHeight;
                    Message_label.Width = (maxCharactersPerLine * characterWidth);

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

                    Message_label.Text = formattedMessage.ToString();

                }

                this.Height = BackMessage_guna2GradientPanel6.Height + 40;
                int newYPositionHight = BackMessage_guna2GradientPanel6.Top + BackMessage_guna2GradientPanel6.Height + 5;
                int newYPositionwidth = BackMessage_guna2GradientPanel6.Width - 60;
                DateTime_label.Top = newYPositionHight;
                DateTime_label.Left = newYPositionwidth;


                int labelX = (BackMessage_guna2GradientPanel6.Width - Message_label.Width) / 2;
                int labelY = (BackMessage_guna2GradientPanel6.Height - Message_label.Height) / 2;
                Message_label.Location = new Point(labelX, labelY);
            }
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
