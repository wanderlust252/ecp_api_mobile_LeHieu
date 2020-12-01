using Dapper;
using ECP_WEBAPI.Context;
using ECP_WEBAPI.Models;
using ECP_WEBAPI.Models.EF;
using ECPNPC_API.Helper;
using ECPNPC_API.Models;
using Microsoft.AspNet.Identity;
using OneSignal.CSharp.SDK;
using OneSignal.CSharp.SDK.Resources;
using OneSignal.CSharp.SDK.Resources.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace ECPNPC_API.Controllers
{
    public class ECPNPCController : ApiController
    {
        static List<ConnectionStringObj> _ConnectionStringObj = new List<ConnectionStringObj>();

        public string GetConnectString(string strMaDviqly)
        {
            if (!string.IsNullOrEmpty(strMaDviqly))
            {
                strMaDviqly = strMaDviqly.ToUpper();
                if (_ConnectionStringObj != null)
                {
                    var connectionStringObj = _ConnectionStringObj.FirstOrDefault(x => strMaDviqly == (x.DBName));
                    if (connectionStringObj != null && !string.IsNullOrEmpty(connectionStringObj.ConnectionString))
                    {
                        return connectionStringObj.ConnectionString;
                    }
                }
                string strConn = string.Empty;
                using (var npc = new ECP_NPCEntities())
                {
                    var vConn = (from p in npc.tbl_Company
                                 where strMaDviqly == (p.MA_DVIQLY) && p.MA_DVIQLY.Length >= 2 && p.MA_DVIQLY != "PA"
                                 select new
                                 {
                                     p.MA_DVIQLY,
                                     p.TEN_DVIQLY,
                                     p.MA_DVICTREN,
                                     p.SERVERNAME,
                                     p.DATABASENAME,
                                     p.DBUSERNAME,
                                     p.DBPASSWORD,
                                     p.SERVERNAMEIMAGE,
                                     p.LINKAPI
                                 }).FirstOrDefault();
                    var iConn = vConn;
                    strConn = "data source=" + iConn.SERVERNAME + ";initial catalog=" + iConn.DATABASENAME
                        + ";persist security info=True;user id=" + iConn.DBUSERNAME + ";password=" + iConn.DBPASSWORD
                        + ";MultipleActiveResultSets=True;Integrated Security=false;";
                    _ConnectionStringObj.Add(new ConnectionStringObj() { DBName = strMaDviqly, ConnectionString = strConn });
                }
                return strConn;
            }
            return "DBNotFound";
        }

        public string GetEntityConnectionString(string connectionString,int optionDapper=0)
        {
            var entityBuilder = new EntityConnectionStringBuilder();
            //data source = DESKTOP - FSCTC81\SQLEXPRESS; initial catalog = ECP_PA04; integrated security = False; persist security info = True; user id = sa; password = 123456
            entityBuilder.Provider = "System.Data.SqlClient";
            entityBuilder.ProviderConnectionString = GetConnectString(connectionString) + ";MultipleActiveResultSets=True;App=EntityFramework;";
            entityBuilder.Metadata = @"res://*/Models.EF.ECP_Model.csdl|res://*/Models.EF.ECP_Model.ssdl|res://*/Models.EF.ECP_Model.msl";
            if (optionDapper == 1)
            {
                return entityBuilder.ProviderConnectionString;
            }
            return entityBuilder.ToString();
        }

        [Route("GetPhien")]
        [System.Web.Mvc.HttpGet]
        public object GetPhien(int idphong, string iddvi, int idrole, string tungay, string denngay, int tcphien, string userid, string dbname, int page, int pageSize)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                DateTime dtungay = DateTime.ParseExact(tungay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime ddennngay = DateTime.ParseExact(denngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                if (idrole < 4) // cap quan ly
                {
                    // if (idphong == -1) // id phong null, quan ly cap don vi, lay tat ca phien lam viec cua cac phong ban         
                    // {
                    var kq = (from i in db.tblPhienLamViecs
                              from j in db.tblPhongBans
                              where j.MaDVi == iddvi
                                 && i.PhongBanID == j.Id
                                 && (idphong == -1 || i.PhongBanID == idphong)
                                 && (tcphien == -1 || i.TT_Phien == tcphien)
                                 && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                                 && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                                 && i.TrangThai == 2
                              orderby i.Id descending
                              select new
                              {
                                  i.DiaDiem,
                                  i.GiamSatVien,
                                  i.GioBd,
                                  i.GioKt,
                                  i.Id,
                                  i.LanhDaoTrucBan,
                                  i.NgayDuyet,
                                  i.NgayLamViec,
                                  i.NgaySua,
                                  i.NgayTao,
                                  i.NguoiChiHuy,
                                  i.NguoiDuyet,
                                  i.NguoiDuyet_SoPa,
                                  i.NguoiKiemSoat,
                                  i.NguoiKiemTraPhieu,
                                  i.NguoiSua,
                                  i.NguoiTao,
                                  i.NoiDung,
                                  i.PhongBanID,
                                  i.TrangThai,
                                  i.KinhDo,
                                  i.ViDo
                              }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();
                    return kq;
                    // }
                }
                else
                {
                    var kq1 = (from i in db.tblPhienLamViecs
                               from x in db.tbl_NhanVien_PhienLamViec
                               where
                               i.Id == x.PhienLamViecId
                               && (userid == "-1" || x.NhanVienId == userid)
                               && (tcphien == -1 || i.TT_Phien == tcphien)
                               && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                               && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                               && i.TrangThai == 2
                               && i.NgayLamViec.Day == DateTime.Now.Day
                               && i.NgayLamViec.Month == DateTime.Now.Month
                               && i.NgayLamViec.Year == DateTime.Now.Year
                               orderby i.Id descending
                               select new
                               {
                                   i.DiaDiem,
                                   i.GiamSatVien,
                                   i.GioBd,
                                   i.GioKt,
                                   i.Id,
                                   i.LanhDaoTrucBan,
                                   i.NgayDuyet,
                                   i.NgayLamViec,
                                   i.NgaySua,
                                   i.NgayTao,
                                   i.NguoiChiHuy,
                                   i.NguoiDuyet,
                                   i.NguoiDuyet_SoPa,
                                   i.NguoiKiemSoat,
                                   i.NguoiKiemTraPhieu,
                                   i.NguoiSua,
                                   i.NguoiTao,
                                   i.NoiDung,
                                   i.PhongBanID,
                                   i.TrangThai,
                                   i.KinhDo,
                                   i.ViDo
                               }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).ToList();
                    var kq2 = (from i in db.tblPhienLamViecs
                               from x in db.tbl_NhanVien_PhienLamViec
                               where
                               i.Id == x.PhienLamViecId
                               && (userid == "-1" || x.NhanVienId == userid)
                               && (tcphien == -1 || i.TT_Phien == tcphien)
                               && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                               && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                               && i.TrangThai == 2
                               && i.NgayLamViec != DateTime.Now
                               //&& i.NgayLamViec.Day != DateTime.Now.Day
                               //&& i.NgayLamViec.Month != DateTime.Now.Month
                               //&& i.NgayLamViec.Year != DateTime.Now.Year
                               orderby i.Id descending
                               select new
                               {
                                   i.DiaDiem,
                                   i.GiamSatVien,
                                   i.GioBd,
                                   i.GioKt,
                                   i.Id,
                                   i.LanhDaoTrucBan,
                                   i.NgayDuyet,
                                   i.NgayLamViec,
                                   i.NgaySua,
                                   i.NgayTao,
                                   i.NguoiChiHuy,
                                   i.NguoiDuyet,
                                   i.NguoiDuyet_SoPa,
                                   i.NguoiKiemSoat,
                                   i.NguoiKiemTraPhieu,
                                   i.NguoiSua,
                                   i.NguoiTao,
                                   i.NoiDung,
                                   i.PhongBanID,
                                   i.TrangThai,
                                   i.KinhDo,
                                   i.ViDo
                               }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).ToList();
                    var kq3 = kq1.Union(kq2);
                    var kq4 = kq3.Skip(page * pageSize).Take(pageSize);
                    return kq4;
                }
            }
        }

        [Route("GetPhienKTVATCT")]
        [System.Web.Mvc.HttpGet]
        public object GetPhienKTVATCT(int idphong, string iddvi, int idrole, string tungay, string denngay, int tcphien, string dbname, int page = 0, int pageSize = 500)
        {
            //LayStrConnect(iddvi);
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                DateTime dtungay = DateTime.ParseExact(tungay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime ddennngay = DateTime.ParseExact(denngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                if (idrole < 4) // cap quan ly
                {
                    // if (idphong == -1) // id phong null, quan ly cap don vi, lay tat ca phien lam viec cua cac phong ban         
                    // {
                    var kq = (from i in db.tblPhienLamViecs
                              from j in db.tblPhongBans
                              where j.MaDVi == iddvi
                                 && i.PhongBanID == j.Id
                                 && (idphong == -1 || i.PhongBanID == idphong)
                                 && (tcphien == -1 || i.TT_Phien == tcphien)
                                 && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                                 && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                                 && i.TrangThai == 2
                              orderby i.Id descending
                              select new
                              {
                                  i.DiaDiem,
                                  i.GiamSatVien,
                                  i.GioBd,
                                  i.GioKt,
                                  i.Id,
                                  i.LanhDaoTrucBan,
                                  i.NgayDuyet,
                                  i.NgayLamViec,
                                  i.NgaySua,
                                  i.NgayTao,
                                  i.NguoiChiHuy,
                                  i.NguoiDuyet,
                                  i.NguoiDuyet_SoPa,
                                  i.NguoiKiemSoat,
                                  i.NguoiKiemTraPhieu,
                                  i.NguoiSua,
                                  i.NguoiTao,
                                  i.NoiDung,
                                  i.PhongBanID,
                                  i.TrangThai,
                                  i.KinhDo,
                                  i.ViDo
                              }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();
                    return kq;
                    // }
                }
                else
                { // lay thong tin theo 1 phong
                    var kq = (from i in db.tblPhienLamViecs
                              from x in db.tbl_NhanVien_PhienLamViec
                              where i.PhongBanID == idphong &&
                              i.Id == x.PhienLamViecId
                              && (tcphien == -1 || i.TT_Phien == tcphien)
                              && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                              && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                              && i.TrangThai == 2
                              orderby i.Id descending
                              select new
                              {
                                  i.DiaDiem,
                                  i.GiamSatVien,
                                  i.GioBd,
                                  i.GioKt,
                                  i.Id,
                                  i.LanhDaoTrucBan,
                                  i.NgayDuyet,
                                  i.NgayLamViec,
                                  i.NgaySua,
                                  i.NgayTao,
                                  i.NguoiChiHuy,
                                  i.NguoiDuyet,
                                  i.NguoiDuyet_SoPa,
                                  i.NguoiKiemSoat,
                                  i.NguoiKiemTraPhieu,
                                  i.NguoiSua,
                                  i.NguoiTao,
                                  i.NoiDung,
                                  i.PhongBanID,
                                  i.TrangThai,
                                  i.KinhDo,
                                  i.ViDo
                              }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();
                    return kq;
                }
            }
        }

        [Route("GetPhien_TimKiem/{idphong}/{iddvi}/{idrole}/{tungay}/{denngay}/{tcphien}/{userid}/{dbname}/{page}/{pageSize}/{tennguoi}/{tencviec}/{tonghoptiendo}")]
        [System.Web.Mvc.HttpGet]
        public object GetPhien_TimKiem(int idphong, string iddvi, int idrole, string tungay, string denngay, int tcphien, string userid, string dbname, int page, int pageSize, string tennguoi, string tencviec, int tonghoptiendo)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                DateTime dtungay = DateTime.ParseExact(tungay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime ddennngay = DateTime.ParseExact(denngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                if (idrole < 4) // cap quan ly
                {
                    // if (idphong == -1) // id phong null, quan ly cap don vi, lay tat ca phien lam viec cua cac phong ban         
                    // {
                    var kq = (from i in db.tblPhienLamViecs
                              from j in db.tblPhongBans
                              where j.MaDVi == iddvi
                                 && i.PhongBanID == j.Id
                                 && (idphong == -1 || i.PhongBanID == idphong)
                                 && (tcphien == -1 || i.TT_Phien == tcphien)
                                 && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                                 && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                                 && (tencviec == "-1" || i.NoiDung.ToUpper().Contains(tencviec.ToUpper()))
                                 // && (tennguoi == "-1" || (z.TenNhanVien.ToUpper().Contains(tennguoi.ToUpper())) && z.Id == x.NhanVienId)
                                 //&& i.TrangThai == 2
                                 && (tonghoptiendo == -1 || i.TrangThai == tonghoptiendo)
                              orderby i.Id descending
                              select new
                              {
                                  i.DiaDiem,
                                  i.GiamSatVien,
                                  i.GioBd,
                                  i.GioKt,
                                  i.Id,
                                  i.LanhDaoTrucBan,
                                  i.NgayDuyet,
                                  i.NgayLamViec,
                                  i.NgaySua,
                                  i.NgayTao,
                                  i.NguoiChiHuy,
                                  i.NguoiDuyet,
                                  i.NguoiDuyet_SoPa,
                                  i.NguoiKiemSoat,
                                  i.NguoiKiemTraPhieu,
                                  i.NguoiSua,
                                  i.NguoiTao,
                                  i.NoiDung,
                                  i.PhongBanID,
                                  i.TrangThai,
                                  i.KinhDo,
                                  i.ViDo
                              }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();
                    return kq;
                    // }
                }
                else
                {
                    var kq1 = (from i in db.tblPhienLamViecs
                               from x in db.tbl_NhanVien_PhienLamViec
                               from z in db.tblNhanViens
                               where
                               i.Id == x.PhienLamViecId
                               && (userid == "-1" || x.NhanVienId == userid)
                               && (tcphien == -1 || i.TT_Phien == tcphien)
                               && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                               && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                               //&& i.TrangThai == 2
                               && (tonghoptiendo == -1 || i.TrangThai == tonghoptiendo)
                               && i.NgayLamViec.Day == DateTime.Now.Day
                               && i.NgayLamViec.Month == DateTime.Now.Month
                               && i.NgayLamViec.Year == DateTime.Now.Year
                               && (tencviec == "-1" || i.NoiDung.ToUpper().Contains(tencviec.ToUpper()))
                               && (tennguoi == "-1" || (z.TenNhanVien.ToUpper().Contains(tennguoi.ToUpper())) && z.Id == x.NhanVienId)
                               orderby i.Id descending
                               select new
                               {
                                   i.DiaDiem,
                                   i.GiamSatVien,
                                   i.GioBd,
                                   i.GioKt,
                                   i.Id,
                                   i.LanhDaoTrucBan,
                                   i.NgayDuyet,
                                   i.NgayLamViec,
                                   i.NgaySua,
                                   i.NgayTao,
                                   i.NguoiChiHuy,
                                   i.NguoiDuyet,
                                   i.NguoiDuyet_SoPa,
                                   i.NguoiKiemSoat,
                                   i.NguoiKiemTraPhieu,
                                   i.NguoiSua,
                                   i.NguoiTao,
                                   i.NoiDung,
                                   i.PhongBanID,
                                   i.TrangThai,
                                   i.KinhDo,
                                   i.ViDo
                               }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).ToList();
                    var kq2 = (from i in db.tblPhienLamViecs
                               from x in db.tbl_NhanVien_PhienLamViec
                               from z in db.tblNhanViens
                               where
                               i.Id == x.PhienLamViecId
                               && (userid == "-1" || x.NhanVienId == userid)
                               && (tcphien == -1 || i.TT_Phien == tcphien)
                               && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                               && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                               //&& i.TrangThai == 2
                               && (tonghoptiendo == -1 || i.TrangThai == tonghoptiendo)
                               && i.NgayLamViec != DateTime.Now
                               && (tencviec == "-1" || i.NoiDung.ToUpper().Contains(tencviec.ToUpper()))
                               && (tennguoi == "-1" || (z.TenNhanVien.ToUpper().Contains(tennguoi.ToUpper())) && z.Id == x.NhanVienId)
                               //&& i.NgayLamViec.Day != DateTime.Now.Day
                               //&& i.NgayLamViec.Month != DateTime.Now.Month
                               //&& i.NgayLamViec.Year != DateTime.Now.Year
                               orderby i.Id descending
                               select new
                               {
                                   i.DiaDiem,
                                   i.GiamSatVien,
                                   i.GioBd,
                                   i.GioKt,
                                   i.Id,
                                   i.LanhDaoTrucBan,
                                   i.NgayDuyet,
                                   i.NgayLamViec,
                                   i.NgaySua,
                                   i.NgayTao,
                                   i.NguoiChiHuy,
                                   i.NguoiDuyet,
                                   i.NguoiDuyet_SoPa,
                                   i.NguoiKiemSoat,
                                   i.NguoiKiemTraPhieu,
                                   i.NguoiSua,
                                   i.NguoiTao,
                                   i.NoiDung,
                                   i.PhongBanID,
                                   i.TrangThai,
                                   i.KinhDo,
                                   i.ViDo
                               }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).ToList();
                    var kq3 = kq1.Union(kq2);
                    var kq4 = kq3.Skip(page * pageSize).Take(pageSize);
                    return kq4;
                }
            }
        }

        [Route("GetPhienKTVATCT_TimKiem/{idphong}/{iddvi}/{idrole}/{tungay}/{denngay}/{tcphien}/{userid}/{dbname}/{tennguoi}/{tencviec}/{tonghoptiendo}")]
        [System.Web.Mvc.HttpGet]
        public object GetPhienKTVATCT_TimKiem(int idphong, string iddvi, int idrole, string tungay, string denngay, int tcphien, string userid, string dbname, string tennguoi, string tencviec, int tonghoptiendo, int page = 0, int pageSize = 500)
        {
            //LayStrConnect(iddvi);
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                DateTime dtungay = DateTime.ParseExact(tungay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime ddennngay = DateTime.ParseExact(denngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                if (idrole < 4) // cap quan ly
                {
                    // if (idphong == -1) // id phong null, quan ly cap don vi, lay tat ca phien lam viec cua cac phong ban         
                    // {
                    var kq = (from i in db.tblPhienLamViecs
                              from j in db.tblPhongBans
                              where j.MaDVi == iddvi
                                 && i.PhongBanID == j.Id
                                 && (idphong == -1 || i.PhongBanID == idphong)
                                 && (tcphien == -1 || i.TT_Phien == tcphien)
                                 && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                                 && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                                 && (tencviec == "-1" || i.NoiDung.ToUpper().Contains(tencviec.ToUpper()))
                                 && (tonghoptiendo == -1 || i.TrangThai == tonghoptiendo)
                              // tennguoi
                              //&& i.TrangThai == 2
                              orderby i.Id descending
                              select new
                              {
                                  i.DiaDiem,
                                  i.GiamSatVien,
                                  i.GioBd,
                                  i.GioKt,
                                  i.Id,
                                  i.LanhDaoTrucBan,
                                  i.NgayDuyet,
                                  i.NgayLamViec,
                                  i.NgaySua,
                                  i.NgayTao,
                                  i.NguoiChiHuy,
                                  i.NguoiDuyet,
                                  i.NguoiDuyet_SoPa,
                                  i.NguoiKiemSoat,
                                  i.NguoiKiemTraPhieu,
                                  i.NguoiSua,
                                  i.NguoiTao,
                                  i.NoiDung,
                                  i.PhongBanID,
                                  i.TrangThai,
                                  i.KinhDo,
                                  i.ViDo
                              }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();
                    return kq;
                    // }
                }
                else
                { // lay thong tin theo 1 phong
                    var kq = (from i in db.tblPhienLamViecs
                              from x in db.tbl_NhanVien_PhienLamViec
                              from z in db.tblNhanViens
                              where i.PhongBanID == idphong &&
                              i.Id == x.PhienLamViecId
                              && (tcphien == -1 || i.TT_Phien == tcphien)
                              && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                              && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                              && (tencviec == "-1" || i.NoiDung.ToUpper().Contains(tencviec.ToUpper()))
                              && (tennguoi == "-1" || (z.TenNhanVien.ToUpper().Contains(tennguoi.ToUpper())) && z.Id == x.NhanVienId)
                              //&& i.TrangThai == 2
                              && (tonghoptiendo == -1 || i.TrangThai == tonghoptiendo)
                              orderby i.Id descending
                              select new
                              {
                                  i.DiaDiem,
                                  i.GiamSatVien,
                                  i.GioBd,
                                  i.GioKt,
                                  i.Id,
                                  i.LanhDaoTrucBan,
                                  i.NgayDuyet,
                                  i.NgayLamViec,
                                  i.NgaySua,
                                  i.NgayTao,
                                  i.NguoiChiHuy,
                                  i.NguoiDuyet,
                                  i.NguoiDuyet_SoPa,
                                  i.NguoiKiemSoat,
                                  i.NguoiKiemTraPhieu,
                                  i.NguoiSua,
                                  i.NguoiTao,
                                  i.NoiDung,
                                  i.PhongBanID,
                                  i.TrangThai,
                                  i.KinhDo,
                                  i.ViDo
                              }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();
                    return kq;
                }
            }
        }

        [Route("GetPhieuCongTac/{tungay}/{denngay}/{userid}/{dbname}/{page}/{pageSize}/{tennguoi}/{tencviec}")]
        [System.Web.Mvc.HttpGet]
        public object GetPhieuCongTac(string tungay, string denngay, string userid, string dbname, int page, int pageSize, string tennguoi, string tencviec)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                DateTime dtungay = DateTime.ParseExact(tungay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime ddennngay = DateTime.ParseExact(denngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                {
                    var kq1 = (from k in db.plv_PhieuCongTac
                               from x in db.plv_PhieuCT_NhanVien
                               from z in db.tblNhanViens
                               where
                               k.ID == x.MaPCT
                               && (tungay == "01-01-1990" || k.NgayTao >= dtungay)
                               && (denngay == "01-01-1990" || k.NgayTao <= ddennngay)
                               && (tencviec == "-1" || k.NoiDung.ToUpper().Contains(tencviec.ToUpper()))
                               && (tennguoi == "-1" || (z.TenNhanVien.ToUpper().Contains(tennguoi.ToUpper())) && z.Id == x.MaNV)
                               select new
                               {

                                   ID_PCT = k.ID,
                                   k.SoPhieu,
                                   k.MaLP,
                                   k.MaTT,
                                   k.NgayCN,
                                   NDUNG_PCT = k.NoiDung,
                                   k.AnToanKhac,
                                   k.ChiTietBienBao,
                                   k.ChiTietCatDien,
                                   k.ChiTietNoiDat,
                                   k.ChiTietRaoChan,
                                   k.CHTT_B2,
                                   k.CHTT_B3,
                                   k.CHTT_B6,
                                   k.DieuKienAnToan,
                                   k.DonViLienQuanQLVH,
                                   k.GiayBanGiaoQLVH,
                                   k.GSAT_B2,
                                   k.GSAT_B3,
                                   k.NgayGioKT_B2,
                                   k.NgayGioKT_B3,
                                   k.NgayGioKT_B6,
                                   k.NguoiChoPhep,
                                   k.NguoiCN,
                                   k.NGUOICP_B2,
                                   k.NGUOICP_B6,
                                   k.NguoiTaoKT_B6,
                                   k.NoiDatTai,
                                   k.PhamViLamViec,
                                   k.CanhBaoNguyHiem,
                                   NgayCapPhieu = k.NgayTao.Value.Day + "/" + k.NgayTao.Value.Month + "/" + k.NgayTao.Value.Year,
                               }).Distinct().ToList();
                    return kq1;
                }
            }
        }

        [Route("ThongKe/{tungay}/{denngay}/{dbname}")]
        [HttpGet]
        public object ThongKe(string tungay, string denngay, string dbname)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                DateTime dtungay = DateTime.ParseExact(tungay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime ddennngay = DateTime.ParseExact(denngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                var kq1 = (from i in db.tblPhienLamViecs
                           where
                           (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                           && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                           select new
                           {
                               i.DiaDiem,
                               i.GiamSatVien,
                               i.GioBd,
                               i.GioKt,
                               i.Id,
                               i.LanhDaoTrucBan,
                               i.NgayDuyet,
                               i.NgayLamViec,
                               i.NgaySua,
                               i.NgayTao,
                               i.NguoiChiHuy,
                               i.NguoiDuyet,
                               i.NguoiDuyet_SoPa,
                               i.NguoiKiemSoat,
                               i.NguoiKiemTraPhieu,
                               i.NguoiSua,
                               i.NguoiTao,
                               i.NoiDung,
                               i.PhongBanID,
                               i.TrangThai,
                               i.TT_Phien,
                               THANGNAM = i.NgayLamViec.Month + "." + i.NgayLamViec.Year
                           }).Distinct().ToList();
                return kq1;

            }
        }

        [System.Web.Mvc.HttpGet]
        public object GetPhien_HuySUA(string madonvi, int idphong, string iddvi, int idrole, string tungay, string denngay, int page = 0, int pageSize = 500)
        {
            //LayStrConnect(madonvi);
            using (var db = new ECP_PAEntities(GetEntityConnectionString(madonvi)))
            {
                DateTime dtungay = DateTime.ParseExact(tungay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime ddennngay = DateTime.ParseExact(denngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                if (idrole < 4) // cap quan ly
                {
                    // if (idphong == -1) // id phong null, quan ly cap don vi, lay tat ca phien lam viec cua cac phong ban         
                    // {
                    var kq = (from i in db.tblPhienLamViecs
                              from j in db.tblPhongBans
                              where j.MaDVi == iddvi
                                 && i.PhongBanID == j.Id
                                 && (idphong == -1 || i.PhongBanID == idphong)
                                 && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                                 && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                                 && (i.TrangThai == 2 || i.TrangThai == 3)
                              orderby i.Id descending
                              select new
                              {
                                  i.DiaDiem,
                                  i.GiamSatVien,
                                  i.GioBd,
                                  i.GioKt,
                                  i.Id,
                                  i.LanhDaoTrucBan,
                                  i.NgayDuyet,
                                  i.NgayLamViec,
                                  i.NgaySua,
                                  i.NgayTao,
                                  i.NguoiChiHuy,
                                  i.NguoiDuyet,
                                  i.NguoiDuyet_SoPa,
                                  i.NguoiKiemSoat,
                                  i.NguoiKiemTraPhieu,
                                  i.NguoiSua,
                                  i.NguoiTao,
                                  i.NoiDung,
                                  i.PhongBanID,
                                  i.TrangThai
                              }).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();
                    return kq;
                    // }
                }
                else
                { // lay thong tin theo 1 phong
                    var kq = (from i in db.tblPhienLamViecs
                              where i.PhongBanID == idphong
                                    && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                                 && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                                 && (i.TrangThai == 2 || i.TrangThai == 3)
                              orderby i.Id descending
                              select new
                              {
                                  i.DiaDiem,
                                  i.GiamSatVien,
                                  i.GioBd,
                                  i.GioKt,
                                  i.Id,
                                  i.LanhDaoTrucBan,
                                  i.NgayDuyet,
                                  i.NgayLamViec,
                                  i.NgaySua,
                                  i.NgayTao,
                                  i.NguoiChiHuy,
                                  i.NguoiDuyet,
                                  i.NguoiDuyet_SoPa,
                                  i.NguoiKiemSoat,
                                  i.NguoiKiemTraPhieu,
                                  i.NguoiSua,
                                  i.NguoiTao,
                                  i.NoiDung,
                                  i.PhongBanID,
                                  i.TrangThai
                              }).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();
                    return kq;
                }
            }
        }

        [Route("GetPhienHT")]
        [System.Web.Mvc.HttpGet]
        public object GetPhienHT(int idphong, string iddvi, int idrole, string tungay, string denngay, string userid, string dbname, int page = 0, int pageSize = 500)
        //(int idphong, string iddvi, int idrole, string tungay, string denngay, int tcphien, string userid)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                DateTime dtungay = DateTime.ParseExact(tungay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime ddennngay = DateTime.ParseExact(denngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                if (idrole < 4) // cap quan ly
                {
                    // if (idphong == -1) // id phong null, quan ly cap don vi, lay tat ca phien lam viec cua cac phong ban         
                    // {
                    var kq = (from i in db.tblPhienLamViecs
                              from j in db.tblPhongBans
                              where j.MaDVi == iddvi
                                 && i.PhongBanID == j.Id
                                 && (idphong == -1 || i.PhongBanID == idphong)
                                 && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                                 && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                                 && i.TrangThai == 3
                              orderby i.Id descending
                              select new
                              {
                                  i.DiaDiem,
                                  i.GiamSatVien,
                                  i.GioBd,
                                  i.GioKt,
                                  i.Id,
                                  i.LanhDaoTrucBan,
                                  i.NgayDuyet,
                                  i.NgayLamViec,
                                  i.NgaySua,
                                  i.NgayTao,
                                  i.NguoiChiHuy,
                                  i.NguoiDuyet,
                                  i.NguoiDuyet_SoPa,
                                  i.NguoiKiemSoat,
                                  i.NguoiKiemTraPhieu,
                                  i.NguoiSua,
                                  i.NguoiTao,
                                  i.NoiDung,
                                  i.PhongBanID,
                                  i.TrangThai,
                                  i.KinhDo,
                                  i.ViDo
                              }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();
                    return kq;
                    // }
                }
                else
                { // lay thong tin theo 1 phong
                    var kq = (from i in db.tblPhienLamViecs
                              from x in db.tbl_NhanVien_PhienLamViec
                              where // i.PhongBanID == idphong &&
                              i.Id == x.PhienLamViecId
                                 && (userid == "-1" || x.NhanVienId == userid)
                                 && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                                 && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                                 && i.TrangThai == 3
                              orderby i.Id descending
                              select new
                              {
                                  i.DiaDiem,
                                  i.GiamSatVien,
                                  i.GioBd,
                                  i.GioKt,
                                  i.Id,
                                  i.LanhDaoTrucBan,
                                  i.NgayDuyet,
                                  i.NgayLamViec,
                                  i.NgaySua,
                                  i.NgayTao,
                                  i.NguoiChiHuy,
                                  i.NguoiDuyet,
                                  i.NguoiDuyet_SoPa,
                                  i.NguoiKiemSoat,
                                  i.NguoiKiemTraPhieu,
                                  i.NguoiSua,
                                  i.NguoiTao,
                                  i.NoiDung,
                                  i.PhongBanID,
                                  i.TrangThai,
                                  i.KinhDo,
                                  i.ViDo
                              }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();
                    return kq;
                }
            }
        }
        [Route("GetLichSuPhienByTrangThai")]
        [System.Web.Mvc.HttpGet]
        public object GetLichSuPhienByTrangThai(int idphong, string iddvi, int idrole, string tungay, string denngay, string userid,int trangThai, string dbname, int page = 0, int pageSize = 500)
        //(int idphong, string iddvi, int idrole, string tungay, string denngay, int tcphien, string userid)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                DateTime dtungay = DateTime.ParseExact(tungay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime ddennngay = DateTime.ParseExact(denngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                if (idrole < 4) // cap quan ly
                {
                    // if (idphong == -1) // id phong null, quan ly cap don vi, lay tat ca phien lam viec cua cac phong ban         
                    // {
                    var kq = (from i in db.tblPhienLamViecs
                              from j in db.tblPhongBans
                              where j.MaDVi == iddvi
                                 && i.PhongBanID == j.Id
                                 && (idphong == -1 || i.PhongBanID == idphong)
                                 && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                                 && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                                 && i.TrangThai == trangThai
                              orderby i.Id descending
                              select new
                              {
                                  i.DiaDiem,
                                  i.GiamSatVien,
                                  i.GioBd,
                                  i.GioKt,
                                  i.Id,
                                  i.LanhDaoTrucBan,
                                  i.NgayDuyet,
                                  i.NgayLamViec,
                                  i.NgaySua,
                                  i.NgayTao,
                                  i.NguoiChiHuy,
                                  i.NguoiDuyet,
                                  i.NguoiDuyet_SoPa,
                                  i.NguoiKiemSoat,
                                  i.NguoiKiemTraPhieu,
                                  i.NguoiSua,
                                  i.NguoiTao,
                                  i.NoiDung,
                                  i.PhongBanID,
                                  i.TrangThai,
                                  i.KinhDo,
                                  i.ViDo
                              }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();
                    return kq;
                    // }
                }
                else
                { // lay thong tin theo 1 phong
                    var kq = (from i in db.tblPhienLamViecs
                              from x in db.tbl_NhanVien_PhienLamViec
                              where // i.PhongBanID == idphong &&
                              i.Id == x.PhienLamViecId
                                 && (userid == "-1" || x.NhanVienId == userid)
                                 && (tungay == "01-01-1990" || i.NgayLamViec >= dtungay)
                                 && (denngay == "01-01-1990" || i.NgayLamViec <= ddennngay)
                                 && i.TrangThai == trangThai
                              orderby i.Id descending
                              select new
                              {
                                  i.DiaDiem,
                                  i.GiamSatVien,
                                  i.GioBd,
                                  i.GioKt,
                                  i.Id,
                                  i.LanhDaoTrucBan,
                                  i.NgayDuyet,
                                  i.NgayLamViec,
                                  i.NgaySua,
                                  i.NgayTao,
                                  i.NguoiChiHuy,
                                  i.NguoiDuyet,
                                  i.NguoiDuyet_SoPa,
                                  i.NguoiKiemSoat,
                                  i.NguoiKiemTraPhieu,
                                  i.NguoiSua,
                                  i.NguoiTao,
                                  i.NoiDung,
                                  i.PhongBanID,
                                  i.TrangThai,
                                  i.KinhDo,
                                  i.ViDo
                              }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();
                    return kq;
                }
            }
        }
        [Route("GetNhanVien_PhienLV")]
        [System.Web.Mvc.HttpGet]
        public object GetNhanVien_PhienLV(string iddvi, string userid, int page = 0, int pageSize = 500)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(iddvi)))
            {
                int ngay = DateTime.Now.Day;
                int thang = DateTime.Now.Month;
                int nam = DateTime.Now.Year;

                var kq = (from i in db.tblPhienLamViecs
                          from x in db.tbl_NhanVien_PhienLamViec
                          where
                             x.NhanVienId == userid
                             && i.Id == x.PhienLamViecId
                             && i.TrangThai == 2
                             && i.NgayLamViec.Day == DateTime.Now.Day
                             && i.NgayLamViec.Month == DateTime.Now.Month
                             && i.NgayLamViec.Year == DateTime.Now.Year
                          orderby i.Id descending
                          select new
                          {
                              i.DiaDiem,
                              i.GiamSatVien,
                              i.GioBd,
                              i.GioKt,
                              i.Id,
                              i.LanhDaoTrucBan,
                              i.NgayDuyet,
                              i.NgayLamViec,
                              i.NgaySua,
                              i.NgayTao,
                              i.NguoiChiHuy,
                              i.NguoiDuyet,
                              i.NguoiDuyet_SoPa,
                              i.NguoiKiemSoat,
                              i.NguoiKiemTraPhieu,
                              i.NguoiSua,
                              i.NguoiTao,
                              i.NoiDung,
                              i.PhongBanID,
                              i.TrangThai,
                              i.KinhDo,
                              i.ViDo
                          }).Distinct().OrderByDescending(c => c.NgayLamViec).ThenBy(c => c.GioBd).Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();
                return kq;
            }
        }

        //[Route("GetPhong/{iddvi}")]
        //[System.Web.Mvc.HttpGet]
        //public object GetPhong(string iddvi)
        //{
        //    string conn = GetEntityConnectionString(iddvi);
        //    using (var db = new ECP_PAEntities(conn))
        //    {
        //        var kt = (from i in db.tblPhongBans
        //                  where i.MaDVi == iddvi
        //                  select new
        //                  {
        //                      i.Id,
        //                      i.MaDVi,
        //                      i.MoTa,
        //                      i.TenPhongBan
        //                  }).AsNoTracking().ToList();
        //        return kt;
        //    }
        //}
        [Route("GetPhongBanById")]
        [System.Web.Mvc.HttpGet]
        public object GetPhong(int phongBanId, string dbname)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                var kt = (from i in db.tblPhongBans
                          where i.Id == phongBanId
                          select new
                          {
                              i.Id,
                              i.MaDVi,
                              i.MoTa,
                              i.TenPhongBan,
                              i.SDT
                          }).AsNoTracking().FirstOrDefault();
                return kt;
            }
        }
        [Route("GetPhong")]
        [System.Web.Mvc.HttpGet]
        public object GetPhong(string iddvi, string dbname)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                var kt = (from i in db.tblPhongBans
                          where i.MaDVi == iddvi
                          select new
                          {
                              i.Id,
                              i.MaDVi,
                              i.MoTa,
                              i.TenPhongBan,
                              i.SDT
                          }).AsNoTracking().ToList();
                return kt;
            }
        }

        [Route("GetPhongTV")]
        [System.Web.Mvc.HttpGet]
        public object GetPhongTV(string iddvi, string dbname)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                var kt = (from i in db.tblPhongBans
                          where i.MaDVi == iddvi && i.LoaiPB == 1
                          select new
                          {
                              i.Id,
                              i.MaDVi,
                              i.MoTa,
                              i.TenPhongBan,
                              i.SDT
                          }).AsNoTracking().ToList();
                return kt;
            }
        }

        [Route("GetDvi")]
        [HttpGet]
        public object GetDvi(string iddvi, string dbname)
        {
            string conn = GetEntityConnectionString(dbname);
            using (var db = new ECP_PAEntities(conn))
            {
                var kt = (from i in db.tblDonVis
                          where i.DviCha == iddvi || i.Id == iddvi
                          select new
                          {
                              i.Id,
                              i.MoTa,
                              i.TenDonVi,
                              i.TenVietTat,
                              i.CapDvi,
                              i.DviCha,
                              i.SDT,
                              i.ViTri
                          }
                      ).AsNoTracking().ToList();
                return kt;
            }
        } 
         

        [HttpGet]
        public Object GetInforUserNPC(string username)
        {
            using (var npc = new ECP_NPCEntities())
            {
                var kq = (from p in npc.AspNetUsers
                          where p.UserName == username
                          select new
                          {
                              p.MA_DVIQLY,
                              p.MA_PBAN,
                              p.Id,
                              p.UserName
                          }).AsNoTracking().ToList();
                return kq;
            }
        }

        [Route("LayCongTyDienLuc")]
        [HttpGet]
        public Object LayCongTyDienLuc()
        {
            using (var npc = new ECP_NPCEntities())
            {
                var kq = (from p in npc.tbl_Company
                          where p.MA_DVIQLY.Length >= 2 // Fix (>=)
                          && p.MA_DVICTREN == "PA"
                          select new
                          {
                              p.MA_DVIQLY,
                              p.TEN_DVIQLY,
                              p.SERVERNAME,
                              p.DATABASENAME,
                              p.DBUSERNAME,
                              p.DBPASSWORD,
                              p.SERVERNAMEIMAGE,
                              p.LINKAPI,
                              p.SERVERFILEUPLOAD,
                              p.SERVERNAMEIMAGE_PUB
                          }).AsNoTracking().ToList();
                return kq;
            }
        }

        [HttpGet]
        public string VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return "";
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return "";
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return buffer3 + " --- " + buffer4; //  ByteArraysEqual(buffer3, buffer4);
        }

        [HttpGet]
        public string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        [Route("GetInforUser")]
        [HttpGet]
        public Object GetInforUser(string username, string dbname)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                var kt = (from j in db.tblNhanViens
                          join u in db.AspNetUsers
                          on j.Id equals u.Id
                          where j.Username == username //&& i.PasswordHash == pass
                          select new { j.UrlImage,j.TenNhanVien, j.PhongBanId, j.DonViId,j.NgaySinh, j.Id, j.ChucVu,j.DiaChi,j.BacAnToan, Role = u.AspNetRoles.FirstOrDefault().Id }).AsNoTracking().ToList();

                if (kt == null)
                {
                    return "0";
                }
                return kt;
            }
        }
        [Route("GetInfoAdminForgotPassword")]//em dang lam api nay
        [HttpGet]
        public async Task<object> GetInfoAdminForgotPassword(string role, string dbname)
        {
            using (IDbConnection db = new SqlConnection(GetEntityConnectionString(dbname,1)))
            {
                string pban;
                pban = @"SELECT distinct b.Id,b.TenNhanVien,b.SoDT,b.Email
                      FROM  [AspNetUserRoles] a join [tblNhanVien] b on a.UserId=b.Id
                      where a.roleid in ('1','17') and DonViId=@role";
                return (await db.QueryAsync<object>(pban, new { role })).ToList();

            }
        }
        [Route("ListNVByRoleID/{iduser}/{donviid}/{dbname}")]
        [HttpGet]
        public Object ListNVByRoleID(string iduser, string donviid, string dbname)
        {
            List<mRole> lstRoleUser = (List<mRole>)GetRoleUser(iduser, dbname);

            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                List<tblNhanVien> lstRes = new List<tblNhanVien>();
                foreach (mRole m in lstRoleUser)
                {
                    var result = db.Database.SqlQuery<tblNhanVien>("EXEC NPC_sp_AdvancedSearchNhanVienByRole @roleId", new SqlParameter("@roleId", m.id));
                    lstRes.AddRange(result.ToList().Where(c => c.DonViId == donviid));
                }
                return lstRes.Distinct();
            }
        }

        [Route("ListPBByRoleID/{iduser}/{donviid}/{dbname}")]
        [HttpGet]
        public Object ListPBByRoleID(string iduser, string donviid, string dbname)
        {
            try
            {
                List<mRole> lstRoleUser = (List<mRole>)GetRoleUser(iduser, dbname);

                using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
                {
                    List<object> lstRes = new List<object>();
                    foreach (mRole m in lstRoleUser)
                    {
                        var result = db.Database.SqlQuery<tblNhanVien>("EXEC NPC_sp_AdvancedSearchNhanVienByRole @roleId", new SqlParameter("@roleId", m.id));
                        List<tblNhanVien> listNV = result.Where(c => c.DonViId == donviid).ToList();
                        List<int?> idPB = listNV.Select(c => c.PhongBanId).ToList();
                        var listPB = (from i in db.tblPhongBans
                                      where idPB.Contains(i.Id)
                                      select new
                                      {
                                          i.Id,
                                          i.MaDVi,
                                          i.MoTa,
                                          i.TenPhongBan
                                      }).AsNoTracking().ToList();
                        lstRes.AddRange(listPB);
                    }
                    return lstRes;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public class mRole
        {
            public string id { get; set; }
            public string name { get; set; }
            public string des { get; set; }
            public int type { get; set; }
        }

        [Route("GetRoleUser")]
        [HttpGet]
        public object GetRoleUser(string iduser, string dbname)
        {
            string strConnect = GetConnectString(dbname);
            List<object> x = new List<object>();
            using (SqlConnection connection = new SqlConnection(strConnect))
            {
                connection.Open();
                string strSQL = "SELECT  b.Id, b.Name, b.Description, b.TypeOfRole from AspNetUserRoles a, AspNetRoles b  WHERE a.UserId = '" + iduser + "' and a.RoleId = b.Id";
                SqlCommand DBCmd = new SqlCommand(strSQL, connection);
                SqlDataReader myDataReader;
                myDataReader = DBCmd.ExecuteReader();
                while (myDataReader.Read())
                {
                    string id = myDataReader.GetString(0);
                    string name = myDataReader.GetString(1);
                    string des = myDataReader.GetString(2);
                    int type = myDataReader.GetInt32(3);
                    var m = new
                    {
                        id = id,
                        name = name,
                        des = des,
                        type = type
                    };
                    x.Add(m);
                }
                connection.Close();
            }
            return x;
        }

        [Route("GetImagePhien")]
        [HttpGet]
        public object GetImagePhien(int id_phien_lviec, string dbname)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                var a = (from i in db.tblImages
                         where i.PhienLamViecId == id_phien_lviec
                         select new
                         {
                             i.Comment,
                             i.GroupId,
                             i.Id,
                             i.ImgSize,
                             i.isVideo,
                             i.KinhDo,
                             i.NgayCapNhat,
                             i.NgayChup,
                             i.Note,
                             i.PhienLamViecId,
                             i.Tag,
                             i.Type,
                             i.Url,
                             i.VideoPath,
                             i.ViDo,
                             i.UserUp,
                         }).ToList();
                return a;
            }
        }

        [Route("TestConvert/{modificationDate}")]
        [HttpGet]
        public object TestConvert(string modificationDate)
        {
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
            long m = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            int ran = new Random().Next(10000, 60000);
            long res = m + ran;
            return m + "   " + ran + " " + res;

            //DateTime dtModify = DateTime.Now;
            //try
            //{
            //    dtModify = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(modificationDate)).AddHours(7);
            //}
            //catch
            //{
            //    dtModify = DateTime.Now;
            //}
            //return dtModify;
        }


        [Route("GetImagePhienFullInfo/{id_phien_lviec}/{dbname}")]
        [HttpGet]
        public object GetImagePhienFullInfo(int id_phien_lviec, string dbname)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                var a = (from i in db.tblImages
                         from p in db.tblNhanViens
                         from j in db.tblPhongBans
                         from d in db.tblDonVis
                         where i.PhienLamViecId == id_phien_lviec
                         && i.UserUp == p.Id
                         && p.PhongBanId == j.Id
                         && j.MaDVi == d.Id
                         select new
                         {
                             i.Comment,
                             i.GroupId,
                             i.Id,
                             i.ImgSize,
                             i.isVideo,
                             i.KinhDo,
                             i.NgayCapNhat,
                             i.NgayChup,
                             i.Note,
                             i.PhienLamViecId,
                             i.Tag,
                             i.Type,
                             i.Url,
                             i.VideoPath,
                             i.ViDo,
                             i.UserUp,
                             p.SoDT,
                             p.TenNhanVien,
                             j.TenPhongBan,
                             d.TenDonVi
                         }).ToList();
                return a;
            }
        }

        [HttpPost]
        public object UploadFile()
        {
            try
            {
                HttpFileCollection files = HttpContext.Current.Request.Files;
                int idphien = -1;
                int.TryParse(HttpContext.Current.Request.Params["value1"], out idphien);
                int idphong = -1;
                int.TryParse(HttpContext.Current.Request.Params["value2"], out idphong);
                string userid = HttpContext.Current.Request.Params["value3"];
                string iddvi = HttpContext.Current.Request.Params["iddvi"];
                string strSoPhieu = HttpContext.Current.Request.Params["strSoPhieu"];
                string kinhDo = "0";
                string viDo = "0";
                try
                {
                    kinhDo = HttpContext.Current.Request.Params["kinhDo"];
                    viDo = HttpContext.Current.Request.Params["viDo"];
                }
                catch { }
                string nhomAnh = "";
                try
                {
                    nhomAnh = HttpContext.Current.Request.Params["nhomAnh"];
                }
                catch { }
                string donvicha = "";
                if (iddvi.StartsWith("PH")) donvicha = "PH";
                else if (iddvi.StartsWith("PN")) donvicha = "PN";
                else if (iddvi.StartsWith("PM")) donvicha = "PM";
                else donvicha = HttpContext.Current.Request.Params["database"];


                foreach (string fileName in HttpContext.Current.Request.Files)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[fileName];
                    if (file.ContentLength > 0)
                    {
                        string root = HttpContext.Current.Server.MapPath("~/ImagesPLV/" + donvicha + "/Files");
                        string directory = "";
                        string pathurl = "";
                        DateTime dt = DateTime.Now;
                        //string filename = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString()
                        //    + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString()
                        //    + file.FileName.Substring(file.FileName.LastIndexOf("."));

                        string modificationDate = file.FileName;
                        DateTime dtModify = DateTime.Now;
                        try
                        {
                            //dtModify = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(modificationDate)).AddHours(7);
                            dtModify = UnixTimestampToDateTime(modificationDate);
                        }
                        catch
                        {
                            dtModify = DateTime.Now;
                        }
                        string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";

                        long m = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                        int ran = new Random().Next(10000, 60000);
                        long res = m + ran;
                        filename = res + ".png";

                        string strNam = dt.Year.ToString();
                        string strThang = dt.Month.ToString();
                        string strNgay = dt.Day.ToString();
                        if (idphien == -1)
                        {
                            directory = root + "/" + iddvi + "/0";
                            pathurl = "/" + iddvi + "/0/" + filename;
                        }
                        else
                        {
                            directory = root + "/" + iddvi + "/" + idphong + "/" + strNam + "/" + strThang + "/" + strNgay;
                            pathurl = "/" + iddvi + "/" + idphong + "/" + strNam + "/" + strThang + "/" + strNgay + "/" + filename;
                        }

                        if (Directory.Exists(directory) == false)
                            Directory.CreateDirectory(directory);
                        var path = Path.Combine(directory, filename);

                        file.SaveAs(path);

                        if (idphien == -1)
                            insert_image_phien(null, userid, pathurl, kinhDo, viDo, nhomAnh, iddvi, strSoPhieu, donvicha, dtModify);
                        else
                            insert_image_phien(idphien, userid, pathurl, kinhDo, viDo, nhomAnh, iddvi, strSoPhieu, donvicha, dtModify);
                    }
                }

                return Ok(new { success = true, mess = "OK" });
            }
            catch (Exception ex)
            {
                var messages = new List<string>();
                do
                {
                    messages.Add(ex.Message);
                    ex = ex.InnerException;
                }
                while (ex != null);
                var message = string.Join(" - ", messages);

                return Ok(new { success = false, mess = message });
            }
        }


        [HttpPost]
        public object UploadFileV2()
        {
            try
            {
                HttpFileCollection files = HttpContext.Current.Request.Files;
                int idphien = -1;
                int.TryParse(HttpContext.Current.Request.Params["value1"], out idphien);
                int idphong = -1;
                int.TryParse(HttpContext.Current.Request.Params["value2"], out idphong);
                string userid = HttpContext.Current.Request.Params["value3"];
                string iddvi = HttpContext.Current.Request.Params["iddvi"];
                string strSoPhieu = HttpContext.Current.Request.Params["strSoPhieu"];
                string kinhDo = "0";
                string viDo = "0";
                try
                {
                    kinhDo = HttpContext.Current.Request.Params["kinhDo"];
                    viDo = HttpContext.Current.Request.Params["viDo"];
                }
                catch { }
                string nhomAnh = "";
                try
                {
                    nhomAnh = HttpContext.Current.Request.Params["nhomAnh"];
                }
                catch { }
                string donvicha = "";
                if (iddvi.StartsWith("PH")) donvicha = "PH";
                else if (iddvi.StartsWith("PN")) donvicha = "PN";
                else if (iddvi.StartsWith("PM")) donvicha = "PM";
                else donvicha = HttpContext.Current.Request.Params["database"];


                foreach (string fileName in HttpContext.Current.Request.Files)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[fileName];
                    if (file.ContentLength > 0)
                    {
                        string root = HttpContext.Current.Server.MapPath("~/Files");
                        string directory = "";
                        string pathurl = "";
                        DateTime dt = DateTime.Now;
                        //string filename = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString()
                        //    + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString()
                        //    + file.FileName.Substring(file.FileName.LastIndexOf("."));

                        string modificationDate = file.FileName.Split('|')[0];
                        DateTime dtModify = DateTime.Now;
                        try
                        {
                            //dtModify = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(modificationDate)).AddHours(7);
                            dtModify = UnixTimestampToDateTime(modificationDate);
                        }
                        catch
                        {
                            dtModify = DateTime.Now;
                        }
                        string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";

                        long m = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                        int ran = new Random().Next(10000, 60000);
                        long res = m + ran;
                        filename = res + ".png";

                        string strNam = dt.Year.ToString();
                        string strThang = dt.Month.ToString();
                        string strNgay = dt.Day.ToString();
                        if (idphien == -1)
                        {
                            directory = root + "/" + iddvi + "/0";
                            pathurl = "/" + iddvi + "/0/" + filename;
                        }
                        else
                        {
                            directory = root + "/" + iddvi + "/" + idphong + "/" + strNam + "/" + strThang + "/" + strNgay;
                            pathurl = "/" + iddvi + "/" + idphong + "/" + strNam + "/" + strThang + "/" + strNgay + "/" + filename;
                        }

                        if (Directory.Exists(directory) == false)
                            Directory.CreateDirectory(directory);
                        var path = Path.Combine(directory, filename);

                        file.SaveAs(path);

                        if (idphien == -1)
                            insert_image_phien(null, userid, pathurl, kinhDo, viDo, nhomAnh, iddvi, strSoPhieu, donvicha, dtModify);
                        else
                            insert_image_phien(idphien, userid, pathurl, kinhDo, viDo, nhomAnh, iddvi, strSoPhieu, donvicha, dtModify);
                    }
                }

                return Ok(new { success = true, mess = "OK" });
            }
            catch (Exception ex)
            {
                var messages = new List<string>();
                do
                {
                    messages.Add(ex.Message);
                    ex = ex.InnerException;
                }
                while (ex != null);
                var message = string.Join(" - ", messages);

                return Ok(new { success = false, mess = message });
            }
        }

        //[Route("UnixTimestampToDateTime/{unixEpochTime}")]
        //[HttpGet]
        //   public DateTime UnixTimestampToDateTime(string unixEpochTime)
        //   {
        //       var now = DateTime.Now.ToUniversalTime().Subtract(
        //new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        //).TotalMilliseconds;
        //       DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //       if (unixEpochTime.Length < now.ToString().Length)
        //       {
        //           return epoch.AddSeconds(double.Parse(unixEpochTime)).AddHours(7);
        //       }
        //       else
        //           return epoch.AddMilliseconds(double.Parse(unixEpochTime)).AddHours(7);
        //   }

        public DateTime UnixTimestampToDateTime(string unixEpochTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = epoch.AddMilliseconds(double.Parse(unixEpochTime)).AddHours(7);

            if (date.Year > 1972 && date.Year <= DateTime.Now.Year)
                return date;

            return epoch.AddSeconds(double.Parse(unixEpochTime)).AddHours(7);
        }

        private void insert_image_phien(int? idphien, string iduser, string url, string kinhDo, string viDo, string nhomAnh, string iddvi, string strSoPhieu, string database, DateTime ngayChup)
        {
            try
            {
                using (var db = new ECP_PAEntities(GetEntityConnectionString(database)))
                {
                    decimal kd = Decimal.Parse(kinhDo);
                    decimal vd = Decimal.Parse(viDo);
                    int nhom = nhomAnh != "" ? Convert.ToInt32(nhomAnh) : -1;
                    tblImage i = new tblImage
                    {
                        Comment = null,
                        isVideo = 0,
                        NgayCapNhat = DateTime.Now,
                        Note = null,
                        PhienLamViecId = idphien,
                        Url = url,
                        UserUp = iduser,
                        KinhDo = kd,
                        ViDo = vd,
                        GroupId = nhom,
                        NgayChup = ngayChup,
                        IsDelete = false
                    };
                    db.tblImages.Add(i);
                    db.SaveChanges();

                    // Tạm đóng
                    //var phien = (from p in db.tblPhienLamViecs
                    //             where p.Id == idphien
                    //             select p).FirstOrDefault();

                    //var maPCT = phien.MaPCT;
                    //if (maPCT != null)
                    //{
                    //    var phieuCTAC = (from m in db.plv_PhieuCongTac
                    //                     where m.ID == maPCT
                    //                     select m).ToList();
                    //    if (phieuCTAC != null && phieuCTAC.Count() > 0)
                    //    {
                    //        phieuCTAC.ElementAt(0).SoPhieu = strSoPhieu;
                    //    }

                    //    PhieuLamViec_Images phieu = new PhieuLamViec_Images
                    //    {
                    //        MaHA = i.Id,
                    //        MaPCT = maPCT
                    //    };
                    //    db.PhieuLamViec_Images.Add(phieu);
                    //    db.SaveChanges();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("ECP_Login")]
        [HttpPost]
        public object ECP_Login(string username, string password, string dbname)
        {
            //string connect = @"data source=103.63.109.191\MSSQLSERVER2016;initial catalog=ECP_PA25;persist security info=True;user id=sa;password=Vnittech2018;";
            string connect = GetConnectString(dbname);

            var userManager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext(connect)));

            var user = userManager.Find(username, password);
            return user;
        }

        [Route("InsertDevice/{token}/{userid}/{username}/{dbname}")]
        [HttpGet]
        public object InsertDevice(string token, string userid, string username, string dbname)
        {
            try
            {
                using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
                {
                    var kt = (from i in db.tblDevices
                              where i.TOKENKEY == token
                              select i).FirstOrDefault();

                    if (kt != null)
                    {
                        kt.UserID = userid;
                        kt.UserName = username;
                        db.SaveChanges();
                        return 1;
                    }

                    tblDevice dv = new tblDevice();
                    dv.ID = 1;
                    dv.TOKENKEY = token;
                    dv.UserID = userid;
                    dv.UserName = username;
                    db.tblDevices.Add(dv);
                    db.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Route("LayComment")]
        [HttpGet]
        public object LayComment(int id, string dbname)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                var kt = (from i in db.tblComments
                          where i.PhienLamViecId == id
                          select new
                          {
                              i.Id,
                              i.CommentContent,
                              i.CreateTime,
                              ThoiGian = i.CreateTime.Hour + "h" + i.CreateTime.Minute
                                  + "p " + i.CreateTime.Day
                                  + "/" + i.CreateTime.Month + "/" + i.CreateTime.Year,
                              i.Username,
                              i.Priority,
                              i.Description,
                              i.PhienLamViecId,
                          }).AsNoTracking().ToList();
                return kt;
            }
        }

        [Route("insert_taskcmt")]
        [HttpPost]
        public object insert_taskcmt(BinhLuanViewModel param)
        {
            //LayStrConnect(iddvi);
            try
            {
                using (var db = new ECP_PAEntities(GetEntityConnectionString(param.DBName)))
                {
                    tblComment i = new tblComment
                    {
                        PhienLamViecId = param.PhienLamViecId,
                        Username = param.Username,
                        CommentContent = param.CommentContent,
                        CreateTime = DateTime.Now,
                        Priority = 1,
                        Description = "",
                    };
                    db.tblComments.Add(i);
                    db.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("update_NhomAnh")]
        [HttpPost]
        public object update_NhomAnh(UpdateNhomAnhViewModel param)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(param.dbname)))
            {
                var dky = (from i in db.tblImages
                           where i.Id == param.ImageId
                           select i);
                if (dky == null) return 0;
                foreach (var i in dky)
                {
                    i.GroupId = param.GroupId;
                }
                db.SaveChanges();
                return 1;
            }
        }
        [Route("update_commentimg/{id}/{strCmt}/{idNhomAnh}/{dbname}")]
        [HttpGet]
        public object update_commentimg(int id, string strCmt, int idNhomAnh, string dbname)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
            {
                var dky = (from i in db.tblImages
                           where i.Id == id
                           select i);
                if (dky == null) return 0;
                foreach (var i in dky)
                {
                    i.Comment = strCmt;
                    i.GroupId = idNhomAnh;
                }
                db.SaveChanges();
                return 1;
            }
        }

        [Route("update_trangthaiphien")]
        [HttpPost]
        public object update_trangthaiphien(tblPhienLamViec param)
        {
            try
            {
                using (var db = new ECP_PAEntities(GetEntityConnectionString(param.dbname)))
                {
                    var dky = (from i in db.tblPhienLamViecs
                               where i.Id == param.Id
                               select i);
                    if (dky == null) return 0;
                    foreach (var i in dky)
                    {
                        i.TrangThai = 3;
                        i.NgayKetThuc = DateTime.Now;
                        i.NguoiKetThuc = param.NguoiKetThuc;
                    }
                    db.SaveChanges();
                    return 1;
                }
            }
            catch (Exception e)
            {
                String err = e.Message;
                return 0;
            }
        }

        [Route("LayGroupImage")]
        [HttpGet]
        public Object LayGroupImage()
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString("PA04")))
            {
                var kt = (from j in db.tblGroupImages
                          select new
                          {
                              j.Id,
                              j.MoTa,
                              j.NoiDung,
                              j.ThuTu
                          }).OrderBy(c => c.ThuTu).AsNoTracking().ToList();
                return kt;
            }
        }

        [Route("GetTinhChatPhien")]
        [HttpGet]
        public object GetTinhChatPhien()
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString("PA04")))
            {
                var kq = (from p in db.plv_TinhChatPhien
                          where p.IsActive == true
                          select new
                          {
                              p.Id,
                              Name = p.Name.Replace("CÔNG VIỆC THEO ", "")
                          }).OrderBy(c => c.Id).ToList();
                var m = new
                {
                    Id = -1,
                    Name = "Tất cả"
                };
                kq.Add(m);

                var result = (from x in kq
                              select x).OrderBy(c => c.Id);
                return result;
            }
        }

        [HttpGet]
        public object GetThuocTinhPhien()
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString("PA04")))
            {
                var kq = (from p in db.plv_ThuocTinhPhien
                          where p.TrangThai == true
                          select new
                          {
                              p.Id,
                              p.LoaiThuocTinh,
                              p.MoTa,
                              p.TenThuocTinh
                          }).ToList().OrderBy(c => c.Id);
                return kq;
            }
        }

        [HttpGet]
        public object GetTrangThaiPhien()
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString("PA04")))
            {
                var kq = (from p in db.plv_TrangThaiPhien
                          where p.IsActive == true
                          select new
                          {
                              p.Id,
                              p.Name
                          }).OrderBy(c => c.Id).ToList();
                var m = new
                {
                    Id = -1,
                    Name = "Tất cả"
                };
                kq.Add(m);
                kq.OrderBy(c => c.Id);
                return kq;
            }
        }

        [HttpGet]
        public object GetTrangThaiPhieu()
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString("PA04")))
            {
                var kq = (from p in db.plv_TrangThaiPhieu
                          select new
                          {
                              p.MaTT,
                              p.TenTT
                          }).ToList().OrderBy(c => c.MaTT);
                return kq;
            }
        }

        public object SendMsgDevice(string username, string title, string mess, string dbname)
        {
            try
            {
                using (var db = new ECP_PAEntities(GetEntityConnectionString(dbname)))
                {
                    // lay danh sach device
                    var device = (from i in db.tblDevices
                                  where i.UserName == username
                                  select i).AsNoTracking().ToList();
                    List<string> lstToken = device.Select(c => c.TOKENKEY).ToList();
                    if (device == null) return 0;
                    var client = new OneSignalClient("NGM4YmQ3OTgtNGRhMy00OWExLWE2MTktMjFiNTE3ZmJkMmFk"); // Nhập API Serect Key

                    var options = new NotificationCreateOptions();

                    options.AppId = new Guid("7acac125-c963-4e2f-9fdb-45801ec015ad"); // Nhập My AppID của bạn
                    options.IncludePlayerIds = lstToken;
                    options.Headings.Add(LanguageCodes.English, title);
                    options.Contents.Add(LanguageCodes.English, mess);
                    var result = client.Notifications.Create(options);
                }
            }
            catch (Exception e)
            {
            }
            return 1;
        }

        private void SaveOptimizeImage(HttpPostedFile file, int maxWidth, int maxHeight, int quality, string filePath)
        {
            try
            {
                using (var image = new Bitmap(file.InputStream))
                {
                    // Get the image's original width and height
                    int originalWidth = image.Width;
                    int originalHeight = image.Height;

                    // To preserve the aspect ratio
                    float ratioX = (float)maxWidth / (float)originalWidth;
                    float ratioY = (float)maxHeight / (float)originalHeight;
                    float ratio = Math.Min(ratioX, ratioY);

                    // New width and height based on aspect ratio
                    int newWidth = (int)(originalWidth * ratio);
                    int newHeight = (int)(originalHeight * ratio);

                    // Convert other formats (including CMYK) to RGB.
                    using (var newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format64bppArgb))
                    {
                        // Draws the image in the specified size with quality mode set to HighQuality
                        using (Graphics graphics = Graphics.FromImage(newImage))
                        {
                            graphics.CompositingQuality = CompositingQuality.HighQuality;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;
                            graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                        }
                    }

                    // Get an ImageCodecInfo object that represents the JPEG codec.
                    ImageCodecInfo imageCodecInfo = this.GetEncoderInfo(ImageFormat.Jpeg);

                    // Create an Encoder object for the Quality parameter.
                    System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;

                    // Create an EncoderParameters object. 
                    EncoderParameters encoderParameters = new EncoderParameters(1);

                    // Save the image as a JPEG file with quality level.
                    EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
                    encoderParameters.Param[0] = encoderParameter;
                    // newImage.Save(filePath, imageCodecInfo, encoderParameters);
                    image.Save(filePath, imageCodecInfo, encoderParameters);
                }
            }
            catch
            { }

            //Bitmap image = new Bitmap(file.InputStream);
            //// Get the image's original width and height
            //int originalWidth = image.Width;
            //int originalHeight = image.Height;

            //// To preserve the aspect ratio
            //float ratioX = (float)maxWidth / (float)originalWidth;
            //float ratioY = (float)maxHeight / (float)originalHeight;
            //float ratio = Math.Min(ratioX, ratioY);

            //// New width and height based on aspect ratio
            //int newWidth = (int)(originalWidth * ratio);
            //int newHeight = (int)(originalHeight * ratio);

            //// Convert other formats (including CMYK) to RGB.
            //Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format64bppArgb);

            //// Draws the image in the specified size with quality mode set to HighQuality
            //using (Graphics graphics = Graphics.FromImage(newImage))
            //{
            //    graphics.CompositingQuality = CompositingQuality.HighQuality;
            //    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    graphics.SmoothingMode = SmoothingMode.HighQuality;
            //    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            //}

            //// Get an ImageCodecInfo object that represents the JPEG codec.
            //ImageCodecInfo imageCodecInfo = this.GetEncoderInfo(ImageFormat.Jpeg);


            //// Create an Encoder object for the Quality parameter.
            //System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;

            //// Create an EncoderParameters object. 
            //EncoderParameters encoderParameters = new EncoderParameters(1);

            //// Save the image as a JPEG file with quality level.
            //EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
            //encoderParameters.Param[0] = encoderParameter;
            //// newImage.Save(filePath, imageCodecInfo, encoderParameters);
            //image.Save(filePath, imageCodecInfo, encoderParameters);
        }

        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }

        [HttpGet]
        // khoi tao lay 20 cai anh gan day nhat cua don vi
        public object Get20Images(int idphong, string iddvi)
        {
            //LayStrConnect(iddvi);
            using (var db = new ECP_PAEntities(GetConnectString(iddvi)))
            {
                var kt = (from i in db.tblPhongBans
                          join g in db.tblPhienLamViecs
                          on i.Id equals g.PhongBanID
                          join u in db.tblImages
                          on g.Id equals u.PhienLamViecId
                          where i.Id == idphong
                          select u).Union
                      (from g in db.tblPhongBans
                       join t in db.tblNhanViens
                       on g.Id equals t.PhongBanId
                       join uu in db.tblImages
                       on t.Id equals uu.UserUp
                       where g.Id == idphong
                       select uu
                ).OrderByDescending(g => g.Id).Take(20).ToList().Select(u => new
                {
                    u.Id,
                    u.Url,
                    NgayCapNhat = u.NgayCapNhat.ToString("dd/MM/yyy"),
                    u.Note,
                    u.Comment,
                }).ToList();

                return kt;
            }
        }
        [HttpGet]
        // lay anh voi id max nhat cua thiet bi
        public object GetImagesMax(int idphong, int idanhmax, string iddvi)
        {
            //LayStrConnect(iddvi);
            //DateTime dt = DateTime.ParseExact(ngaymax, "dd-MM-yyyy", CultureInfo.InvariantCulture);  
            using (var db = new ECP_PAEntities(GetConnectString(iddvi)))
            {
                var kt = (from i in db.tblPhongBans
                          join g in db.tblPhienLamViecs
                          on i.Id equals g.PhongBanID
                          join u in db.tblImages
                          on g.Id equals u.PhienLamViecId
                          where i.Id == idphong
                          && u.Id > idanhmax
                          orderby u.Id descending
                          select u).Union
                      (from g in db.tblPhongBans
                       join t in db.tblNhanViens
                       on g.Id equals t.PhongBanId
                       join uu in db.tblImages
                       on t.Id equals uu.UserUp
                       where g.Id == idphong
                       && uu.Id > idanhmax
                       select uu
                ).OrderByDescending(g => g.Id).ToList().Select(u => new
                {
                    u.Id,
                    u.Url,
                    NgayCapNhat = u.NgayCapNhat.ToString("dd/MM/yyy"),
                    u.Note,
                    u.Comment,
                }).ToList();
                return kt;
            }
        }
        [HttpGet]
        // lay anh voi id min nhat cua thiet bi
        public object GetImagesMin(int idphong, int idmin, string iddvi)
        {
            //LayStrConnect(iddvi);
            //DateTime dt = DateTime.ParseExact(ngaymax, "dd-MM-yyyy", CultureInfo.InvariantCulture);        
            using (var db = new ECP_PAEntities(GetConnectString(iddvi)))
            {
                var kt = (from i in db.tblPhongBans
                          join g in db.tblPhienLamViecs
                          on i.Id equals g.PhongBanID
                          join u in db.tblImages
                          on g.Id equals u.PhienLamViecId
                          where i.Id == idphong
                          && u.Id < idmin
                          orderby u.Id descending
                          select u).Union
                      (from g in db.tblPhongBans
                       join t in db.tblNhanViens
                       on g.Id equals t.PhongBanId
                       join uu in db.tblImages
                       on t.Id equals uu.UserUp
                       where g.Id == idphong
                        && uu.Id < idmin
                       select uu
                ).OrderByDescending(g => g.Id).Take(20).ToList().Select(u => new
                {
                    u.Id,
                    u.Url,
                    NgayCapNhat = u.NgayCapNhat.ToString("dd/MM/yyy"),
                    u.Note,
                    u.Comment,
                }).ToList();
                return kt;
            }
        }

        [HttpGet]
        public object GetImageByDate(int idphong, string date, string iddvi)
        {
            //LayStrConnect(iddvi);
            using (var db = new ECP_PAEntities(GetConnectString(iddvi)))
            {
                DateTime dt = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var kt = (from i in db.tblPhongBans
                          join g in db.tblPhienLamViecs
                          on i.Id equals g.PhongBanID
                          join u in db.tblImages
                          on g.Id equals u.PhienLamViecId
                          where i.Id == idphong
                          && DbFunctions.TruncateTime(u.NgayCapNhat) == dt
                          orderby u.Id descending
                          select new
                          {
                              u.Id,
                              u.Url,
                              u.NgayCapNhat,
                              u.Note,
                              u.Comment
                          }).Union
                         (from g in db.tblPhongBans
                          join t in db.tblNhanViens
                          on g.Id equals t.PhongBanId
                          join uu in db.tblImages
                          on t.Id equals uu.UserUp
                          where g.Id == idphong
                           && DbFunctions.TruncateTime(uu.NgayCapNhat) == dt
                          select new
                          {
                              uu.Id,
                              uu.Url,
                              uu.NgayCapNhat,
                              uu.Note,
                              uu.Comment
                          }
                   ).OrderByDescending(g => g.Id).ToList().Select(u => new
                   {
                       u.Id,
                       u.Url,
                       NgayCapNhat = u.NgayCapNhat.ToString("dd/MM/yyy"),
                       u.Note,
                       u.Comment,
                   }).ToList();
                return kt;
            }
        }

        [HttpGet]
        public int CheckImagePhien(int idphien, string iddvi)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(iddvi)))
            {
                var a = (from i in db.tblImages
                         where i.PhienLamViecId == idphien
                         select i).Count();
                return a;
            }
        }

        [HttpGet]
        public Object gettendvi(string id, string iddvi)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(iddvi)))
            {
                var kt = (from j in db.tblDonVis
                          where j.Id == id
                          select j.TenDonVi).FirstOrDefault();

                if (kt == null)
                {
                    return "0";
                }
                return kt;
            }
        }

        [HttpGet]
        public Object gettenphong(int id, string iddvi)
        {
            using (var db = new ECP_PAEntities(GetEntityConnectionString(iddvi)))
            {
                var kt = (from j in db.tblPhongBans
                          where j.Id == id
                          select j.TenPhongBan).FirstOrDefault();

                if (kt == null)
                {
                    return "0";
                }
                return kt;
            }
        }

        /// <summary>
        /// Gui push cho khach hang
        /// </summary>
        /// <param name="id">id nhan vien</param>
        /// <param name="mess">noi dung ca push</param>    
        //[HttpGet]
        //public object SendMsgDevice(string id, string mess)
        //{
        //    try
        //    {
        //        using (var db = new ECP_PAEntities())
        //        {
        //            // lay danh sach device
        //            var t = (from i in db.tblDevices
        //                     where i.ID == id
        //                     select i).AsNoTracking().ToList();

        //            string applicationID = "AIzaSyD99zqBX_h6Swz2bny7DVNEr7PkHhdKtuI";
        //            string[] arrRegid = t.Select(d => d.TOKENKEY).ToArray();
        //            string SENDER_ID = "948568754667";

        //            WebRequest tRequest;
        //            //thiết lập GCM send
        //            tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
        //            tRequest.Method = "POST";
        //            tRequest.UseDefaultCredentials = true;

        //            tRequest.PreAuthenticate = true;

        //            tRequest.Credentials = CredentialCache.DefaultNetworkCredentials;

        //            //định dạng JSON
        //            tRequest.ContentType = "application/json";

        //            tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));

        //            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

        //            string RegArr = string.Empty;

        //            RegArr = string.Join("\",\"", arrRegid);

        //            string postData = "{ \"registration_ids\": [ \"" + RegArr + "\" ],\"data\": {\"message\": \"" + mess + "\",\"collapse_key\":\"" + mess + "\"}}";

        //            Console.WriteLine(postData);
        //            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //            tRequest.ContentLength = byteArray.Length;

        //            Stream dataStream = tRequest.GetRequestStream();
        //            dataStream.Write(byteArray, 0, byteArray.Length);
        //            dataStream.Close();

        //            WebResponse tResponse = tRequest.GetResponse();

        //            dataStream = tResponse.GetResponseStream();

        //            StreamReader tReader = new StreamReader(dataStream);

        //            String sResponseFromServer = tReader.ReadToEnd();
        //            Console.Write(sResponseFromServer);
        //            tReader.Close();
        //            dataStream.Close();
        //            tResponse.Close();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { ex.Message });
        //    }
        //}
    }
    public class ConnectionStringObj
    {
        public string DBName;
        public string ConnectionString;
    }
}
