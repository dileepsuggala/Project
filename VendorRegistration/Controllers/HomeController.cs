using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendorRegistration.Models;

namespace VendorRegistration.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(Feedback feedback)
        {
            TempData["msg"] = null;
            try
            {
                using (ProjectDBEntities db = new ProjectDBEntities())
                {
                    db.Feedbacks.Add(feedback);
                    db.SaveChanges();


                    return RedirectToAction("ContactUs");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Errorpage");

            }

        }

        public ActionResult ErrorPage()
        {
            return View();
        }

    }
}
