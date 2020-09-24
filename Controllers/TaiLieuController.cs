using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using ECP_WEBAPI.Context;

namespace ECPNPC_API.Controllers
{
    public class TaiLieuController : ApiController
    {
        //private ECP_NPCEntities npc = new ECP_NPCEntities();

        public IHttpActionResult GetListTaiLieuByDonVi(string MA_DVIQLY)
        {
            using (var npc = new ECP_NPCEntities())
            {
                string sNPC = "PA";
                //var serverImageNPC = npc.tbl_Company.Where(x => x.MA_DVIQLY.ToLower().Equals(sNPC.ToLower())).FirstOrDefault();
                List<tbl_TaiLieu> taiLieuList = npc.tbl_TaiLieu.Where(x => x.MA_DVIQLY.ToLower().Equals(MA_DVIQLY.ToLower()) || x.MA_DVIQLY.ToLower().Equals(sNPC.ToLower())).ToList();

                if (taiLieuList != null && taiLieuList.Count > 0)
                {
                    //foreach (var item in taiLieuList)
                    //{
                    //    string url = "http://" + serverImageNPC.SERVERNAMEIMAGE + item.Url;
                    //    item.Url = url;
                    //}

                    return Ok(taiLieuList);
                }                
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }
    }
}
