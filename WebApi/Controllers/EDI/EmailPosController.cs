using System.Net.Http;
using System.Web.Http;
using WebApi.Service.Interface;
using WebApi.Models; 
using System.Web.Http.Cors;
using Newtonsoft.Json;
namespace WebApi.Controllers 
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmailPosController : ApiController
    {
        private IPosExcelUploadService _posExcelUploadService;

        public EmailPosController(IPosExcelUploadService posExcelUploadService)
        {
            this._posExcelUploadService = posExcelUploadService;
        }
        public bool Post(Edi_Pos edi_Pos)
        {
            bool _OK = false;
            this._posExcelUploadService.Email(edi_Pos);
            _OK = true;
            string _Remark = JsonConvert.SerializeObject(edi_Pos);
            this._posExcelUploadService.InsertButtonLog("Feedback", _Remark, "Edi/UploadFile.html");
            return _OK;
        }

    }
}
