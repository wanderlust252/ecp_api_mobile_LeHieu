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
    
    public partial class vs_DanhMucLoaiVeSinh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public vs_DanhMucLoaiVeSinh()
        {
            this.vs_NoiDungVeSinh = new HashSet<vs_NoiDungVeSinh>();
        }
    
        public int Id { get; set; }
        public string Ten { get; set; }
        public Nullable<int> ThuTu { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string TenNoiDung { get; set; }
        public string LoaiDonViTinh { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
        public string NguoiSua { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<vs_NoiDungVeSinh> vs_NoiDungVeSinh { get; set; }
    }
}
