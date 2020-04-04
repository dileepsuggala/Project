using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VendorRegistration.Models;

namespace VendorRegistration.Controllers
{
    public class VendorController : Controller
    {
        // Registration action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [Authorize]
        public ActionResult VOrder()
        {
            ProjectDBEntities db = new ProjectDBEntities();
            var order = db.Orders;
            return View(order.ToList());
        }
        [Authorize]
      public ActionResult VendorGrid()
        {
            vendorEntities dc = new vendorEntities();
            var data = dc.Vendors;
            return View(data.ToList());
        }
      

        //Registration Post action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")] Vendor vendor)
        {

            bool Status = false;
            string message = "";
            //
            // Model Validation 
            if (ModelState.IsValid)
            {

                #region //Email is already Exist 
                var isExist = IsEmailExist(vendor.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(vendor);
                }
                #endregion

                #region Generate Activation Code 
                vendor.ActivationCode = Guid.NewGuid();
                #endregion

                #region  Password Hashing 
                vendor.Password = Crypto.Hash(vendor.Password);
                vendor.ConfirmPassword = Crypto.Hash(vendor.ConfirmPassword); //
                #endregion
                vendor.IsEmailVerified = false;

                #region Save to Database
                using (vendorEntities dc = new vendorEntities())
                {
                    dc.Vendors.Add(vendor);
                    dc.SaveChanges();

                    //Send Email to User
                    SendVerificationLinkEmail(vendor.Email, vendor.ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link " +
                        " has been sent to your email id:" + vendor.Email;
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
         
            return View(vendor);
        }

        //Verify Email

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool status = false;
            using(vendorEntities dc = new vendorEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; //this line is to avoid 
                                                                //confirm password doesnotmatch issue on savechanges
                var v = dc.Vendors.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if(v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    status = true;
                }

                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = status;
            return View();
        }

        //login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //login Post
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(VendorLogin login,string ReturnUrl)
        {
            string message = "";
            using (vendorEntities dc = new vendorEntities())
            {
                var v = dc.Vendors.Where(a => a.Email == login.Email).FirstOrDefault();
                if (v != null)
                {
                    if(string.Compare(Crypto.Hash(login.Password),v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 1; //525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if(Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                       
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        //logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Vendor");
        }

        
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using(vendorEntities dc = new vendorEntities())
            {
                var v = dc.Vendors.Where(a => a.Email == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode,string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Vendor/"+emailFor+"/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("dileeprockon007@gmail.com", "News Paper");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "@2317padma"; // Replace with actual password

            string subject = "";
            string body = "";

            if(emailFor == "VerifyAccount" )
            {
                 subject = "Your account is successfully created!";

                 body = "<br/><br/>We are excited to tell you that your vendor account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if(emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/>br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
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

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        //forgot Password

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";
            bool status = false;

            using (vendorEntities dc = new vendorEntities())
            {
                var account = dc.Vendors.Where(a => a.Email == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResestPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    //in our model class in part 1
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    message = "Reset password link has been sent to your email id.";
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

            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            //if (string.IsNullOrWhiteSpace(id))
            //{
            //    return HttpNotFound();
            //}

            using (vendorEntities dc = new vendorEntities())
            {
                var user = dc.Vendors.Where(a => a.ResestPasswordCode == id).FirstOrDefault();
                if (user != null)
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
            var message = "";
            if(ModelState.IsValid)
            {
                using(vendorEntities dc = new vendorEntities())
                {
                    var user = dc.Vendors.Where(a => a.ResestPasswordCode == model.ResetCode).FirstOrDefault();
                    if(user !=null)
                    {
                        user.Password = Crypto.Hash(model.NewPassword);
                        user.ResestPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New Password updated successfully";
                    }
                }

            }
            else
            {
                message = "something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }
    }

}