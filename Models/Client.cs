using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ALlyHub.Models
{
    public class Client
    {
        public int ClientID { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(50)]
        public string CompanyName { get; set; }

        [StringLength(100)]
        public string ClientLocation { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

    }
}