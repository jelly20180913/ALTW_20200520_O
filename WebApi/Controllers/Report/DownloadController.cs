using System.Net.Http;
using System.Web.Http;
using WebApi.Service.Interface;
namespace WebApi.Controllers.Report
{

    public class DownloadController : ApiController
    {
        private IReportFileService _reportFileService;

        public DownloadController(IReportFileService reportFileService)
        {
            this._reportFileService = reportFileService;
        }
        public HttpResponseMessage GetReportFile()
        { 
            return this._reportFileService.GetReportFile();
        }

    }
}
