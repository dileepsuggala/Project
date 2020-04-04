using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace VendorRegistration.Models.Extended
{

    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {
        

    }
    public class CustomerMetadata
    {
        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name required")]
        public string CustomerName { get; set; }


        [Display(Name = "Contact")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contact required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }


        [Display(Name = "Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address required")]

        public string Address { get; set; }

        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email ID required")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum of 6 characters required")]
        public string Password { get; set; }


        [Display(Name = "Confirm Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match")]

        public string ConfirmPassword { get; set; }


    }
}