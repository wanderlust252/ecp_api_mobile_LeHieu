using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECP_WEBAPI.Models
{
    public class sc_KienNghiMienTruModel
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public Nullable<int> SuCoId { get; set; }
        public string NoiDung { get; set; }
    }
}