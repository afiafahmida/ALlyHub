using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ALlyHub.Models
{
    public class FindTalentModel
    {
        [Key]
        public int DeveloperID { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserAddress { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string DevDescription { get; set; }
        public string AreaofExpertise { get; set; }
        public string PortfolioLink { get; set; }
        public string LinkedIn {  get; set; }
        public string Facebook {  get; set; }
        public string Country {  get; set; }
        public string Languagee { get; set; }
        public string DOB { get; set; }
        public string UserPhoto { get; set; }


        //For Fetching Working Projects
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int PaymentAmount { get; set; }
        public string ExpertiseLevel { get; set; }
        public string SkillSet { get; set; }
        public DateTime PostedOn { get; set; }
        public string ClientName { get; set; }
        public string ClientLocation { get; set; }

        // Work Experience
        //Input Fields
        public string CompanyInput { get; set; }
        public string PositionInput { get; set; }
        public string StartDateInput { get; set; }
        public string EndDateInput { get; set; }
        public string JobDescriptionInput { get; set; }
        //Output Fields
        public string Company { get; set; }
        public string Position { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string JobDescription { get; set; }

        public List<FindTalentModel> Experiences { get; set; }

        //Reviews
        public int ReviewID { get; set; }
        public string ReviewText { get; set; }
        public int GivenBy { get; set; }
        public int GivenTo { get; set; }
        public string ReviewDate { get; set; }
        public string ReviewerFName { get; set; }
        public string ReviewerLName { get; set; }
        public List<FindTalentModel> Reviews { get; set; }



        public List<FindTalentModel> Projects { get; set; }






        

       


        
        public int ClientID { get; set; }
        public int ProjectID { get; set; }
        public DateTime HandshakeDate { get; set; }
        public string Status { get; set; }
        public string ProjectTitle { get; set; }
        public string Duration { get; set; }


        //OTP for Email Sending
        public string UserOtp { get; set; }
        public string UserPassword { get; set; }

        //For Fetching Working Projects
    


            public string CompanyName { get; set; }
            public string ClientDescription { get; set; }





    }
}




