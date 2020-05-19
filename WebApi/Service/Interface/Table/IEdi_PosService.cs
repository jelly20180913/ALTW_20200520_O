using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public  interface IEdi_PosService
    {
        void Create(Edi_Pos instance);

        void Update(Edi_Pos instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Edi_Pos GetByID(int Id);

        IEnumerable<Edi_Pos> GetAll();
        List<string> MiltiCreate(IQueryable<Edi_Pos> instance, int fK_LoginId);
    }
}
