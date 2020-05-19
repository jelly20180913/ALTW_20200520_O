using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface IPosOrderMappingService
    {
        void Create(PosOrderMapping instance);

        void Update(PosOrderMapping instance);

        void Delete(int Id);

        bool IsExists(int Id);

        PosOrderMapping GetByID(int Id);

        IEnumerable<PosOrderMapping> GetAll();

        PosOrderMapping GetByModel(int model);
    }
}
