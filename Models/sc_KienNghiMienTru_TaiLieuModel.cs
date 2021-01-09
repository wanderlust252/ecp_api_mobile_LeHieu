using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECP_WEBAPI.Models
{
    public class sc_KienNghiMienTru_TaiLieuModel
    {
        public int Id { get; set; }
        public int TypeObj { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public Nullable<int> KienNghiId { get; set; }

        public virtual sc_KienNghiMienTruModel sc_KienNghiMienTru { get; set; }
    }
}