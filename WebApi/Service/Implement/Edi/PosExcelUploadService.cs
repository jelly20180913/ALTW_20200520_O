using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Service.Interface;
using System.IO;
using WebApi.Models;
using WebApi.Common.FileAdapter;
using WebApi.Service.Interface.Table;
using WebApi.Service.Interface.Common;
using WebApi.DataModel.CustomModel.Edi;
using WebApi.Common;
using Spire.Xls;
using System.Drawing;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Configuration;
namespace WebApi.Service.Implement
{
    public class PosExcelUploadService : IPosExcelUploadService
    {
        private IUploadLogService _uploadLogService;
        private ILoginService _loginService;
        private IFileAdapterFactory _fileAdapterFactory;
        private IEdi_PosService _edi_PosService;
        private ICommonFileService _commonFileService;
        private Interface.Common.ICommonService _commonService;
        public PosExcelUploadService(IUploadLogService uploadLogSerivice, ILoginService loginService, FileAdapterFactory fileAdapterFactory, IEdi_PosService edi_PosService, ICommonFileService commonFileService, Interface.Common.ICommonService commonService)
        {
            this._uploadLogService = uploadLogSerivice;
            this._loginService = loginService;
            this._fileAdapterFactory = fileAdapterFactory;
            this._edi_PosService = edi_PosService;
            this._commonFileService = commonFileService;
            this._commonService = commonService;
        }
        /// <summary>
        /// 1. upload file (excel)
        /// 2. parse excel column value
        /// 3. batch insert posdata
        /// 4. insert uploadlog
        /// 5. copy file to success directory  
        /// </summary>
        /// <returns></returns>
        public List<string> UploadFile()
        {
            List<string> _ListError = new List<string>();
            string _FilePath = this._commonFileService.Upload();
            UploadLog _ReportFile = this._uploadLogService.GetByName(Path.GetFileName(_FilePath));
            if (_ReportFile != null) throw new Exception(" don't upload duplicate file  ");
            IQueryable<Edi_Pos> _Edi_PosData = parse(_FilePath);
            if (checkData(_Edi_PosData))
            {
                int _LoginID = HttpContext.Current.Request["LoginID"].ToString() != "" ? Convert.ToInt32(HttpContext.Current.Request["LoginID"].ToString()) : 0;
                _ListError = _edi_PosService.MiltiCreate(_Edi_PosData, _LoginID);
                bool _Success = _ListError.Count > 1 ? false : true;
                string _ServerFileName = this._commonFileService.SaveToSuccess(_LoginID, _FilePath, "Edi_Pos");
                string _UploadLogError = insertUploadLog(_LoginID, _FilePath, "Edi_Pos", _Success, _ServerFileName);
                if (_UploadLogError != "") _ListError.Add(_UploadLogError);
                if (_ListError.Count > 1)
                {
                    _ListError[_ListError.Count - 2] = _ListError[_ListError.Count - 2] + "ms";
                    string _Error = string.Join("\r\n", _ListError.ToArray());
                    throw new Exception(_Error);
                }
                else _ListError[0] = _ListError[0] + "ms";
            }
            else
            {
                throw new Exception(" please confirm your file format ");
            }
            return _ListError;
        }
        /// <summary>
        /// </summary>
        /// <param name="filePath"></param>
        private IQueryable<Edi_Pos> parse(string filePath)
        {
            string _Ext = Path.GetExtension(filePath);
            FileBase _Base = _fileAdapterFactory.CreateFileAdapter(_Ext);
            IQueryable<Edi_Pos> _Edi_Pos = _Base.ParseEdiPos(filePath);
            if (_Base.ListError.Count() != 0)
            {
                string _Error = string.Join("\r\n", _Base.ListError.ToArray());
                throw new Exception(_Error);
            }
            return _Edi_Pos;
        }
        /// <summary>
        /// insert uploadlog 
        /// </summary>
        /// <param name="loginID"></param>
        /// <param name="fileSavePath"></param>
        /// <param name="tableName">PosData</param>
        /// <returns></returns>
        private string insertUploadLog(int loginID, string fileSavePath, string tableName, bool success, string serverFileName)
        {
            UploadLog _UploadLog = new UploadLog();
            _UploadLog.FK_LoginId = loginID;
            _UploadLog.FileName = Path.GetFileName(fileSavePath);
            _UploadLog.TableName = tableName;
            _UploadLog.UpdateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            _UploadLog.Success = success;
            _UploadLog.ServerFileName = serverFileName;
            return this._uploadLogService.Create(_UploadLog);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="posData"></param>
        /// <returns></returns>
        private bool checkData(IQueryable<Edi_Pos> posData)
        {
            bool _OK = false;
            if (posData.Count() > 0)
                _OK = posData.First().Cost != null;
            return _OK;
        }
        /// <summary>
        /// get pos data list
        /// </summary>
        /// <returns></returns>
        public List<Edi_Pos> GetEdi_PosList(string dateStart, string dateEnd)
        {
            List<Edi_Pos> _Edi_PosList = new List<Edi_Pos>();
            DateTime _DateStart = DateTime.Parse(dateStart);
            DateTime _DateEnd = DateTime.Parse(dateEnd).AddHours(23).AddMinutes(59).AddSeconds(59);
            _Edi_PosList = this._edi_PosService.GetAll().Where(x => x.ShipDate2 != "").ToList();
            _Edi_PosList = _Edi_PosList.Where(x => DateTime.Parse(x.ShipDate2) > _DateStart && DateTime.Parse(x.ShipDate2) < _DateEnd).OrderByDescending(x => x.Id).ToList();
            return _Edi_PosList;
        }
        public bool UpdateEdi_Pos(Edi_Pos edi_Pos)
        {
            bool _Success = false;
            if (edi_Pos.Id == 0) _Success = updateEdi_Pos(edi_Pos);
            else { _Success = updateEdi_PosById(edi_Pos); }
            _Success = true;
            return _Success;
        }
        /// <summary>
        /// update all same name customer columns value
        /// </summary>
        /// <param name="edi_Pos"></param>
        /// <returns></returns>
        private bool updateEdi_Pos(Edi_Pos edi_Pos)
        {
            bool _Success = false;
            List<Edi_Pos> _Edi_PosList = this._edi_PosService.GetAll().Where(x => x.CustomerName == edi_Pos.CustomerName).ToList();
            foreach (Edi_Pos c in _Edi_PosList)
            {
                if (edi_Pos.Address != null) c.Address = edi_Pos.Address;
                if (edi_Pos.MarketCode != null) c.MarketCode = edi_Pos.MarketCode;
                if (edi_Pos.Market != null) c.Market = edi_Pos.Market;
                if (edi_Pos.SubSegment != null) c.SubSegment = edi_Pos.SubSegment;
                if (edi_Pos.SubSegmentCode != null) c.SubSegmentCode = edi_Pos.SubSegmentCode;
                if (edi_Pos.CorporateMarket != null) c.CorporateMarket = edi_Pos.CorporateMarket;
                this._edi_PosService.Update(c);
            }
            _Success = true;
            return _Success;
        }
        /// <summary>
        /// update data by id
        /// </summary>
        /// <param name="edi_Pos"></param>
        /// <returns></returns>
        private bool updateEdi_PosById(Edi_Pos edi_Pos)
        {
            bool _Success = false;
            Edi_Pos _Edi_Pos = this._edi_PosService.GetByID(edi_Pos.Id);
            _Edi_Pos.Type = edi_Pos.Type;
            _Edi_Pos.Distributor = edi_Pos.Distributor;
            _Edi_Pos.Address = edi_Pos.Address;
            _Edi_Pos.City = edi_Pos.City;
            _Edi_Pos.State = edi_Pos.State;
            _Edi_Pos.ZIP = edi_Pos.ZIP;
            _Edi_Pos.Series = edi_Pos.Series;
            _Edi_Pos.PartNo = edi_Pos.PartNo;
            _Edi_Pos.Quantity = edi_Pos.Quantity;
            _Edi_Pos.Cost = edi_Pos.Cost;
            _Edi_Pos.Price = edi_Pos.Price;
            _Edi_Pos.ResellingExt = edi_Pos.ResellingExt;
            _Edi_Pos.ACCT = edi_Pos.ACCT;
            _Edi_Pos.CustomerName = edi_Pos.CustomerName;
            _Edi_Pos.MarketCode = edi_Pos.MarketCode;
            _Edi_Pos.Market = edi_Pos.Market;
            _Edi_Pos.SubSegmentCode = edi_Pos.SubSegmentCode;
            _Edi_Pos.SubSegment = edi_Pos.SubSegment;
            _Edi_Pos.Remarks = edi_Pos.Remarks;
            _Edi_Pos.ShipDate = edi_Pos.ShipDate;
            _Edi_Pos.ShipDate2 = edi_Pos.ShipDate2;
            _Edi_Pos.ShipMonth = edi_Pos.ShipMonth;
            _Edi_Pos.ShipQuarter = edi_Pos.ShipQuarter;
            _Edi_Pos.CountryCode = edi_Pos.CountryCode;
            _Edi_Pos.Country = edi_Pos.Country;
            _Edi_Pos.Region = edi_Pos.Region;
            _Edi_Pos.UpdateTime = DateTime.Now;
            _Edi_Pos.YY = edi_Pos.YY;
            _Edi_Pos.CorporateMarket = edi_Pos.CorporateMarket;
            this._edi_PosService.Update(_Edi_Pos);
            _Success = true;
            return _Success;
        }
        /// <summary>
        /// group pos data 
        /// </summary>
        /// <param name="type">ICAT/IFD/IPD</param>
        /// <param name="quarter">Q1/Q2/Q3/Q4</param>
        /// <param name="month"></param>
        /// <param name="group">Series/Country/PN/CUS/PN_CUS</param>
        /// <param name="series">Asiya provide list</param>
        /// <param name="country">Asiya provide list</param>
        /// <param name="dist">Asiya provide list</param>
        /// <param name="market">Asiya provide list</param>
        /// <returns></returns>
        public List<PosGroup> GetPosGroupList(string type, string quarter, string month, string group, string series, string country, string dist, string market,string yy,string region,string groupKey)
        {
            List<Edi_Pos> _Edi_PosList = new List<Edi_Pos>();
            List<PosGroup> _PosGroupList = new List<PosGroup>();
            _Edi_PosList = this._edi_PosService.GetAll().ToList();
            if (type != "ALL" && type != "Type") _Edi_PosList = _Edi_PosList.Where(x => x.Type == type).ToList();
            if (quarter != "ALL" && quarter != "Quarter") _Edi_PosList = _Edi_PosList.Where(x => x.ShipQuarter == quarter).ToList();
            if (month != "ALL" && month != "Month") _Edi_PosList = _Edi_PosList.Where(x => x.ShipMonth == month).ToList();
            if (series != "ALL" && series != "Series") _Edi_PosList = _Edi_PosList.Where(x => x.Series == series).ToList();
            if (country != "ALL" && country != "Country") _Edi_PosList = _Edi_PosList.Where(x => x.Country == country).ToList();
            if (dist != "ALL" && dist != "Dist") _Edi_PosList = _Edi_PosList.Where(x => x.Distributor == dist).ToList();
            if (market != "ALL" && market != "Market") _Edi_PosList = _Edi_PosList.Where(x => x.Market == market).ToList();
            if (yy != "ALL" && yy!= "YY") _Edi_PosList = _Edi_PosList.Where(x => x.YY == yy).ToList();
            if (region != "ALL" && region != "Region") _Edi_PosList = _Edi_PosList.Where(x => x.Region == region).ToList();
            if (group == "Series")
            {

                _PosGroupList = _Edi_PosList.GroupBy(x => x.Series).Select(x => new PosGroup
                {
                    Name = x.Key,
                    Qty = x.Sum(y => y.Quantity)
                }).ToList();
            }

            else if (group == "Country") _PosGroupList = _Edi_PosList.GroupBy(x => x.Country).Select(x => new PosGroup
            {
                Name = x.Key,
                Qty = x.Count()
            }).ToList();
            else if (group == "PN") _PosGroupList = _Edi_PosList.GroupBy(x => x.PartNo).Select(x => new PosGroup
            {
                Name = x.Key,
                Qty = x.Sum(y => y.Quantity)
            }).ToList();
            else if (group == "CUS") _PosGroupList = _Edi_PosList.GroupBy(x => x.PartNo).Select(x => new PosGroup
            {
                Name = x.Key,
                Qty = x.Count()
            }).ToList();
            else if (group == "PN_CUS") _PosGroupList = _Edi_PosList.GroupBy(x => x.CustomerName).Select(x => new PosGroup
            {
                Name = x.Key,
                Qty = x.Sum(y => y.Quantity)
            }).ToList();
            else if (group == "PartNo")
            {
                if (groupKey != "ALL" && groupKey != "GroupKey") _Edi_PosList = _Edi_PosList.Where(x => x.Series == groupKey).ToList();
                _PosGroupList = _Edi_PosList.GroupBy(x => x.PartNo).Select(x => new PosGroup
                { 
                    Name = x.Key,
                    Qty = x.Sum(y => y.Quantity)
                }).ToList();
            }
            return _PosGroupList;
        }
        /// <summary>
        /// 1. get period export data 
        /// 1.1. wait moment when you first to get
        /// 2. export to excel
        /// 3. print watermark in excel
        /// 4. responce to client for download
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        public HttpResponseMessage GetEdi_PosListToExcel(string dateStart, string dateEnd)
        {
            List<Edi_Pos> _Edi_PosList = GetEdi_PosList(dateStart, dateEnd);
            string _Start = DateTime.Parse(dateStart).ToString("yyyyMMdd");
            string _End = DateTime.Parse(dateEnd).ToString("yyyyMMdd");
            string _FileName = _Start + "_" + _End + ".xlsx";
            string _FilePath = saveExcel(_Edi_PosList, _FileName);
            saveExcelWatermark(_FilePath);
            return GetReportFile(_FileName);
        }
        private string saveExcel(List<Edi_Pos> edi_PosList, string fileName)
        {
            XSLXHelper helper = new XSLXHelper();
            var xlsx = helper.Export(edi_PosList, false);
            string pathString = HttpContext.Current.Server.MapPath("~/Success/PosExport");
            if (!Directory.Exists(pathString))
            {
                Directory.CreateDirectory(pathString);
            }
            string _FilePath = pathString + "/" + fileName;
            xlsx.SaveAs(pathString + "/" + fileName);
            return _FilePath;
        }
        /// <summary>
        /// water mark
        /// </summary>
        /// <param name="filePath"></param>
        private void saveExcelWatermark(string filePath)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(filePath);
            Font font = new System.Drawing.Font("arial", 40);
            String watermark = "Confidential";
            foreach (Worksheet sheet in workbook.Worksheets)
            {
                //call DrawText() to create an image
                System.Drawing.Image imgWtrmrk = WebApi.Common.Function.DrawText(watermark, font, System.Drawing.Color.LightCoral, System.Drawing.Color.White, sheet.PageSetup.PageHeight, sheet.PageSetup.PageWidth);
                //set image as left header image
                sheet.PageSetup.LeftHeaderImage = imgWtrmrk;
                sheet.PageSetup.LeftHeader = "&G";
                //the watermark will only appear in this mode, it will disappear if the mode is normal
                sheet.ViewMode = ViewMode.Layout;
            }
            workbook.SaveToFile(filePath, ExcelVersion.Version2010);
        }
        /// <summary>
        /// download report file
        /// </summary>
        /// <returns></returns>
        private HttpResponseMessage GetReportFile(string fileName)
        {
            var _Token = HttpContext.Current.Request["Token"].ToString();
            HttpStatusCode _Code = _Token == "" ? HttpStatusCode.Forbidden : HttpStatusCode.OK;
            var FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Success/PosExport/" + fileName);
            var stream = new FileStream(FilePath, FileMode.Open);
            HttpResponseMessage response = new HttpResponseMessage(_Code);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            return response;
        }
        /// <summary>
        /// user trace log 
        /// </summary>
        /// <param name="buttonName"></param>
        /// <param name="remark"></param>
        /// <param name="page"></param>
        public void InsertButtonLog(string buttonName, string remark, string page)
        {
            int _LoginID = HttpContext.Current.Request["LoginID"].ToString() != "" ? Convert.ToInt32(HttpContext.Current.Request["LoginID"].ToString()) : 0;
            this._commonService.InsetButtonLog(_LoginID, buttonName, remark, page);
        }
        /// <summary>
        /// feedback 
        /// </summary>
        /// <param name="edi_Pos"></param>
        /// <returns></returns>
        public bool Email(Edi_Pos edi_Pos)
        {
            int _LoginID = HttpContext.Current.Request["LoginID"].ToString() != "" ? Convert.ToInt32(HttpContext.Current.Request["LoginID"].ToString()) : 0;
            Login _Login = this._loginService.GetByID(_LoginID);
            string _Color = "Blue";
            string _Body = "Hello Asiya<br><br>";
            _Body += "Id:" + setFontColor(edi_Pos.Id.ToString(), "Green") + "      correct pos data is...<br><br> ";
            _Body += "NAME   :" + setFontColor(edi_Pos.CustomerName, _Color) + "<br> ";
            _Body += "Market Code     :" + setFontColor(edi_Pos.MarketCode, _Color) + "<br> ";
            _Body += "Market Segment  :" + setFontColor(edi_Pos.Market, _Color) + "<br> ";
            _Body += "Sub-Segment Code :" + setFontColor(edi_Pos.SubSegmentCode, _Color) + "<br> ";
            _Body += "Sub-Segment     :" + setFontColor(edi_Pos.SubSegment, _Color) + "<br> ";
            _Body += "Type           :" + setFontColor(edi_Pos.Type, _Color) + "<br> ";
            _Body += "DIST    :" + setFontColor(edi_Pos.Distributor, _Color) + "<br> ";
            _Body += "Address        :" + setFontColor(edi_Pos.Address, _Color) + "<br> ";
            _Body += "CITY           :" + setFontColor(edi_Pos.City, _Color) + "<br> ";
            _Body += "STATE          :" + setFontColor(edi_Pos.State, _Color) + "<br> ";
            _Body += "ZIP            :" + setFontColor(edi_Pos.ZIP, _Color) + "<br> ";
            _Body += "Series         :" + setFontColor(edi_Pos.Series, _Color) + "<br> ";
            _Body += "ALTW PARTNO        :" + setFontColor(edi_Pos.PartNo, _Color) + "<br> ";
            string _Quantity = edi_Pos.Quantity != null ? edi_Pos.Quantity.ToString() : "";
            _Body += "QTY       :" + setFontColor(_Quantity, _Color) + "<br> ";
            string _Cost = edi_Pos.Cost != null ? edi_Pos.Cost.ToString() : "";
            _Body += "COST           :" + setFontColor(_Cost, _Color) + "<br> ";
            string _Price = edi_Pos.Price != null ? edi_Pos.Price.ToString() : "";
            _Body += "PRICE          :" + setFontColor(_Price, _Color) + "<br> ";
            string _ResellingExt = edi_Pos.ResellingExt != null ? edi_Pos.ResellingExt.ToString() : "";
            _Body += "RES.EXT   :" + setFontColor(_ResellingExt, _Color) + "<br> ";
            _Body += "ACCT           :" + setFontColor(edi_Pos.ACCT, _Color) + "<br> "; 
            _Body += "Remarks        :" + setFontColor(edi_Pos.Remarks, _Color) + "<br> ";
            _Body += "SHIP DATE       :" + setFontColor(edi_Pos.ShipDate, _Color) + "<br> ";
            _Body += "SHIP DATE2      :" + setFontColor(edi_Pos.ShipDate2, _Color) + "<br> ";
            _Body += "SHIP MONTH      :" + setFontColor(edi_Pos.ShipMonth, _Color) + "<br> ";
            _Body += "SHIP QUARTER    :" + setFontColor(edi_Pos.ShipQuarter, _Color) + "<br> ";
            _Body += "COUNTRY CODE    :" + setFontColor(edi_Pos.CountryCode, _Color) + "<br> ";
            _Body += "COUNTRY        :" + setFontColor(edi_Pos.Country, _Color) + "<br> ";
            _Body += "Region         :" + setFontColor(edi_Pos.Region, _Color) + "<br> ";
             string _FeedbackEmail = ConfigurationManager.AppSettings["FeedbackEmail"];
            bool _Success = Function.SendEmail("Feedback for pos data", _Body, _Login.CustomerName, _FeedbackEmail);
            return _Success;
        }
        private string setFontColor(string columnName, string color)
        {
            string _Font = "<font color = '" + color + "'>" + columnName + "</font>";
            return _Font;
        } 
    }
}