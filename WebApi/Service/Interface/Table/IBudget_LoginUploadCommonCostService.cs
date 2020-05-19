using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface IBudget_LoginUploadCommonCostService
    {
        string Create(Budget_LoginUploadCommonCost instance);

        void Update(Budget_LoginUploadCommonCost instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_LoginUploadCommonCost GetByID(int Id);

        IEnumerable<Budget_LoginUploadCommonCost> GetAll();

    }
}
