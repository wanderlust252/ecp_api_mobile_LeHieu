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
    
    public partial class sc_SuCoPTD
    {
        public int Id { get; set; }
        public Nullable<int> SuCoId { get; set; }
        public Nullable<int> PhanTuDienId { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public string NguoiTao { get; set; }
    
        public virtual sc_PhanTuDien sc_PhanTuDien { get; set; }
        public virtual sc_TaiNanSuCo sc_TaiNanSuCo { get; set; }
    }
}
