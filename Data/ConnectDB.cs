using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALlyHub.Data
{
    public class ConnectDB
    {
        public static string connect { get; } = "Data Source=ASHIK\\SQLEXPRESS;Initial Catalog=Allyhub;Integrated Security=True";

        public ConnectDB() { }
    }
}