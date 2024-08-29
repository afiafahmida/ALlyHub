using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALlyHub.Data
{
    public class ConnectDB
    {
        public static string connect { get; set; } = "Data Source=DESKTOP-O5HM1C3\\SQLEXPRESS;Initial Catalog=Allyhub;Integrated Security=True";

        public ConnectDB() { }
    }
}