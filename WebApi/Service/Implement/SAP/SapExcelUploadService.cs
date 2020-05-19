using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Service.Interface;
using WebApi.Models;
using System.Data;
using WebApi.Common.SapAdapter;
using WebApi.Service.Interface.Table;
using WebApi.Service.Interface.Common;
using WebApi.DataModel.CustomModel.SAP;

namespace WebApi.Service.Implement
{
    public class SapExcelUploadService : ISapExcelUploadService
    {
        private IUploadLogService _uploadLogService;
        private ILoginService _loginService;
        private ISapAdapterFactory _sapAdapterFactory;
        private ICommonFileService _commonFileService;
        private ISAP_PriceListService _sap_PriceListService;
        /// <summary>
        /// Dependence Injection
        /// </summary>
        /// <param name="uploadLogSerivice"></param>
        /// <param name="loginService"></param>
        /// <param name="sapAdapterFactory"></param>
        /// <param name="commonFileService"></param>
        /// <param name="sap_PriceListService"></param>
        public SapExcelUploadService(IUploadLogService uploadLogSerivice, ILoginService loginService, SapAdapterFactory sapAdapterFactory, ICommonFileService commonFileService, ISAP_PriceListService  sap_PriceListService)
        {
            this._uploadLogService = uploadLogSerivice;
            this._loginService = loginService;
            this._sapAdapterFactory = sapAdapterFactory;
            this._commonFileService = commonFileService;
            this._sap_PriceListService = sap_PriceListService;
        }
        /// <summary>
        /// 1. upload file (excel)
        /// 2. parse excel column value
        /// 3. batch insert  data 
        /// 4. copy file to success directory 
        /// 5. insert uploadlog
        /// </summary>
        /// <returns></returns>
        public List<string> UploadFile()
        {
            List<string> _ListError = new List<string>();
            string _FilePath = this._commonFileService.Upload();
            int _LoginID = HttpContext.Current.Request["LoginID"].ToString() != "" ? Convert.ToInt32(HttpContext.Current.Request["LoginID"].ToString()) : 0;
            string _TableName = "";
            SapMiddleData _SapMiddleData = parse(_FilePath);
            _ListError = miltiCreate("", _SapMiddleData, out _TableName);
            bool _Success = _ListError.Count > 1 ? false : true;
            string _ServerFileName = this._commonFileService.SaveToSuccess(_LoginID, _FilePath, _TableName);
            string _UploadLogError = this._commonFileService.InsertUploadLog(_LoginID, _FilePath, _TableName, _Success, _ServerFileName);
            if (_UploadLogError != "") _ListError.Add(_UploadLogError);
            if (_ListError.Count > 0) _ListError[0] = _ListError[0] + "ms";
            return _ListError;
        }
        /// <summary>
        /// according to SapMiddleData type batch insert data
        /// </summary>
        /// <param name="type">SapMiddleData type</param>
        /// <param name="sapMiddleData"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private List<string> miltiCreate(string type, SapMiddleData sapMiddleData, out string tableName)
        {
            tableName = "";
            List<string> _ListError = new List<string>();
            switch (type)
            {
                default:
                    _ListError = this._sap_PriceListService.MiltiCreate(sapMiddleData.PriceList);
                    tableName = "SAP_PriceList";
                    break;
            }
            return _ListError;
        }

        /// <summary>
        ///  use  adatper factory to produce  object
        /// </summary>
        /// <param name="filePath"></param>
        private SapMiddleData parse(string filePath)
        {
            SapBase _Base = _sapAdapterFactory.CreateSapAdapter("");
            _Base.PriceType= HttpContext.Current.Request["PriceType"].ToString() ;
            SapMiddleData _SapMiddleData = _Base.Parse(filePath);
            //if (_Base.ListError.Count() != 0)
            //{
            //    string _Error = string.Join("\r\n", _Base.ListError.ToArray());
            //    throw new Exception(_Error);
            //}
            return _SapMiddleData;
        }
    }
}