using System.Collections.Generic;
using WebApi.Models;
using System.Net.Http;
namespace WebApi.Service.Interface
{
    public interface IReportFileService
    {
        List<string> UploadFile();
        List<ReportFile> GetReportFileByDate(string date);
        HttpResponseMessage GetReportFile();
    }
}
