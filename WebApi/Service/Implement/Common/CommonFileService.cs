using System;
using System.Linq;
using System.Web;
using WebApi.Service.Interface;
using System.IO;
using WebApi.Models;
using System.Data;
using WebApi.Service.Interface.Common;
using System.Globalization;

namespace WebApi.Service.Implement.Common
{
    public class CommonFileService : ICommonFileService
    {
        private ILoginService _loginService;
        private IUploadLogService _uploadLogService;
        public CommonFileService(ILoginService loginService, IUploadLogService uploadLogSerivice)
        {
            this._loginService = loginService;
            this._uploadLogService = uploadLogSerivice;
        }
        /// <summary>
        /// copy file to /UploadedFiles   path
        /// return path
        /// </summary>
        /// <returns></returns>
        public string Upload()
        {
            if (!HttpContext.Current.Request.Files.AllKeys.Any()) throw new Exception("No file found to upload");
            // Get the uploaded image from the Files collection
            var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];
            if (httpPostedFile == null) throw new Exception("Could not get the uploaded file");
            // Validate the uploaded image(optional) 
            // Get the complete file path
            string pathString = HttpContext.Current.Server.MapPath("~/UploadedFiles");
            if (!Directory.Exists(pathString))
            {
                Directory.CreateDirectory(pathString);
            }
            string fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), Path.GetFileNameWithoutExtension(httpPostedFile.FileName) + Path.GetExtension(httpPostedFile.FileName));
            // Save the uploaded file to "UploadedFiles" folder
            httpPostedFile.SaveAs(fileSavePath);
            return fileSavePath;
        }

        /// <summary>
        /// copy file to success  directory
        /// EX:~/Success/(ALTW)/(PosData)/2018-10-03_xxxxxxxxxxxxxxxxx.xls
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="sourceFile"></param>
        /// <param name="tableName"></param>
        public string SaveToSuccess(int loginId, string sourceFile, string tableName)
        {
            Login _Login = _loginService.GetByID(loginId);
            string _FileName = "";
            if (_Login != null)
            {
                string pathString = HttpContext.Current.Server.MapPath("~/Success");
                if (!Directory.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString);
                }
                string _CustomerFilePath = pathString + "/" + _Login.CustomerName;
                if (!Directory.Exists(_CustomerFilePath))
                {
                    Directory.CreateDirectory(_CustomerFilePath);
                }
                string _CustomerFileTablePath = _CustomerFilePath + "/" + tableName;
                if (!Directory.Exists(_CustomerFileTablePath))
                {
                    Directory.CreateDirectory(_CustomerFileTablePath);
                }
                string _Ext = Path.GetExtension(sourceFile);
                _FileName = _CustomerFileTablePath + "/" + DateTime.Now.ToString("yyyy-MM-dd") + "_" + Guid.NewGuid().ToString("N") + _Ext;
                System.IO.File.Copy(sourceFile, _FileName, true);
            }
            return _FileName;
        }
        /// <summary>
        /// get Q1 Q2 Q3 Q4
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public string GetQuarter(int month)
        {
            string _Quarter = "";
            if (month > 0 && month < 4)
                _Quarter = "Q1";
            else if (month > 3 && month < 7)
                _Quarter = "Q2";
            else if (month > 6 && month < 10)
                _Quarter = "Q3";
            else if (month > 9 && month < 13)
                _Quarter = "Q4";
            return _Quarter;
        }
        /// <summary>
        /// get country english name by code
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public string GetCountryEngName(string countryCode)
        {
            CultureInfo[] _CultureInfoArray = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            string _EngName = _CultureInfoArray.Where(x => x.Name == countryCode).FirstOrDefault().EnglishName;
            return _EngName;
        }
        /// <summary>
        /// insert uploadlog 
        /// </summary>
        /// <param name="loginID"></param>
        /// <param name="fileSavePath"></param>
        /// <param name="tableName">PosData</param>
        /// <returns></returns>
        public string InsertUploadLog(int loginID, string fileSavePath, string tableName, bool success, string serverFileName)
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
        /// process excel minus number
        /// </summary>
        /// <param name="input_number"></param>
        /// <returns></returns>
        public int GetExcelMinusNumber(string input_number)
        {
            int _MinusNumber = 0;
            if (input_number != null)
            {
                if (input_number.StartsWith("(") && input_number.EndsWith(")"))
                {
                    _MinusNumber= int.Parse(input_number.Replace("(", "").Replace(")", ""), System.Globalization.NumberStyles.AllowThousands);
                    _MinusNumber = _MinusNumber * (-1);
                }
                else
                {
                    _MinusNumber = int.Parse(input_number, System.Globalization.NumberStyles.AllowThousands);
                }
            }
           return _MinusNumber;
        }
    }
}