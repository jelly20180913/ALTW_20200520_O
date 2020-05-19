using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IEdi_SalesHeader_855Service
    {
        string Create(Edi_SalesHeader_855 instance);

        void Update(Edi_SalesHeader_855 instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Edi_SalesHeader_855 GetByID(int Id);

        IEnumerable<Edi_SalesHeader_855> GetAll(); 

    }
}
