using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALlyHub.Models
{
    public class Handshake
    {
        public int HandshakeID { get; set; }
        public int ProjectID { get; set; }
        public int DeveloperID { get; set; }
        public int ClientID { get; set; }
        public DateTime HandshakeDate { get; set; }
        public string Status { get; set; }
        public string Duration { get; set; }
    }

}