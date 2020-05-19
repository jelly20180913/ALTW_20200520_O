using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
namespace WebApi.Service.Interface
{
    public interface IPosDataService
    {
        void Create(PosData instance);

        void Update(PosData instance);

        void Delete(int Id);

        bool IsExists(int Id);

        PosData GetByID(int Id);

        IEnumerable<PosData> GetAll();

        List<string> MiltiCreate(IQueryable<PosData> instance, int fK_LoginId);
    }
}
