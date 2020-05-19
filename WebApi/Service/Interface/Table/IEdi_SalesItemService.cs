using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IEdi_SalesItemService
    {
        string Create(Edi_SalesItem instance);

        void Update(Edi_SalesItem instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Edi_SalesItem GetByID(int Id);

        IEnumerable<Edi_SalesItem> GetAll(); 

    }
}
