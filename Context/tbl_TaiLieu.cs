//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ECP_WEBAPI.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_TaiLieu
    {
        public int Id { get; set; }
        public string TenTaiLieu { get; set; }
        public string Url { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<System.DateTime> NgayCapNhat { get; set; }
        public string NguoiCapNhat { get; set; }
        public string MA_DVIQLY { get; set; }
        public Nullable<int> LoaiTaiLieu { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsPublic { get; set; }
    }
}
