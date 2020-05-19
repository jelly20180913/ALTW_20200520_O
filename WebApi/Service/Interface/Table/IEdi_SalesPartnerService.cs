using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IEdi_SalesPartnerService
    {
        string Create(Edi_SalesPartner instance);

        void Update(Edi_SalesPartner instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Edi_SalesPartner GetByID(int Id);

        IEnumerable<Edi_SalesPartner> GetAll(); 

    }
}
