using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IEdi_SalesSchedule_855Service
    {
        string Create(Edi_SalesSchedule_855 instance);

        void Update(Edi_SalesSchedule_855 instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Edi_SalesSchedule_855 GetByID(int Id);

        IEnumerable<Edi_SalesSchedule_855> GetAll(); 

    }
}
