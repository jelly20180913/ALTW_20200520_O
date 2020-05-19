using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IEdi_CustomerService
    {
        string Create(Edi_Customer instance);

        void Update(Edi_Customer instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Edi_Customer GetByID(int Id);

        IEnumerable<Edi_Customer> GetAll(); 

    }
}
