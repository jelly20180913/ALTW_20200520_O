using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface IBudget_CostTravelingMappingService
    {
        string Create(Budget_CostTravelingMapping instance);

        void Update(Budget_CostTravelingMapping instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_CostTravelingMapping GetByID(int Id);

        IEnumerable<Budget_CostTravelingMapping> GetAll();

        List<string> MiltiCreate(List<Budget_CostTravelingMapping> instance);
    }
}
