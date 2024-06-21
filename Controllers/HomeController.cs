using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALlyHub.Models;


namespace ALlyHub.Controllers
{
    public class HomeController : Controller
    {
        AllyhubEntities db = new AllyhubEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult FindTalent()
        {             
            ViewBag.Message = "Find Talent";
        
            return View();
        }
        public ActionResult FindJobs()
        {
            ViewBag.Message = "Find Jobs";
            return View();
        }
        public ActionResult Login()
        {
            
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(ProgramUser programUser)
        {
            if (db.ProgramUsers.Any(x => x.UserEmail == programUser.UserEmail))
            {
                ViewBag.Notification = "This Email is already Taken";
                return View();  
            }
            else
            {   
                db.ProgramUsers.Add(programUser); 
                db.SaveChanges();

                Session["IDUsSS"] =programUser.UserId.ToString();
                Session["UsernameSS"]=programUser.UserName.ToString();

                return RedirectToAction("Index","Home");

            }
           
        }

    }
}