using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface IReportFileService
    {
        string Create(ReportFile instance);

        void Update(ReportFile instance);

        void Delete(int Id);

        bool IsExists(int Id);

        ReportFile GetByID(int Id);

        IEnumerable<ReportFile> GetAll();

        IEnumerable<ReportFile> GetAllByDate(string date);

        ReportFile GetByFileNameAndDate(string fileName, string date, string flag);
    }
}
