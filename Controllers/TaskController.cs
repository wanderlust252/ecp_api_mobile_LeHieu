using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ECP_WEBAPI.Context;
using System.Configuration;

namespace ECP_WEBAPI.Controllers
{
    public class TaskController : ApiController
    {
        public string cnn = ConfigurationManager.ConnectionStrings["ECP_Model"].ConnectionString;
        public TaskController()
        {
        }
        [HttpPost]
        public long CreateTask(tblTask tbl)
        {
            DateTime date = Convert.ToDateTime(tbl.CrateDate);
            DateTime createdate = new DateTime(date.Year, date.Month, date.Day);
            GoodmorningDataContext db = new GoodmorningDataContext(cnn);
            tblTask obj = new tblTask();
            obj.Active = tbl.Active;
            obj.ContentTask = tbl.ContentTask;
            obj.CrateDate = createdate;
            obj.TitleTask = tbl.TitleTask;
            obj.UserId = tbl.UserId;
            db.tblTasks.InsertOnSubmit(obj);
            db.SubmitChanges();
            return obj.IdTask;
        }
        [HttpPost]
        public long updateTask(tblTask tbl)
        {
            GoodmorningDataContext db = new GoodmorningDataContext(cnn);
            tblTask obj = new tblTask();
            obj = db.tblTasks.FirstOrDefault(x => x.IdTask == tbl.IdTask);
            if(obj!=null)
            {
                obj.Active = tbl.Active;
                obj.ContentTask = tbl.ContentTask;
                obj.CrateDate = tbl.CrateDate;
                obj.TitleTask = tbl.TitleTask;
                obj.UserId = tbl.UserId;
                db.tblTasks.InsertOnSubmit(obj);
                db.SubmitChanges();
                return obj.IdTask;
            }
            else
            {
                return 0;
            }
           
        }
        [HttpGet]
        public List<tblTask> GetTask(string UserId, string Date)
        {
            GoodmorningDataContext db = new GoodmorningDataContext(cnn);
            List<tblTask> obj = new List<tblTask>();
            DateTime date = Convert.ToDateTime(Date);
            DateTime createdate = new DateTime(date.Year, date.Month, date.Day,0,0,0);
            obj = (from c in db.tblTasks where c.UserId == Convert.ToInt64(UserId)
                  // && c.CrateDate == createdate
                    && c.CrateDate.Value.Year == date.Year
                    && c.CrateDate.Value.Month == date.Month
                    && c.CrateDate.Value.Day == date.Day

                   select c).ToList<tblTask>();
             if (obj != null)
            {
          
                return obj ;
            }
            else
            {
                return null;
            }

        }

    }
}

