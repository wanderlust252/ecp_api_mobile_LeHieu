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
    
    public partial class SystemConfig
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string UniqueKey { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
