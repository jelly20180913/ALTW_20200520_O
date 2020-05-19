using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Service.Interface;

namespace WebApi.Controllers.SAP
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SAPExcelUploadController : ApiController
    {
        private ISapExcelUploadService _sapExcelUploadService;

        public SAPExcelUploadController(ISapExcelUploadService sapExcelUploadService)
        {
            this._sapExcelUploadService = sapExcelUploadService;
        }
        /// <summary>
        /// upload file 
        /// </summary>
        /// <returns></returns>
        public List<string> Post()
        {
            return this._sapExcelUploadService.UploadFile();
        }
    }
}