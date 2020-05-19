using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public  interface IBudget_HeadCountHRService
    {
        void Create(Budget_HeadCountHR instance);

        void Update(Budget_HeadCountHR instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_HeadCountHR GetByID(int Id);

        IEnumerable<Budget_HeadCountHR> GetAll();
        List<string> MiltiCreate(List<Budget_HeadCountHR> instance );
        Budget_HeadCountHR GetByName(string AltwName);
    }
}
