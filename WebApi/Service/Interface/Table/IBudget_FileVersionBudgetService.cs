using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface IBudget_FileVersionBudgetService
    {
        string Create(Budget_FileVersionBudget instance);

        void Update(Budget_FileVersionBudget instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_FileVersionBudget GetByID(int Id);

        IEnumerable<Budget_FileVersionBudget> GetAll();
        Budget_FileVersionBudget GetLastVersion(string itemId, string departmentId, string factory, string date);
        Budget_FileVersionBudget GetLastVersion(string itemId , string date, string factory);
    }
}
