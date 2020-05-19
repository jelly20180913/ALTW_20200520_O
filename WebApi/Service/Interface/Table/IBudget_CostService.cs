using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface IBudget_CostService
    {
        string Create(Budget_Cost instance);

        void Update(Budget_Cost instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_Cost GetByID(int Id);

        IEnumerable<Budget_Cost> GetAll();
        List<string> MiltiCreate(List<Budget_Cost> instance);
    }
}
