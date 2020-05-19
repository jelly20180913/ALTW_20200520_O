using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface IBudget_LoginReviewCommonCostService
    {
        string Create(Budget_LoginReviewCommonCost instance);

        void Update(Budget_LoginReviewCommonCost instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_LoginReviewCommonCost GetByID(int Id);

        IEnumerable<Budget_LoginReviewCommonCost> GetAll();

    }
}
