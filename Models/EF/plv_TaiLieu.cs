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
    
    public partial class plv_TaiLieu
    {
        public int ID { get; set; }
        public string URL { get; set; }
        public string Kieu { get; set; }
        public string Ten { get; set; }
        public Nullable<int> MaPCT { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
    
        public virtual plv_PhieuCongTac plv_PhieuCongTac { get; set; }
    }
}
