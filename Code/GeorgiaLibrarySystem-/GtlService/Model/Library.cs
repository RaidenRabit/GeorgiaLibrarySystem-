//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GtlService.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Library
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Library()
        {
            this.Copies = new HashSet<Copy>();
        }
    
        public string LibraryName { get; set; }
        public Nullable<int> AddressID { get; set; }
    
        public virtual Address Address { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Copy> Copies { get; set; }
    }
}