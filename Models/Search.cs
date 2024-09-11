using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ALlyHub.Models
{
    public class Search
    {
        public List<Search> Developers { get; set; }
        public List<Search> Users { get; set; }
        public List<Search> Clients { get; set; }
        public List<Project> Projects { get; set; }

        public string queryText { get; set; }






        List<Search> userids {  get; set; }
        List<Search> devids { get; set; }
        List<Search> clientids { get; set; }






        public int DeveloperID { get; set; }
        public int ClientID { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserAddress { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string UserType { get; set; }
        public string DevDescription { get; set; }
        public string AreaofExpertise { get; set; }
        public string PortfolioLink { get; set; }
        public string LinkedIn { get; set; }
        public string Facebook { get; set; }
        public string Country { get; set; }
        public string Languagee { get; set; }
        public string DOB { get; set; }
        public string UserPhoto { get; set; }





  
        public string CompanyName { get; set; }
        public string ClientDescription { get; set; }
        





 
        
            [Key]
            public int ProjectID { get; set; }
            public string ProjectTitle { get; set; }
            public string ShortDescription { get; set; }
            public string Description { get; set; }
            public int PaymentAmount { get; set; }
        
            public string ExpertiseLevel { get; set; }
            public string Duration { get; set; }
            public string SkillSet { get; set; }
            public DateTime PostedOn { get; set; }
      
            public string ClientName { get; set; }
            public string ClientLocation { get; set; }
        
            public string JobNature { get; set; }



            public int ApplicantID { get; set; }
            
            public string ApplicantsName { get; set; }
            public string ApplicantsEmail { get; set; }
            public string ApplicantsWebsite { get; set; }
            public string ApplicantsFile { get; set; }
            public string ApplicantsCoverLetter { get; set; }
            public virtual ICollection<Project> Applicants { get; set; }



            public DateTime HandshakeDate { get; set; }
            public string Status { get; set; }
        }
    }


