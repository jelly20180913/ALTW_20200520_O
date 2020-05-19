using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Service.Interface;
using WebApi.Models;
namespace WebApi.Controllers
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportFileController : ApiController
    {
        private IReportFileService _reportFileService;

        public ReportFileController(IReportFileService reportFileService)
        {
            this._reportFileService = reportFileService;
        }
        /// <summary>
        /// upload file 
        /// </summary>
        /// <returns></returns>
        public List<string> Post()
        { 
            return  this._reportFileService.UploadFile();
        }
        public List<ReportFile> Get(string date)
        {
            return this._reportFileService.GetReportFileByDate(date);
        }
    }
}
