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
    
    public partial class tblPhienLamViec_His
    {
        public int Id { get; set; }
        public int PhongBanID { get; set; }
        public string NoiDung { get; set; }
        public string DiaDiem { get; set; }
        public System.DateTime NgayLamViec { get; set; }
        public System.DateTime GioBd { get; set; }
        public System.DateTime GioKt { get; set; }
        public string NguoiDuyet_SoPa { get; set; }
        public string NguoiChiHuy { get; set; }
        public string GiamSatVien { get; set; }
        public string NguoiKiemSoat { get; set; }
        public string NguoiKiemTraPhieu { get; set; }
        public string LanhDaoTrucBan { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public Nullable<int> TT_Phien { get; set; }
        public Nullable<int> TrangThai { get; set; }
        public string NguoiDuyet { get; set; }
        public Nullable<System.DateTime> NgayDuyet { get; set; }
        public string LyDoThayDoi { get; set; }
        public Nullable<int> MaPCT { get; set; }
        public string NguoiDuyet_SoPa_Id { get; set; }
        public string NguoiChiHuy_Id { get; set; }
        public string GiamSatVien_Id { get; set; }
        public string NguoiKiemSoat_Id { get; set; }
        public string NguoiKiemTraPhieu_Id { get; set; }
        public string LanhDaoTrucBan_Id { get; set; }
        public string DonViId { get; set; }
        public Nullable<bool> IsChuyenNPC { get; set; }
        public Nullable<System.DateTime> NgayDuyetNPC { get; set; }
        public string NguoiDuyetNPC { get; set; }
        public Nullable<System.DateTime> NgayKetThuc { get; set; }
        public Nullable<long> PhienLamViecId { get; set; }
        public Nullable<int> PhongBanIDCreate { get; set; }
    }
}
