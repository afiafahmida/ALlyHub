using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ALlyHub.Models
{

    [Table("Project")]
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }
        public string ProjectTitle { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int PaymentAmount { get; set; }
        public int ClientID { get; set; }
        public string ExpertiseLevel { get; set; }
        public string Duration { get; set; }
        public string SkillSet { get; set; }
        public DateTime PostedOn { get; set; }
        public string CompanyName { get; set; }
        public string ClientName { get; set; }
        public string ClientLocation { get; set; }
        public string ClientDescription { get; set; }
        public string JobNature { get; set; }



        public int ApplicantID { get; set; }
        public int DeveloperID { get; set; }
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
