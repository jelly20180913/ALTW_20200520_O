using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.DataModel.CustomModel.Edi;
using System.Net.Http;
namespace WebApi.Service.Interface
{
   public interface IPosExcelUploadService
    {
        List<string> UploadFile();
        List<Edi_Pos> GetEdi_PosList(string dateStart, string dateEnd);
        bool UpdateEdi_Pos(Edi_Pos edi_Pos);
        List<PosGroup> GetPosGroupList(string type, string quarter, string month, string group, string series, string country, string dist, string market, string yy, string region, string groupKey);
        HttpResponseMessage GetEdi_PosListToExcel(string dateStart, string dateEnd);
        void InsertButtonLog(string buttonName, string remark,string page);
        bool Email(Edi_Pos edi_Pos);
    }
}
