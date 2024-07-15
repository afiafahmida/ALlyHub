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
        public string PostedOn { get; set; }
        public string ClientName { get; set; }
        public string CompanyName { get; set; }
        public string ClientLocation { get; set; }
    }
}
