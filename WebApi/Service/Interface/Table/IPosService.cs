using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public  interface IPosService
    {
        void Create(Pos instance);

        void Update(Pos instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Pos GetByID(int Id);

        IEnumerable<Pos> GetAll();
        List<string> MiltiCreate(IQueryable<Pos> instance, int fK_LoginId);
    }
}
