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
    
    public partial class vs_KyBaoCao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public vs_KyBaoCao()
        {
            this.vs_NoiDungVeSinh = new HashSet<vs_NoiDungVeSinh>();
        }
    
        public int Id { get; set; }
        public string DonViId { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public Nullable<bool> IsChuyenNPC { get; set; }
        public string NguoiChuyenNPC { get; set; }
        public Nullable<System.DateTime> NgayChuyenNPC { get; set; }
        public Nullable<bool> TrangThaiChot { get; set; }
        public string NguoiChot { get; set; }
        public Nullable<System.DateTime> NgayChot { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<vs_NoiDungVeSinh> vs_NoiDungVeSinh { get; set; }
    }
}