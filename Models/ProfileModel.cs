using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ALlyHub.Models
{
    public class ProfileModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserAddress { get; set; }
        public string UserPhoto { get; set; }
        public string DevDescription { get; set; }
        public string AreaofExpertise { get; set; }
        public string CompanyName { get; set; }
        public string ClientDescription { get; set; }
        public string PortfolioLink { get; set; }
        public string LinkedIn { get; set; }
        public string Facebook { get; set; }
        public string Country { get; set; }
        public string Languagee { get; set; }
        public string DOB { get; set; }

    }
}
