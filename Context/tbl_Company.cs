
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
    
public partial class tbl_Company
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public tbl_Company()
    {

        this.AspNetUsers = new HashSet<AspNetUser>();

    }


    public string MA_DVIQLY { get; set; }

    public string TEN_DVIQLY { get; set; }

    public string MA_DVICTREN { get; set; }

    public Nullable<int> CAP_DVI { get; set; }

    public string DIA_CHI { get; set; }

    public string DIEN_THOAI { get; set; }

    public string DTHOAI_KDOANH { get; set; }

    public string DTHOAI_NONG { get; set; }

    public string DTHOAI_TRUC { get; set; }

    public string FAX { get; set; }

    public string EMAIL { get; set; }

    public string MA_STHUE { get; set; }

    public string DAI_DIEN { get; set; }

    public string CHUC_VU { get; set; }

    public string SO_UQUYEN { get; set; }

    public Nullable<System.DateTime> NGAY_UQUYEN { get; set; }

    public string TEN_DVIUQ { get; set; }

    public string DCHI_DVIUQ { get; set; }

    public string CVU_UQUYEN { get; set; }

    public string TNGUOI_UQUYEN { get; set; }

    public string TEN_TINH { get; set; }

    public string WEBSITE { get; set; }

    public string SERVERNAME { get; set; }

    public string DATABASENAME { get; set; }

    public string DBUSERNAME { get; set; }

    public string DBPASSWORD { get; set; }

    public string SERVERNAMEIMAGE { get; set; }

    public string LINKAPI { get; set; }

    public string SERVERFILEUPLOAD { get; set; }

    public string SERVERNAMEIMAGE_PUB { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AspNetUser> AspNetUsers { get; set; }

}

}
