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
    public class PublishController : ApiController
    {
        public IHttpActionResult GetPublishInfoECP()
        {
            string sPublish = "PublishDate";
            string sVersion = "Version";
            using (var npc = new ECP_NPCEntities())
            {
                //var serverImageNPC = npc.tbl_Company.Where(x => x.MA_DVIQLY.ToLower().Equals(sNPC.ToLower())).FirstOrDefault();
                var publish = npc.NPCSystemConfigs.Where(x => x.Name.ToLower().Equals(sPublish.ToLower())).FirstOrDefault();
                var version = npc.NPCSystemConfigs.Where(x => x.Name.ToLower().Equals(sVersion.ToLower())).FirstOrDefault();

                if (publish != null && version != null)
                {
                    var info = new
                    {
                        publish = publish.Value,
                        version = version.Value
                    };

                    return Ok(info);
                }
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }
    }
}
