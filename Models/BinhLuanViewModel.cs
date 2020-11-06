using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECP_WEBAPI.Models
{
    public class BinhLuanViewModel
    {
        //public int Id { get; set; }
        public string CommentContent { get; set; }
        //public System.DateTime CreateTime { get; set; }
        public string Username { get; set; }
        //public short Priority { get; set; }
        //public string Description { get; set; }
        public Nullable<int> PhienLamViecId { get; set; }
        public string DBName { get; set; }
    }
}