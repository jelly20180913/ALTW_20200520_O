using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface IBudget_DepartmentReportService
    {
        string Create(Budget_DepartmentReport instance);

        void Update(Budget_DepartmentReport instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_DepartmentReport GetByID(int Id);

        IEnumerable<Budget_DepartmentReport> GetAll();

    }
}
