//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ECP_WEBAPI.Models.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class plv_TrangThaiPhieu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public plv_TrangThaiPhieu()
        {
            this.plv_PhieuCongTac = new HashSet<plv_PhieuCongTac>();
        }
    
        public int MaTT { get; set; }
        public string TenTT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<plv_PhieuCongTac> plv_PhieuCongTac { get; set; }
    }
}