﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Projet_Server.Classes
{
    public class MessageEnvoyerCL
    {
        public string Content { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
