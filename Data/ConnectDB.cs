using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALlyHub.Data
{
    public class ConnectDB
    {
        //Data Source=ASHIK\\SQLEXPRESS;Initial Catalog=Allyhub;Integrated Security=True
        //Data Source=SQL8020.site4now.net;Initial Catalog=db_aacb8e_allyhubdb;User Id=db_aacb8e_allyhubdb_admin;Password=allyhub123

        public static string connect { get; set; } = "Data Source=SQL8020.site4now.net;Initial Catalog=db_aacb8e_allyhubdb;User Id=db_aacb8e_allyhubdb_admin;Password=allyhub123";

        public ConnectDB() { }
    }
}