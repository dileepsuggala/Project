using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendorRegistration.Models
{
    public class Cart
    {
        public int PaperId { get; set; }
        public string PaperName { get; set; }
        public decimal price { get; set; }
        public int qty { get; set; }
        public decimal bill { get; set; }
    }
}