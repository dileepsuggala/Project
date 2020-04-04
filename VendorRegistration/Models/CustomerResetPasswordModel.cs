using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VendorRegistration.Models
{
    public class CustomerResetPasswordModel
    {
        [Required(ErrorMessage = "New Password required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New password and confirm Password doesn't match")]

        public string ConfirmPassword { get; set; }
        [Required]
        public string ResetCode { get; set; }

    }
}