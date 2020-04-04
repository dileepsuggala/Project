using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VendorRegistration.Models
{
    public class VendorLogin
    {
        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        public string Email { get; set; }



        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }

    }
}