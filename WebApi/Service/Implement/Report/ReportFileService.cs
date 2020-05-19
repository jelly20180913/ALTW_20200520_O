using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Service.Interface;
using System.IO;
using WebApi.Models;
using WebApi.Service.Interface.Common;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net; 
namespace WebApi.Service.Implement
{
    public class ReportFileService : IReportFileService
    {
        private ICommonFileService _commonFileService;
        private WebApi.Service.Interface.Table.IReportFileService _reportFileService;
        private ILoginService _loginService;
        public ReportFileService(ICommonFileService commonFileService, WebApi.Service.Interface.Table.IReportFileService reportFileService, ILoginService loginService)
        {
            this._commonFileService = commonFileService;
            this._reportFileService = reportFileService;
            this._loginService = loginService;
        }
        /// <summary>
        /// 1. need log in by admin account   
        /// 2. upload file
        /// 3. same file name 
        /// 3.1. same :need update table and delete old server file 
        /// 3.2. different: need insert table  
        /// 4. copy file to success directory 
        /// </summary>
        /// <returns></returns>
        public List<string> UploadFile()
        {
            string _UploadLogError = "";
            List<string> _ListError = new List<string>();
            int _LoginID = HttpContext.Current.Request["LoginID"].ToString() != "" ? Convert.ToInt32(HttpContext.Current.Request["LoginID"].ToString()) : 0;
            Login _Login = _loginService.GetByID(_LoginID); 
           // if (_Login.Account != "ADMIN" && _Login.Password != "ADMIN") throw new Exception("Please log in by admin account");
            string _FilePath = this._commonFileService.Upload();
            bool _Success = _ListError.Count > 0 ? false : true;
            string _Date = HttpContext.Current.Request["Date"].ToString();
            string _FileName = Path.GetFileNameWithoutExtension(_FilePath);
            ReportFile _ReportFile = this._reportFileService.GetByFileNameAndDate(_FileName, _Date, _Login.CustomerName);
            if (_ReportFile != null) File.Delete(_ReportFile.ServerFileName);
            string _ServerFileName = this._commonFileService.SaveToSuccess(_LoginID, _FilePath, _Date);
            ReportFile _SetReportFile = setReportFile(_LoginID, _FilePath, _Success, _ServerFileName, _Date, _ReportFile,_Login.CustomerName);
            if (_ReportFile != null) this._reportFileService.Update(_SetReportFile);
            else _UploadLogError = this._reportFileService.Create(_SetReportFile); 
            if (_UploadLogError != "") _ListError.Add(_UploadLogError);
            if (_ListError.Count > 0) _ListError[0] = _ListError[0] + "ms";
            return _ListError;
        }

        /// <summary>
        /// set reportFile entity
        /// </summary>
        /// <param name="loginID"></param>
        /// <param name="fileSavePath"></param>
        /// <param name="success"> </param>
        /// <param name="serverFileName"> </param>
        /// <param name="date"> </param>
        /// <param name="_ReportFile"> </param>
        /// <returns></returns>
        private ReportFile setReportFile(int loginID, string fileSavePath, bool success, string serverFileName, string date, ReportFile _ReportFile,string customerName)
        {
            string _Ext = Path.GetExtension(serverFileName);
            bool _CanDownload = false;
            if ((_Ext == ".XLSX") || (_Ext == ".xlsx") || (_Ext == ".xls") || (_Ext == ".pptx")) _CanDownload = true;
            if (_ReportFile == null) _ReportFile = new ReportFile();
            _ReportFile.FK_LoginId = loginID;
            _ReportFile.FileName = Path.GetFileNameWithoutExtension(fileSavePath);
            _ReportFile.UpdateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            _ReportFile.Success = success;
            _ReportFile.ServerFileName = serverFileName;
            _ReportFile.Date = date;
            _ReportFile.CanDownload = _CanDownload;
            _ReportFile.IsDel = false;
            _ReportFile.ServerSimpleFileName = Path.GetFileName(serverFileName);
            _ReportFile.Flag = customerName;
            return _ReportFile;
        }
        /// <summary>
        /// Login.Country:report type
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<ReportFile> GetReportFileByDate(string date)
        {
            int _LoginID = HttpContext.Current.Request["LoginID"].ToString() != "" ? Convert.ToInt32(HttpContext.Current.Request["LoginID"].ToString()) : 0;
            Login _Login = _loginService.GetByID(_LoginID);
            int _Count = this._reportFileService.GetAllByDate(date).ToList().Count();
            List < ReportFile > _ReportFileList= this._reportFileService.GetAllByDate(date).Where(x => x.Flag == _Login.Country).ToList();
            return _ReportFileList;
        }
        /// <summary>
        /// download report file
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetReportFile()
        {
            var _Id = HttpContext.Current.Request["Id"].ToString() != "" ? Convert.ToInt32(HttpContext.Current.Request["Id"].ToString()) : 0;
            int _LoginID = HttpContext.Current.Request["LoginID"].ToString() != "" ? Convert.ToInt32(HttpContext.Current.Request["LoginID"].ToString()) : 0;
            Login _Login = _loginService.GetByID(_LoginID);
            var _Token = HttpContext.Current.Request["Token"].ToString(); 
            HttpStatusCode _Code = _Token == "" ? HttpStatusCode.Forbidden : HttpStatusCode.OK;
            ReportFile _ReportFile = this._reportFileService.GetByID(_Id);
           // var FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Success/AmphenolReport/" + _ReportFile.Date + "/" + _ReportFile.ServerSimpleFileName);
            var FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Success/"+ _Login.Country + "/" + _ReportFile.Date + "/" + _ReportFile.ServerSimpleFileName);
            string _Ext = Path.GetExtension(_ReportFile.ServerSimpleFileName);
            var stream = new FileStream(FilePath, FileMode.Open);
            HttpResponseMessage response = new HttpResponseMessage(_Code);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = _ReportFile.FileName + _Ext
            };
            return response;
        }
    }
}