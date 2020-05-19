using System.Net.Http;
using System.Web.Http;
using WebApi.Service.Interface;  
namespace WebApi.Controllers 
{

    public class DownloadPosController : ApiController
    {
        private IPosExcelUploadService _posExcelUploadService;

        public DownloadPosController(IPosExcelUploadService posExcelUploadService)
        {
            this._posExcelUploadService = posExcelUploadService;
        }
        /// <summary>
        /// download pos excel
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public HttpResponseMessage GetEdi_PosListToExcel(string dateStart, string dateEnd)
        {
            this._posExcelUploadService.InsertButtonLog("Excel", dateStart+"_"+ dateEnd,"Edi/UploadFile.html");
            return this._posExcelUploadService.GetEdi_PosListToExcel(  dateStart,  dateEnd);
        }

    }
}
