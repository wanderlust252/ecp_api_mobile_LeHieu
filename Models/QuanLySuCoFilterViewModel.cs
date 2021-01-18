using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECP_WEBAPI.Models
{
    public class QuanLySuCoFilterViewModel
    {
        public int Id { get; set; }
        public string TypeOfLSC { get; set; }
        public string TenLoaiSuCo { get; set; }
        public string MoTa { get; set; }
    }
}