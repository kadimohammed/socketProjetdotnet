using Socket_Projet_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Projet_Client.Outiles
{
    public class MyUtility
    {
        public static Image? GetImageFromByte(byte[] photoBytes)
        {

            if (photoBytes != null && photoBytes.Length > 0)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(photoBytes))
                    {
                        return Image.FromStream(ms);
                    }
                }
                catch (ArgumentException ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
