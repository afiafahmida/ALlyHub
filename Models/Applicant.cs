using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ALlyHub.Models
{
    public class Applicant
    {
        [Key]
        public int ApplicantID { get; set; }
        public int ProjectID { get; set; }
        public int DeveloperID { get; set; }
        public string ApplicantsName { get; set; }
        public string ApplicantsEmail { get; set; }
        public string ApplicantsWebsite { get; set; }
        public string ApplicantsFile { get; set; }
        public string ApplicantsCoverLetter { get; set; }

        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }
    }
}