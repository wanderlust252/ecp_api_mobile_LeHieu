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
    
    public partial class sc_TaiLieu
    {
        public int Id { get; set; }
        public int TypeObj { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
        public string NguoiSua { get; set; }
        public Nullable<int> SuCoId { get; set; }
    
        public virtual sc_TaiNanSuCo sc_TaiNanSuCo { get; set; }
    }
}
