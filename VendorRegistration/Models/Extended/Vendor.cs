using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VendorRegistration.Models
{
    [MetadataType(typeof(VendorMetadata))]
    public partial class Vendor
    {

    }
    
    public class VendorMetadata
    {
        [Display(Name = "Vendor Name")]
        [Required(AllowEmptyStrings = false,ErrorMessage = "This field is required")]
        public string VendorName { get; set; }

        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="minimum 6 characters are required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Password does not match")]
        [MinLength(6, ErrorMessage = "minimum 6 characters are required")]
        public string ConfirmPassword { get; set; }

    }
}