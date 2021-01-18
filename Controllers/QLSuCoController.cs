using Dapper;
using ECP_WEBAPI.Context;
using ECP_WEBAPI.Models;
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
    public class QLSuCoController : ApiController
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

        public string GetEntityConnectionString(string connectionString, int optionDapper = 0)
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
        [Route("CountListPagingSuCo")]
        [HttpGet]
        public object CountListPaging(string filter, string TuNgay, string DenNgay,
           string DonViId, string PhongBanId, string LoaiSuCo, string TinhChat, string NguyenNhan, string TrangThaiNhap,
           string MienTru, string KienNghi, string TCTDuyetMT, string CapDienAp, string LoaiTaiSan, 
           bool IsThongKe, string TrangThaiChuyenNPC, string dbname)
        {
            string start1 = "";
            string end1 = "";

            if (string.IsNullOrEmpty(TuNgay))
            {

            }
            else
            {
                DateTime dts = DateTime.ParseExact(TuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                start1 = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dts.Date);
            }

            if (string.IsNullOrEmpty(DenNgay))
            {

            }
            else
            {
                DateTime dte = DateTime.ParseExact(DenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                end1 = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dte.Date);
            }

            int count = 0;
            try
            {

                using (SqlConnection db = new SqlConnection(GetConnectString(dbname)))
                {
                    string query = "";

                    #region query1
                    query =
                        "select Count(Id) " +
                        "from sc_TaiNanSuCo a " +
                        "where " +
                        "(a.TomTat like REPLACE(N'%'+(N'" + filter + "')+N'%',' ',N'%') or N'" + filter + "'=N'') " +
                        //"and (tb.PhongBanID=@PhongBanId or @PhongBanId='') " +
                        "and (CONVERT(date,a.ThoiGianXuatHien) >= CONVERT(date,@TuNgay) or @TuNgay='') " +
                        "and (CONVERT(date,a.ThoiGianXuatHien) <= CONVERT(date,@DenNgay) or @DenNgay='') "
                        //"and (a.LoaiSuCoId=@LoaiSuCo or @LoaiSuCo='' or a.LoaiSuCoId is null)"
                        ;

                    //if (IsThongKe)
                    //{
                    query = query + " and (@DonViId in (select c.DonViId from sc_TaiNanSuCo_DonVi c where c.SuCoId = a.Id) " +
                    "or @DonViId='') ";
                    //}
                    //else
                    //{
                    //    query = query + " and (a.DonViId=@DonViId " +
                    //    "or @DonViId in (select c.DonViId from sc_TaiNanSuCo_DonVi c where c.SuCoId = a.Id) " +
                    //    "or @DonViId in (select c.DonViId from tblNhanVien c where c.Username=a.NguoiTao) " +
                    //    "or @DonViId='') ";
                    //}

                    if (LoaiSuCo == "all" || string.IsNullOrEmpty(LoaiSuCo))
                    {

                    }
                    else
                    {

                        query = query + "and a.LoaiSuCoId=@LoaiSuCo ";

                    }

                    if (TinhChat == "all" || string.IsNullOrEmpty(TinhChat))
                    {

                    }
                    else
                    {

                        query = query + "and a.TinhChatId=@TinhChat ";

                    }

                    if (NguyenNhan == "all" || string.IsNullOrEmpty(NguyenNhan))
                    {

                    }
                    else
                    {
                        query = query + "and a.NguyenNhanId=@NguyenNhan ";
                    }

                    if (MienTru == "all" || string.IsNullOrEmpty(MienTru))
                    {

                    }
                    else
                    {
                        if (bool.Parse(MienTru))
                            query = query + "and a.IsMienTru=1 ";
                        else
                            query = query + "and isnull(a.IsMienTru,0)=0 ";
                    }

                    if (KienNghi == "all" || string.IsNullOrEmpty(KienNghi))
                    {

                    }
                    else if (bool.Parse(KienNghi))
                    {
                        query = query + "and a.KienNghiId is not null ";
                    }
                    else if (!bool.Parse(KienNghi))
                    {
                        query = query + "and a.KienNghiId is null ";
                    }

                    if (!string.IsNullOrEmpty(TrangThaiNhap))
                    {
                        if (TrangThaiNhap == "tre")
                            query = query + "and DATEDIFF(hour, a.ThoiGianXuatHien, a.NgayTao)>24 ";
                        else if (TrangThaiNhap == "dunghan")
                            query = query + "and DATEDIFF(hour, a.ThoiGianXuatHien, a.NgayTao)<=24 ";
                    }

                    if (TCTDuyetMT == "all" || string.IsNullOrEmpty(TCTDuyetMT))
                    {

                    }
                    else if (bool.Parse(TCTDuyetMT))
                    {
                        query = query + "and a.NPCIsDuyetMT=1 ";
                    }
                    else if (!bool.Parse(TCTDuyetMT))
                    {
                        query = query + "and a.NPCIsDuyetMT=0 ";
                    }

                    if (!string.IsNullOrEmpty(CapDienAp))
                    {
                        query = query + "and a.CapDienAp=@CapDienAp ";
                    }

                    if (!string.IsNullOrEmpty(LoaiTaiSan))
                    {
                        if (LoaiTaiSan == "dienluc")
                            query = query + "and isnull(a.IsTaiSan,0)=1 ";
                        else if (LoaiTaiSan == "khachhang")
                            query = query + "and isnull(a.IsTaiSan,0)=0 ";
                    }

                    if (!string.IsNullOrEmpty(TrangThaiChuyenNPC))
                    {
                        if (TrangThaiChuyenNPC == "dachuyen")
                            query = query + "and isnull(a.IsChuyenNPC,0)=1 ";
                        else if (TrangThaiChuyenNPC == "chuachuyen")
                            query = query + "and isnull(a.IsChuyenNPC,0)=0 ";
                    }

                    #endregion

                    using (var multipleresult = db.QueryMultiple(query,
                        new
                        {
                            filter = DonViId,
                            TuNgay = start1,
                            DenNgay = end1,
                            PhongBanId = PhongBanId,
                            DonViId = DonViId,
                            LoaiSuCo = LoaiSuCo,
                            TinhChat = TinhChat,
                            NguyenNhan = NguyenNhan,
                            MienTru = MienTru,
                            CapDienAp = CapDienAp,
                        }))
                    {
                        try
                        {
                            var q = multipleresult.Read<int>();
                            count = q.FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            count = 0;
                        }


                    }
                }
            }
            catch (Exception ex) { }
            return count;
        }
        [Route("GetFilterQlSuCo")]
        [HttpGet]
        public async Task<object> GetFilterQlSuCo( string dbname)
        { 
            try
            {
                using (SqlConnection db = new SqlConnection(GetConnectString(dbname)))
                {
                    string query = @"SELECT * FROM [sc_LoaiSuCo] order by [TypeOfLSC]";
                    List<QuanLySuCoFilterViewModel> lst = (await db.QueryAsync<QuanLySuCoFilterViewModel>(query,
                    new
                    {
                        
                    })).ToList();
                    return lst;
                }
            }
            catch (Exception ex) { }
            return new List<object>() { };
        }
    

        [Route("GetListSuCo")]
        [HttpGet]
        public object getListSuCo(int page, int pageSize,  string TuNgay , string DenNgay , string filter = "",
           string DonViId = "", string PhongBanId = "", string LoaiSuCo = "", string TinhChat = "", string NguyenNhan = "", string TrangThaiNhap = "",
           string MienTru = "", string KienNghi = "", string TCTDuyetMT = "", string CapDienAp = "", string LoaiTaiSan = "", 
           string TrangThaiChuyenNPC="",string dbname = "")
        {
            string start1 = "";
            string end1 = "";
            if (filter == "null")
                filter = "";
            if (DonViId == "null")
                DonViId = "";
            if (PhongBanId == "null")
                PhongBanId = "";
            if (LoaiSuCo == "null")
                LoaiSuCo = "";
            if (TinhChat == "null")
                TinhChat = "";
            if (NguyenNhan == "null")
                NguyenNhan = "";
            if (TrangThaiNhap == "null")
                TrangThaiNhap = "";
            if (TrangThaiChuyenNPC == "null")
                TrangThaiChuyenNPC = "";
            if (MienTru == "null")
                MienTru = "";
            if (KienNghi == "null")
                KienNghi = "";
            if (TCTDuyetMT == "null")
                TCTDuyetMT = "";
            if (CapDienAp == "null")
                CapDienAp = "";
            if (LoaiTaiSan == "null")
                LoaiTaiSan = "";
            if (string.IsNullOrEmpty(TuNgay))
            {

            }
            else
            {
                DateTime dts = DateTime.ParseExact(TuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                start1 = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dts.Date);
            }

            if (string.IsNullOrEmpty(DenNgay))
            {

            }
            else
            {
                DateTime dte = DateTime.ParseExact(DenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                end1 = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dte.Date);
            }

            List<SuCoModel> lstData = new List<SuCoModel>();
            try
            {

                using (SqlConnection db = new SqlConnection(GetConnectString(dbname)))
                {
                    string query = "";

                    #region query1
                    query =
                        "select * from ( " +
                        "select ROW_NUMBER() OVER (ORDER BY a.[Id]) AS RowNum " +
                        ",a.Id, a.DonViId,a.CapDienAp,a.TomTat,a.TinhTrangBienBan,a.HinhAnhSuCo,a.ThoiGianXuatHien" +
                        ",a.ThoiGianBatDauKhacPhuc,a.ThoiGianKhacPhucXong,a.ThoiGianKhoiPhuc,a.T_XuatHienBatDauKhacPhuc" +
                        ",a.T_BatDauDenKhacPhucXong,a.T_KhacPhucXongDenKhoiPhuc,a.T_TongThoiGianMatDien,a.NgayTao,a.NguoiTao,a.PhieuCongTacId " +
                        ",LoaiSuCo =(select TenLoaiSuCo from sc_LoaiSuCo where Id = a.LoaiSuCoId) " +
                        ",NguyenNhan =(select TenLoaiSuCo from sc_LoaiSuCo where Id = a.NguyenNhanId) " +
                        ",TinhChat =(select TenLoaiSuCo from sc_LoaiSuCo where Id = a.TinhChatId) " +
                        ",TenDvi =(select TenDonVi from tblDonVi where Id = a.DonViId) " +
                        ",a.TrangThai, a.NgayDuyet, a.NguoiDuyet, a.NgaySua, a.NguoiSua, a.TenThietBi,a.DienBienSuCo,a.IsGianDoan, a.IsTaiSan " +
                        ",TrangThaiNhap=case when DATEDIFF(hour, a.ThoiGianXuatHien, a.NgayTao)>24 then N'Trễ' else N'' end " +
                        ",a.IsChuyenNPC,a.NgayDuyetNPC,a.NguoiDuyetNPC, a.IsMienTru " +
                        ",a.NPCIsDuyetMT,a.NPCNgayDuyetMT,a.NPCNguoiDuyetMT,a.NPCTenNguoiDuyetMT,a.NPCCommentMT " +
                        ",NguoiKienNghi=(select top(1) NguoiTao from sc_KienNghiMienTru where SuCoId=a.Id order by Id desc) " +
                        ",NgayKienNghi=(select top(1) NgayTao from sc_KienNghiMienTru where SuCoId=a.Id order by Id desc) " +
                        ",NoiDungKienNghi=(select top(1) NoiDung from sc_KienNghiMienTru where SuCoId=a.Id order by Id desc) " +
                        ",a.KienNghiId " +
                        ",lstDonViSuCoId=((SELECT  STUFF ((SELECT ',' + Rtrim( c.TenDV ) FROM sc_TaiNanSuCo_DonVi c WHERE c.SuCoId = a.Id FOR XML PATH ('')), 1,1 ,''))) " +
                        "from sc_TaiNanSuCo a " +
                        "where " +
                        "(a.TomTat like REPLACE(N'%'+(N'" + filter + "')+N'%',' ',N'%') or N'" + filter + "'=N'') " +
                        //"and (a.DonViId=@DonViId " +
                        "and (@DonViId in (select c.DonViId from sc_TaiNanSuCo_DonVi c where c.SuCoId = a.Id) " +
                        //"or @DonViId in (select c.DonViId from tblNhanVien c where c.Username=a.NguoiTao) " +
                        "or @DonViId='') " +
                        //"and (tb.PhongBanID=@PhongBanId or @PhongBanId='') " +
                        "and (CONVERT(date,a.ThoiGianXuatHien) >= CONVERT(date,@TuNgay) or @TuNgay='') " +
                        "and (CONVERT(date,a.ThoiGianXuatHien) <= CONVERT(date,@DenNgay) or @DenNgay='') "
                        //"and (a.LoaiSuCoId=@LoaiSuCo or @LoaiSuCo='' or a.LoaiSuCoId is null)"
                        ;

                    if (LoaiSuCo == "all" || string.IsNullOrEmpty(LoaiSuCo))
                    {

                    }
                    else
                    {

                        query = query + "and a.LoaiSuCoId=@LoaiSuCo ";

                    }

                    if (TinhChat == "all" || string.IsNullOrEmpty(TinhChat))
                    {

                    }
                    else
                    {

                        query = query + "and a.TinhChatId=@TinhChat ";

                    }

                    if (NguyenNhan == "all" || string.IsNullOrEmpty(NguyenNhan))
                    {

                    }
                    else
                    {
                        query = query + "and a.NguyenNhanId=@NguyenNhan ";
                    }

                    if (MienTru == "all" || string.IsNullOrEmpty(MienTru))
                    {

                    }
                    else
                    {
                        if (bool.Parse(MienTru))
                            query = query + "and a.IsMienTru=1 ";
                        else
                            query = query + "and isnull(a.IsMienTru,0)=0 ";
                    }

                    if (KienNghi == "all" || string.IsNullOrEmpty(KienNghi))
                    {

                    }
                    else if (bool.Parse(KienNghi))
                    {
                        query = query + "and a.KienNghiId is not null ";
                    }
                    else if (!bool.Parse(KienNghi))
                    {
                        query = query + "and a.KienNghiId is null ";
                    }

                    if (!string.IsNullOrEmpty(TrangThaiNhap))
                    {
                        if (TrangThaiNhap == "tre")
                            query = query + "and DATEDIFF(hour, a.ThoiGianXuatHien, a.NgayTao)>24 ";
                        else if (TrangThaiNhap == "dunghan")
                            query = query + "and DATEDIFF(hour, a.ThoiGianXuatHien, a.NgayTao)<=24 ";
                    }

                    if (TCTDuyetMT == "all" || string.IsNullOrEmpty(TCTDuyetMT))
                    {

                    }
                    else if (bool.Parse(TCTDuyetMT))
                    {
                        query = query + "and a.NPCIsDuyetMT=1 ";
                    }
                    else if (!bool.Parse(TCTDuyetMT))
                    {
                        query = query + "and a.NPCIsDuyetMT=0 ";
                    }

                    if (!string.IsNullOrEmpty(CapDienAp))
                    {
                        query = query + "and a.CapDienAp=@CapDienAp ";
                    }
                    if (!string.IsNullOrEmpty(LoaiTaiSan))
                    {
                        if (LoaiTaiSan == "dienluc")
                            query = query + "and isnull(a.IsTaiSan,0)=1 ";
                        else if (LoaiTaiSan == "khachhang")
                            query = query + "and isnull(a.IsTaiSan,0)=0 ";
                    }

                    if (!string.IsNullOrEmpty(TrangThaiChuyenNPC))
                    {
                        if (TrangThaiChuyenNPC == "dachuyen")
                            query = query + "and isnull(a.IsChuyenNPC,0)=1 ";
                        else if (TrangThaiChuyenNPC == "chuachuyen")
                            query = query + "and isnull(a.IsChuyenNPC,0)=0 ";
                    }

                    query = query +
                        ") as kq " +
                        "where RowNum BETWEEN((@page - 1) * @pageSize) + 1 and @page*@pageSize "
                        ;
                    #endregion


                    using (var multipleresult = db.QueryMultiple(query,
                        new
                        {
                            page = page,
                            pageSize = pageSize,
                            filter = DonViId,
                            TuNgay = start1,
                            DenNgay = end1,
                            PhongBanId = PhongBanId,
                            DonViId = DonViId,
                            LoaiSuCo = LoaiSuCo,
                            TinhChat = TinhChat,
                            NguyenNhan = NguyenNhan,
                            MienTru = MienTru,
                            CapDienAp = CapDienAp,
                        }))
                    {
                        var q = multipleresult.Read<SuCoModel>();
                        lstData = q.ToList();
                    }
                }
            }
            catch (Exception ex) { }
            return lstData;
        }
    }
}
public class SuCoModel
{
    public int Id { get; set; }
    public string DonViId { get; set; }
    public string lstDonViSuCoId { get; set; }
    public string CapDienAp { get; set; }
    public string TenThietBi { get; set; }
    public string DienBienSuCo { get; set; }
    public string TomTat { get; set; }
    public Nullable<bool> TinhTrangBienBan { get; set; }
    public Nullable<bool> HinhAnhSuCo { get; set; }
    public Nullable<System.DateTime> ThoiGianXuatHien { get; set; }
    public Nullable<System.DateTime> ThoiGianBatDauKhacPhuc { get; set; }
    public Nullable<System.DateTime> ThoiGianKhacPhucXong { get; set; }
    public Nullable<System.DateTime> ThoiGianKhoiPhuc { get; set; }
    public Nullable<double> T_XuatHienBatDauKhacPhuc { get; set; }
    public Nullable<double> T_BatDauDenKhacPhucXong { get; set; }
    public Nullable<double> T_KhacPhucXongDenKhoiPhuc { get; set; }
    public Nullable<double> T_TongThoiGianMatDien { get; set; }
    public Nullable<bool> IsGianDoan { get; set; }
    public Nullable<int> PhieuCongTacId { get; set; }
    public Nullable<int> LoaiSuCoId { get; set; }
    public Nullable<int> NguyenNhanId { get; set; }
    public Nullable<int> TinhChatId { get; set; }

    public Nullable<System.DateTime> NgayTao { get; set; }
    public string NguoiTao { get; set; }
    public Nullable<int> TrangThai { get; set; }
    public Nullable<System.DateTime> NgayDuyet { get; set; }
    public string NguoiDuyet { get; set; }
    public Nullable<System.DateTime> NgaySua { get; set; }
    public string NguoiSua { get; set; }
    public Nullable<decimal> KinhDo { get; set; }
    public Nullable<decimal> ViDo { get; set; }
    public Nullable<bool> IsTaiSan { get; set; }
    public Nullable<bool> IsChuyenNPC { get; set; }
    public Nullable<System.DateTime> NgayDuyetNPC { get; set; }
    public string NguoiDuyetNPC { get; set; }

    public string LoaiSuCo { get; set; }
    public string NguyenNhan { get; set; }
    public string TinhChat { get; set; }
    public string TenDvi { get; set; }

    public string BienBan { get; set; }
    public string HinhAnh { get; set; }
    public string TaiSan { get; set; }
    public string TrangThaiNhap { get; set; }

    public Nullable<bool> IsMienTru { get; set; }
    public string strMienTru { get; set; }
    public bool? NPCIsDuyetMT { get; set; }
    public string strNPCIsDuyetMT { get; set; }
    public DateTime? NPCNgayDuyetMT { get; set; }
    public string strNPCNgayDuyetMT { get; set; }
    public string NPCNguoiDuyetMT { get; set; }
    public string NPCTenNguoiDuyetMT { get; set; }
    public string NPCCommentMT { get; set; }

    public int? KienNghiId { get; set; }
    public DateTime? NgayKienNghi { get; set; }
    public string NguoiKienNghi { get; set; }
    public List<sc_KienNghiMienTru_TaiLieuModel> lstTLKN { get; set; }
    public string HinhAnhKienNghi { get; set; }
    public string NoiDungKienNghi { get; set; }
} 
public class ThongKeSuCo
{
    public int ID { get; set; }
    public string TenLoai { get; set; }
    public int SoLuong { get; set; }
}

public class BieuDoTronSuCoModel
{
    public string Id { get; set; }
    public string TenDonVi { get; set; }
    public int SLCap04KV { get; set; }
    public int SLCap6KV { get; set; }
    public int SLCap10KV { get; set; }
    public int SLCap22KV { get; set; }
    public int SLCap35KV { get; set; }
    public int SLCap110KV { get; set; }
    public decimal XuatHienBatDauKhacPhuc { get; set; }
    public decimal BatDauDenKhacPhucXong { get; set; }
    public decimal KhacPhucXongDenKhoiPhuc { get; set; }
    public decimal TongThoiGianMatDien { get; set; }
    public int SLThoangQua { get; set; }
    public int SLKeoDai { get; set; }
    public int SLLoaiKhongXacDinh { get; set; }
    public int SLTSDienLuc { get; set; }
    public int SLTSKhachHang { get; set; }
    public int SLKhachQuan { get; set; }
    public int SLChuQuan { get; set; }
    public int SLNguyenNhanKhongXacDinh { get; set; }
    public int SLHanhLang { get; set; }
    public int SLThietBi { get; set; }
    public int SLMayBienAp { get; set; }
    public int SLDuongDay { get; set; }
    public int SLChuaXacDinh { get; set; }
    public int SLThienTai { get; set; }
    public int SLTinhChatKhongXacDinh { get; set; }
}