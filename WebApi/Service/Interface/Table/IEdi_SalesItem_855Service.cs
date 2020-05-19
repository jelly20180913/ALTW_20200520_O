using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IEdi_SalesItem_855Service
    {
        string Create(Edi_SalesItem_855 instance);

        void Update(Edi_SalesItem_855 instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Edi_SalesItem_855 GetByID(int Id);

        IEnumerable<Edi_SalesItem_855> GetAll(); 

    }
}
