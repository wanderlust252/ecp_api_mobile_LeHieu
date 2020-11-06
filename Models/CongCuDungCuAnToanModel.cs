using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECP_WEBAPI.Models
{
    public class CongCuDungCuAnToanModel
    {
        public int ID { get; set; }
        public string TenThietBi { get; set; }
        public string MaHieu { get; set; }
        public string TenHangSX { get; set; }
        public string TenNuocSX { get; set; }
        public Nullable<int> NamSX { get; set; }
        public Nullable<System.DateTime> NgayDuaVaoSuDung { get; set; }
        public Nullable<int> PhongBanID { get; set; }
        public string DonViId { get; set; }
        public string TenPB { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public Nullable<int> HanKiemDinh { get; set; }
        public string QuyTacDanhMa { get; set; }
        public string TenTT { get; set; }


        //thong tin kiem dinh
        public Nullable<System.DateTime> NgayKiemTra { get; set; }
        public Nullable<System.DateTime> NgayKiemTraTiepTheo { get; set; }
        public string GhiChu { get; set; }

        public int TrangThai { get; set; }
    }
}