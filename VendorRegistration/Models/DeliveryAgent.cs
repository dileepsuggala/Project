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
    
    public partial class DeliveryAgent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryAgent()
        {
            this.AgentLocations = new HashSet<AgentLocation>();
        }
    
        public int Id { get; set; }
        public string AgentName { get; set; }
        public string Mobile { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AgentLocation> AgentLocations { get; set; }
    }
}
