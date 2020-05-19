using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Service.Interface;
using WebApi.Models;
using WebApi.DataModel.CustomModel.Edi;
using Newtonsoft.Json;
namespace WebApi.Controllers
{ 
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EdiPosController : ApiController
    {
        private IPosExcelUploadService _posExcelUploadService;

        public EdiPosController(IPosExcelUploadService posExcelUploadService)
        {
            this._posExcelUploadService = posExcelUploadService;
        }
        /// <summary>
        /// upload file 
        /// </summary>
        /// <returns></returns>
        public List<string> Post()
        {
            this._posExcelUploadService.InsertButtonLog("Upload", "", "Edi/UploadFile.html");
            return this._posExcelUploadService.UploadFile();
        }
        /// <summary>
        /// show all pos data
        /// </summary>
        /// <returns></returns>
        public List<Edi_Pos> Get(string dateStart,string dateEnd)
        {
            List<Edi_Pos> _Edi_PosList = this._posExcelUploadService.GetEdi_PosList(dateStart,  dateEnd);
            this._posExcelUploadService.InsertButtonLog("Pos", dateStart+"_"+dateEnd, "Edi/UploadFile.html");
            return _Edi_PosList;
        }
        /// <summary>
        /// update all same name customer data 
        /// address
        /// market
        /// marketcode
        /// subsegment
        /// subsegmentcode
        /// </summary>
        /// <param name="edi_Pos"></param>
        /// <returns></returns>
        public bool Put(Edi_Pos edi_Pos)
        {
            bool _OK = false;
            this._posExcelUploadService.UpdateEdi_Pos(edi_Pos);
            _OK = true; 
            string _Remark = JsonConvert.SerializeObject(edi_Pos);
            this._posExcelUploadService.InsertButtonLog("Edit", _Remark, "Edi/UploadFile.html");
            return _OK;
        }
        public List<PosGroup> Get(string type,string quarter,string month,string group,string yy,string region,string groupKey)
        {
            List<PosGroup> _PosGroupList = this._posExcelUploadService.GetPosGroupList(  type,  quarter,   month,  group, "Series", "Country","Dist", "Market", yy,   region,   groupKey);
            this._posExcelUploadService.InsertButtonLog("Go", group, "Edi/UploadFile.html");
            return _PosGroupList;
        } 
    }
}
