//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VendorRegistration.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vendor
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsEmailVerified { get; set; }
        public System.Guid ActivationCode { get; set; }
        public string ResestPasswordCode { get; set; }
    }
}
