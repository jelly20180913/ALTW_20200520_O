using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface
{
    public interface IPosColumnMapService
    {
        void Create(PosColumnMap instance);

        void Update(PosColumnMap instance);

        void Delete(int Id);

        bool IsExists(int Id);

        PosColumnMap GetByID(int Id);

        IEnumerable<PosColumnMap> GetAll();

    }
}
