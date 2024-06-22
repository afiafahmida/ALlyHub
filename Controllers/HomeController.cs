using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALlyHub.Data;
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
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (DatabaseHelper.RegisterUser(model.FirstName, model.LastName, model.Email, model.Password, model.Address, model.Phone))
                {
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("", "Registration failed");
                }
            }
            return View(model);
        }

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult Task()
        {
            return View();
        }
        public ActionResult Team()
        {
            return View();
        }

    }
}