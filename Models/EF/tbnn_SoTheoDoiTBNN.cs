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
    
    public partial class tbnn_SoTheoDoiTBNN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbnn_SoTheoDoiTBNN()
        {
            this.tbnn_SoTheoDoiTBNN_TaiLieu = new HashSet<tbnn_SoTheoDoiTBNN_TaiLieu>();
        }
    
        public int ID { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public Nullable<int> MaTB { get; set; }
        public Nullable<int> LanKiemTra { get; set; }
        public Nullable<System.DateTime> NgayKiemTra { get; set; }
        public string NguoiKiemTra { get; set; }
        public string DonViKiemTra { get; set; }
        public string BienBanSo { get; set; }
        public Nullable<bool> KetQua { get; set; }
        public Nullable<System.DateTime> NgayKiemTraTiepTheo { get; set; }
        public string GhiChu { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbnn_SoTheoDoiTBNN_TaiLieu> tbnn_SoTheoDoiTBNN_TaiLieu { get; set; }
        public virtual tbnn_ThietBiNghiemNgat tbnn_ThietBiNghiemNgat { get; set; }
    }
}
