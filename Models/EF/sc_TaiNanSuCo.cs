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
    
    public partial class sc_TaiNanSuCo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sc_TaiNanSuCo()
        {
            this.sc_SuCoPTD = new HashSet<sc_SuCoPTD>();
            this.sc_TaiLieu = new HashSet<sc_TaiLieu>();
            this.sc_TaiNanSuCo_DonVi = new HashSet<sc_TaiNanSuCo_DonVi>();
        }
    
        public int Id { get; set; }
        public string DonViId { get; set; }
        public string CapDienAp { get; set; }
        public string TenThietBi { get; set; }
        public string DienBienSuCo { get; set; }
        public string TomTat { get; set; }
        public Nullable<bool> TinhTrangBienBan { get; set; }
        public Nullable<bool> HinhAnhSuCo { get; set; }
        public Nullable<System.DateTime> ThoiGianXuatHien { get; set; }
        public Nullable<System.DateTime> ThoiGianBatDauKhacPhuc { get; set; }
        public Nullable<System.DateTime> ThoiGianKhacPhucXong { get; set; }
        public Nullable<System.DateTime> ThoiGianKhoiPhuc { get; set; }
        public Nullable<double> T_XuatHienBatDauKhacPhuc { get; set; }
        public Nullable<double> T_BatDauDenKhacPhucXong { get; set; }
        public Nullable<double> T_KhacPhucXongDenKhoiPhuc { get; set; }
        public Nullable<double> T_TongThoiGianMatDien { get; set; }
        public Nullable<bool> IsGianDoan { get; set; }
        public Nullable<int> PhieuCongTacId { get; set; }
        public Nullable<int> LoaiSuCoId { get; set; }
        public Nullable<int> NguyenNhanId { get; set; }
        public Nullable<int> TinhChatId { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<int> TrangThai { get; set; }
        public Nullable<System.DateTime> NgayDuyet { get; set; }
        public string NguoiDuyet { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public Nullable<decimal> KinhDo { get; set; }
        public Nullable<decimal> ViDo { get; set; }
        public Nullable<bool> IsTaiSan { get; set; }
        public Nullable<bool> IsChuyenNPC { get; set; }
        public Nullable<System.DateTime> NgayDuyetNPC { get; set; }
        public string NguoiDuyetNPC { get; set; }
        public Nullable<bool> IsMienTru { get; set; }
        public Nullable<bool> NPCIsDuyetMT { get; set; }
        public Nullable<System.DateTime> NPCNgayDuyetMT { get; set; }
        public string NPCNguoiDuyetMT { get; set; }
        public string NPCTenNguoiDuyetMT { get; set; }
        public string NPCCommentMT { get; set; }
        public Nullable<int> KienNghiId { get; set; }
        public Nullable<bool> IsDuyetKienNghi { get; set; }
    
        public virtual sc_LoaiSuCo sc_LoaiSuCo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sc_SuCoPTD> sc_SuCoPTD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sc_TaiLieu> sc_TaiLieu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sc_TaiNanSuCo_DonVi> sc_TaiNanSuCo_DonVi { get; set; }
    }
}
