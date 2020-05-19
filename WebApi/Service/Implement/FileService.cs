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
namespace WebApi.Service.Implement
{
    public class FileService : IFileService
    {
        private IPosDataService _posDataService;
        private IPosColumnMapService _posColumnMapService;
        private IUploadLogService _uploadLogService;
        private ILoginService _loginService;
        private IFileAdapterFactory _fileAdapterFactory;
        private IPosService _posService;
        private ICommonFileService _commonFileService;
        public FileService(IPosDataService posDataService, IPosColumnMapService posColumnMapService, IUploadLogService uploadLogSerivice, ILoginService loginService, FileAdapterFactory fileAdapterFactory, IPosService posService, ICommonFileService commonFileService)
        {
            this._posDataService = posDataService;
            this._posColumnMapService = posColumnMapService;
            this._uploadLogService = uploadLogSerivice;
            this._loginService = loginService;
            this._fileAdapterFactory = fileAdapterFactory;
            this._posService = posService;
            this._commonFileService = commonFileService;
        }
        /// <summary>
        /// 1. upload file (excel/txt)
        /// 2. parse excel column value
        /// 3. batch insert posdata
        /// 4. insert uploadlog
        /// 5. copy file to success directory
        /// p.s parameter need login id and  model-ok
        /// p.s need remote test upload file method can be right
        /// </summary>
        /// <returns></returns>
        public List<string> UploadFile()
        {
            List<string> _ListError = new List<string>();
            // string _FilePath = Upload(); 
            string _FilePath = this._commonFileService.Upload();
            UploadLog _ReportFile = this._uploadLogService.GetByName(Path.GetFileName(_FilePath));
            if (_ReportFile!=null) throw new Exception(" don't upload duplicate file  ");
            IQueryable<Pos> _PosData = parse(_FilePath );  
            if (checkData(_PosData))
            {
                int _LoginID = HttpContext.Current.Request["LoginID"].ToString() != "" ? Convert.ToInt32(HttpContext.Current.Request["LoginID"].ToString()) : 0;
                _ListError = _posService.MiltiCreate(_PosData, _LoginID);
                bool _Success = _ListError.Count > 1 ? false : true;
              //  string _ServerFileName = saveToSuccess(_LoginID, _FilePath, "Pos");
                string _ServerFileName = this._commonFileService.SaveToSuccess(_LoginID, _FilePath, "Pos");
                string _UploadLogError = insertUploadLog(_LoginID, _FilePath, "Pos", _Success, _ServerFileName);
                if (_UploadLogError != "") _ListError.Add(_UploadLogError);
                if (_ListError.Count > 0) _ListError[0] = _ListError[0] + "ms";
            }
            else
            {
                throw new Exception(" please confirm your file format ");
            }
            return _ListError;
        } 
        /// <summary>
        /// p.s mapping column use hard code ,remember to rewrite this scope-ok
        /// p.s maybe can use Polytype to solve parse diffrent table by excel
        /// p.s use DI container put this class abstact-ok
        /// p.s maybe can use Polytype to solve parse dirffrent typ by txt excel csv-ok
        /// put error message in list and response it on web page
        /// </summary>
        /// <param name="filePath"></param>
        private IQueryable<Pos> parse(string filePath )
        {
            string _Ext = Path.GetExtension(filePath);
            int _Model = HttpContext.Current.Request["Model"].ToString() != "" ? Convert.ToInt32(HttpContext.Current.Request["Model"].ToString()) : 0;
            FileBase _Base = _fileAdapterFactory.CreateFileAdapter(_Ext);
           // IQueryable<PosData> _PosData = _Base.Parse(_Model, filePath); 
            IQueryable<Pos> _Pos = _Base.ParsePos(_Model, filePath); 
            if (_Base.ListError.Count() != 0)
            {
                string _Error = string.Join("\r\n", _Base.ListError.ToArray());
                throw new Exception(_Error);
            } 
            return _Pos;
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
        private bool checkData(IQueryable<Pos> posData)
        {
            bool _OK = false;
            if (posData.Count() > 0)
                _OK = posData.First().Cost != null;
            return _OK;
        } 
    }
}