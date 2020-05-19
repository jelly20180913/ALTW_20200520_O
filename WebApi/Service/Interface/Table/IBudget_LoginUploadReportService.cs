using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface IBudget_LoginUploadReportService
    {
        string Create(Budget_LoginUploadReport instance);

        void Update(Budget_LoginUploadReport instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_LoginUploadReport GetByID(int Id);

        IEnumerable<Budget_LoginUploadReport> GetAll();

    }
}
