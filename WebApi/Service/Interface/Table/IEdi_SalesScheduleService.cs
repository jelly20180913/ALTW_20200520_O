using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IEdi_SalesScheduleService
    {
        string Create(Edi_SalesSchedule instance);

        void Update(Edi_SalesSchedule instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Edi_SalesSchedule GetByID(int Id);

        IEnumerable<Edi_SalesSchedule> GetAll(); 

    }
}
