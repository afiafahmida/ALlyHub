using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALlyHub.Data
{
    public class ConnectDB
    {

        //Use this String to connect
        //Data Source=DESKTOP-O5HM1C3\\SQLEXPRESS;Initial Catalog=Allyhub;Integrated Security=True
        //Data Source = SQL8020.site4now.net; Initial Catalog = db_aacb8e_allyhubdb; User Id = db_aacb8e_allyhubdb_admin; Password=allyhub123
        public static string connect { get; set; } = "Data Source = SQL8020.site4now.net; Initial Catalog = db_aacb8e_allyhubdb; User Id = db_aacb8e_allyhubdb_admin; Password=allyhub123";
        public ConnectDB() { }
    }
}