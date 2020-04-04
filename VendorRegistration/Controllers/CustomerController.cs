using System;
using System.Collections.Generic;
using System.Linq;

using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VendorRegistration;
using VendorRegistration.Models;

namespace Project.Controllers
{
    public class CustomerController : Controller
    {
        ProjectDBEntities db = new ProjectDBEntities();
        [Authorize]
        public ActionResult Index()
        {
            decimal x = 0;
            Session["CustomerId"] = 1;
           if(TempData["cart"] != null)
            {
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                foreach(var item in li2)
                {
                    x += item.bill;
                }
                TempData["total"] = x;
            }
            TempData.Keep();
            var query = from c in db.Newspapers
                            select c;

                return View(query.OrderByDescending(a=>a.NewspaperId).ToList());

            
            
        }
      
        public ActionResult Order()
        {


            var order = db.Orders;
            return View(order.ToList());
        }
        public ActionResult Addtocart(int ?Id)
        {
            Newspaper newspaper = db.Newspapers.Where(a => a.NewspaperId == Id).SingleOrDefault();
            
            return View(newspaper);
        }
        List<Cart> li = new List<Cart>();

        [HttpPost]
        public ActionResult Addtocart(Newspaper n, string qty, int Id)
        {
            Newspaper newspaper = db.Newspapers.Where(a => a.NewspaperId == Id).SingleOrDefault();
            Cart c = new Cart();
            c.PaperId = newspaper.NewspaperId;
            c.PaperName = newspaper.NewspaperName;
            c.price = Convert.ToDecimal(newspaper.Price);
            c.qty = Convert.ToInt32(qty);
            c.bill = c.price *c.qty;
            if(TempData["cart"]==null)
            {
                li.Add(c);
                TempData["cart"] = li;
            }
            else
            {
                List<Cart>li2 = TempData["cart"] as List<Cart>;
                li2.Add(c);
                TempData["cart"] = li2;
            }
            TempData.Keep();
            return RedirectToAction("Index","Customer");
        }

        public ActionResult Remove(int ?Id)
        {
            List<Cart> li2 = TempData["cart"] as List<Cart>;
            Cart c = li2.Where(x => x.PaperId == Id).SingleOrDefault();
            li2.Remove(c);
            TempData["cart"] = li2;
            decimal h = 0;
            foreach(var item in li2)
            {
                h += item.bill;
            }
            TempData["total"] = h;
            TempData.Keep();
            return RedirectToAction("Index", "Customer");
        }
        [Authorize]
        public ActionResult Total()
        {
            TempData.Keep();

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Total(Order order)
        {
            List<Cart> li = TempData["cart"] as List<Cart>;
            tbl_invoice iv = new tbl_invoice();
            iv.CustomerId = Convert.ToInt32(Session["CustomerId"].ToString());
            iv.InvoiceDate = System.DateTime.Now;
            iv.Invoice_TotalBill =Convert.ToDecimal( TempData["total"]);
            db.tbl_invoice.Add(iv);
            db.SaveChanges();
            foreach (var item in li)
            {
                Order o = new Order();
                o.NewspaperId = item.PaperId;
               // o.Newspaper.NewspaperName = item.PaperName;
                o.InvoiceId = iv.InvoiceId;
                o.OrderDate = System.DateTime.Now;
                o.Order_Unitprice = Convert.ToInt32(item.price);
                o.OrderQty = item.qty;
                o.OrderBill = decimal.ToDouble(item.bill);
                db.Orders.Add(o);
                db.SaveChanges();

            }
            TempData.Remove("total");
            TempData.Remove("cart");
            TempData["message"] = "Transaction completed";
            TempData.Keep();
            return RedirectToAction("Index", "Payment");
        }



        //Registration Action


        
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registration Post Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")]Customer customer)
        {

            bool Status = false;
            string Message = "";
            //Model Validation
            if (ModelState.IsValid)
            {
                //Email already exist 

                var isExist = IsEmailExist(customer.EmailId);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(customer);
                }
                //Generate Activation Code
                customer.ActivationCode = Guid.NewGuid();
                //Password Hashing
                customer.Password = Crypto.Hash(customer.Password);
                customer.ConfirmPassword = Crypto.Hash(customer.ConfirmPassword);
                customer.IsEmailVerified = false;

                //Save data to Database

                using (ProjectDBEntities db = new ProjectDBEntities())
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    //Send email to user
                    SendVerificationLinkEmail(customer.EmailId, customer.ActivationCode.ToString());
                    Message = "Registration Successfully done.Account activation link has been sent to your email id :" + customer.EmailId;
                    Status = true;
                }

            }
            else
            {
                Message = "Invalid Request";
            }
            ViewBag.Message = Message;
            ViewBag.Status = Status;

            return View(customer);
        }
       

        //Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (ProjectDBEntities db = new ProjectDBEntities())
            {
                db.Configuration.ValidateOnSaveEnabled = false;//This is to avoid confirm password doesnot match issue on save changes
                var v = db.Customers.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }
        //Login

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //Login Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(CustomerLogin login, string ReturnUrl)
        {
            string Message = "";
            using (ProjectDBEntities db = new ProjectDBEntities())
            {
                var v = db.Customers.Where(a => a.EmailId == login.EmailId).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20;//525600 min=1 year
                        var ticket = new FormsAuthenticationTicket(login.EmailId, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Customer");
                        }
                    }
                    else
                    {
                        Message = "Invalid credential provided";
                    }
                }
                else
                {
                   ViewBag.Message = "Invalid credential provided";
                }
            }
            ViewBag.Message = Message;
            return View();

        }


        //Logout
        // [Authorize]
        [HttpGet]
        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Login", "Customer");
        }
        

        [NonAction]
        public bool IsEmailExist(string EmailId)
        {
            using (ProjectDBEntities db = new ProjectDBEntities())
            {
                var v = db.Customers.Where(a => a.EmailId == EmailId).FirstOrDefault();
                return v != null;
            }
        }
        [NonAction]
        public void SendVerificationLinkEmail(string EmailId, string activationCode, string EmailFor = "VerifyAccount")
        {
            var verifyUrl = "/Customer/" + EmailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);


            var fromEmail = new MailAddress("divz368@gmail.com", "Customer Verification");
            var toEmail = new MailAddress(EmailId);
            var fromEmailPassword = "takeitallwithu";
            string subject = "";
            string body = "";

            if (EmailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created";
                body = "<br /><br /> We are excited to tell you that your account is" +
                   "Successfully created.Please click on the below link to verify your account" +
                   "<br/><br/><a href ='" + link + "'>" + link + "</a>";

            }
            else if (EmailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi, <br/><br/>We got request for reset your password.Please  click on the below link to reset your password" +
                    "<br/><br/><a href='" + link + "'>" + link + "</a>";

            }



            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

            };
            using (var Message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(Message);
        }
        //Forgot password
        public ActionResult ForgotPassword()
        {
            return View();


        }
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //verify EmailId
            //Generate Reset password link
            //Send Email
            string message = "";
            bool status = false;
            using (ProjectDBEntities db = new ProjectDBEntities())
            {
                var account = db.Customers.Where(a => a.EmailId == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.EmailId, resetCode, "ResetPassword");
                   account.ResetPasswordCode  = resetCode;
                    //this is added here to avoid confirm password not match issue, as we had added a confirm password property
                    //in model class
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    message = "Reset password link has been sent to your Email id";



                }
                else
                {
                    message = "Account not found";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            //verify the link
            //find account accosiated with link
            //redirect to reset password page
            using (ProjectDBEntities db = new ProjectDBEntities())
            {
                var cust = db.Customers.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (cust != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }

            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var Message = "";
            if (ModelState.IsValid)
            {
                using (ProjectDBEntities db = new ProjectDBEntities())
                {
                    var cust = db.Customers.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (cust != null)
                    {
                        cust.Password = Crypto.Hash(model.NewPassword);
                        cust.ResetPasswordCode = "";// to avoid using same code for multiple time
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        Message = "New Password updated Successfully";

                    }

                }
            }
            else
            {
                Message = "Something Invalid";
            }

            ViewBag.Message = Message;
            return View(model);
        }


    }


}
