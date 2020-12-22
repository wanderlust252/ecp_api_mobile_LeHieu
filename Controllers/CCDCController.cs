using Dapper;
using ECP_WEBAPI.Context;
using ECP_WEBAPI.Models;
using ECP_WEBAPI.Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http; 

namespace ECP_WEBAPI.Controllers
{
    public class CCDCController : ApiController
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

        public string GetEntityConnectionString(string connectionString)
        {
            var entityBuilder = new EntityConnectionStringBuilder();
            //data source = DESKTOP - FSCTC81\SQLEXPRESS; initial catalog = ECP_PA04; integrated security = False; persist security info = True; user id = sa; password = 123456
            entityBuilder.Provider = "System.Data.SqlClient";
            entityBuilder.ProviderConnectionString = GetConnectString(connectionString) + ";MultipleActiveResultSets=True;App=EntityFramework;";
            entityBuilder.Metadata = @"res://*/Models.EF.ECP_Model.csdl|res://*/Models.EF.ECP_Model.ssdl|res://*/Models.EF.ECP_Model.msl";

            return entityBuilder.ToString();
        }
        [Route("SoKiemDinh")]
        [HttpGet]
        public async Task<object> getListSoKiemDinh( string CongCuId, string dbname)
        { 


            /*
             1. Hết hạn
             2. 15 ngày
             3. 30 ngày
             4. Chưa đến hạn
            */

            List<CongCuDungCuAnToanModel> lstData = new List<CongCuDungCuAnToanModel>();
            try
            {

                using (SqlConnection db = new SqlConnection(GetConnectString(dbname)))
                {
                    string query = @"select * from SoTheoDoiCCDCAT where MaTB= @CongCuId" ;
                        
                    return (await db.QueryAsync<object>(query, new { CongCuId })).ToList();
                }
            }
            catch (Exception ex) { }
            return lstData;
        }
        [Route("CCDC/{page}/{pageSize}/{filter}/{DonViId}/{PhongBanId}/{TrangThai}/{MaLoai}/{TrangThaiKiemDinh}/{MaNhom}/{MaTT}/{dbname}")]
        [HttpGet]
        public object getListCongCu(int page, int pageSize, string filter,
            string DonViId, string PhongBanId, string TrangThai, string MaLoai, string TrangThaiKiemDinh, string MaNhom, string MaTT,string dbname)
        {
            if (filter == "null")
                filter = "";
            if (DonViId == "null")
                DonViId = "";
            if (PhongBanId == "null")
                PhongBanId = "";
            if (TrangThai == "null")
                TrangThai = "";
            if (MaLoai == "null")
                MaLoai = "";
            if (TrangThaiKiemDinh == "null")
                TrangThaiKiemDinh = "";
            if (MaNhom == "null")
                MaNhom = "";
            if (MaTT == "null")
                MaTT = "";


            /*
             1. Hết hạn
             2. 15 ngày
             3. 30 ngày
             4. Chưa đến hạn
            */

            List<CongCuDungCuAnToanModel> lstData = new List<CongCuDungCuAnToanModel>();
            try
            {

                using (SqlConnection db = new SqlConnection(GetConnectString(dbname)))
                {
                    string query =
                        "select * from ( " +
                        "select ROW_NUMBER() OVER (ORDER BY tb.[ID]) AS RowNum " +
                        ",tb.ID,tb.TenThietBi,tb.MaHieu,tb.QuyTacDanhMa " +
                        ",TenHangSX=(select Name from HangSanXuat where ID=tb.MaHSX) " +
                        ",TenNuocSX=(select Name from NuocSanXuat where ID=tb.MaNSX) " +
                        ",tb.NamSX,tb.NgayDuaVaoSuDung " +
                        ",TenPB=(select TenPhongBan from tblPhongBan where Id=tb.PhongBanID) " +
                        ",tb.PhongBanID,tb.DonViId,tb.NgayTao,tb.NguoiTao,tb.NgaySua,tb.NguoiSua,tb.HanKiemDinh " +
                        ",NgayKiemTra=(select top(1) NgayKiemTra from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc) " +
                        ",GhiChu=(select top(1) GhiChu from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc) " +
                        ",NgayKiemTraTiepTheo= (select top(1) NgayKiemTraTiepTheo from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc) " +
                        ",TenTT=(select Name from CCDC_TrangThai where ID=tb.MaTT) " +
                        ",TrangThai= " +
                        "( " +
                        "case when " +
                        "(DATEDIFF(d, (select top(1) NgayKiemTraTiepTheo from SoTheoDoiCCDCAT where MaTB = tb.ID order by ID desc), getdate()) > 0) " +
                        "then 1 " +
                        "else " +
                        "(case when(DATEDIFF(d,getdate(),(select top(1) NgayKiemTraTiepTheo from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc)) <=15 " +
                        "and DATEDIFF(d,getdate(),(select top(1) NgayKiemTraTiepTheo from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc)) >=0)  " +
                        "then 2 " +
                        "else " +
                        "(case when(DATEDIFF(d,getdate(),(select top(1) NgayKiemTraTiepTheo from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc)) <=30 " +
                        "and DATEDIFF(d,getdate(),(select top(1) NgayKiemTraTiepTheo from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc)) >15)  " +
                        "then 3 " +
                        "else " +
                        "(case when DATEDIFF(d,getdate(),(select top(1) NgayKiemTraTiepTheo from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc)) >30 " +
                        "then 4 else 5 " +
                        "end) " +
                        "end) " +
                        "end) " +
                        "end " +
                        ") " +
                        "from ThietBiATLD tb " +
                        "where " +
                        "((tb.TenThietBi like REPLACE(N'%'+(N'" + filter + "')+N'%',' ',N'%') or N'" + filter + "'=N'') " +
                        "or (tb.MaHieu like REPLACE(N'%'+(N'" + filter + "')+N'%',' ',N'%') or N'" + filter + "'=N'')) " +
                        "and (tb.DonViId=@DonViId or @DonViId='') " +
                        "and (tb.PhongBanID=@PhongBanId or @PhongBanId='') " +
                        "and isnull(tb.IsDelete,0)=0 " +
                        "and tb.MaNhom=@MaNhom " +
                        "and (tb.MaLoai=@MaLoai or @MaLoai='') " +
                        "and (tb.MaTT=@MaTT or @MaTT='') "
                        ;

                    // het han
                    if (TrangThai == "hh")
                    {
                        query = query + "and ( " +
                            "DATEDIFF(d,(select top(1) NgayKiemTraTiepTheo from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc),getdate()) >0 " +
                            ") "
                            ;
                    }
                    //sap het han 15 ngay
                    else if (TrangThai == "shh15")
                    {
                        query = query + "and ( " +
                            "DATEDIFF(d,getdate(),(select top(1) NgayKiemTraTiepTheo from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc)) <=15 " +
                            "and DATEDIFF(d,getdate(),(select top(1) NgayKiemTraTiepTheo from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc)) >=0 " +
                            ") "
                            ;
                    }
                    //sap het han 30 ngay
                    else if (TrangThai == "shh30")
                    {
                        query = query + "and ( " +
                            "DATEDIFF(d,getdate(),(select top(1) NgayKiemTraTiepTheo from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc)) <=30 " +
                            "and DATEDIFF(d,getdate(),(select top(1) NgayKiemTraTiepTheo from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc)) >15 " +
                            ") "
                            ;
                    }
                    //chua het han
                    else if (TrangThai == "cdh")
                    {
                        query = query + "and ( " +
                            "DATEDIFF(d,getdate(),(select top(1) NgayKiemTraTiepTheo from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc)) >30 " +
                            ") "
                            ;
                    }

                    //trang thai kiem dinh dat
                    if (TrangThaiKiemDinh == "1")
                    {
                        query = query + "and ( " +
                            "(select top(1) isnull(KetQua,0) from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc) =1 " +
                            ") "
                            ;
                    }
                    //trang thai kiem dinh khong dat
                    else if (TrangThaiKiemDinh == "2")
                    {
                        query = query + "and ( " +
                            "(select top(1) isnull(KetQua,0) from SoTheoDoiCCDCAT where MaTB=tb.ID order by ID desc) =0 " +
                            ") "
                            ;
                    }
                    //ko co kiem dinh
                    else if (TrangThaiKiemDinh == "3")
                    {
                        query = query + "and ( " +
                            "(select count(ID) from SoTheoDoiCCDCAT where MaTB=tb.ID) =0 " +
                            ") "
                            ;
                    }


                    query = query +
                        ") as kq " +
                        "where RowNum BETWEEN ((@page-1)*@pageSize)+1 and @page*@pageSize " +
                        "order by TrangThai "
                        ;

                    using (var multipleresult = db.QueryMultiple(query,
                        new
                        {
                            page = page,
                            pageSize = pageSize,
                            filter = DonViId,
                            PhongBanId = PhongBanId,
                            DonViId = DonViId,
                            MaLoai = MaLoai,
                            MaNhom = MaNhom,
                            MaTT = MaTT
                        }))
                    {
                        var q = multipleresult.Read<CongCuDungCuAnToanModel>();
                        lstData = q.ToList();
                    }
                }
            }
            catch (Exception ex) { }
            return lstData;
        }
    }
    public class ConnectionStringObj
    {
        public string DBName;
        public string ConnectionString;
    }
}