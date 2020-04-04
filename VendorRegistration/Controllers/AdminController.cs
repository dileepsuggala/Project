using VendorRegistration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace VendorRegistration.Controllers
{
    
    public class AdminController : Controller
    {

        // GET: Admin
      
        public ActionResult VendorGrid()
        {
            vendorEntities dc = new vendorEntities();
            var data = dc.Vendors;
            return View(data.ToList());
        }

      
        public ActionResult GridView()
        {

            ProjectDBEntities db = new ProjectDBEntities();
            var data = db.Customers;
            return View(data.ToList());
        }
        public ActionResult AfterLogin()
        {
            
            
              //  return RedirectToAction("Login", "Login","Admin");
            
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string name,string password)
        {
            
                if ("admin".Equals(name)&&"123".Equals(password))
            {
                  
                    Session["user"] = new AdminLogin() { Name = name };
                return RedirectToAction("AfterLogin", "Admin");
            }
            return View();
        }

        [Authorize]
        
        public ActionResult Logout()
        {
            
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Login", "Admin");

        }
        public ActionResult ePaper()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            using (ProjectDBEntities db = new ProjectDBEntities())
            {
                return View(db.Feedbacks.ToList().OrderByDescending(x=>x.FeedbackId));
            }
                
        }

    }
}

        