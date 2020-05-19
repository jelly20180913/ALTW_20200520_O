using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IEdi_SalesHeaderService
    {
        string Create(Edi_SalesHeader instance);

        void Update(Edi_SalesHeader instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Edi_SalesHeader GetByID(int Id);

        IEnumerable<Edi_SalesHeader> GetAll(); 

    }
}
