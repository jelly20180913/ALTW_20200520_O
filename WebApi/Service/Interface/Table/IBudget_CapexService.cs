using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IBudget_CapexService
    {
        string Create(Budget_Capex instance);

        void Update(Budget_Capex instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_Capex GetByID(int Id);

        IEnumerable<Budget_Capex> GetAll();
        List<string> MiltiCreate(List<Budget_Capex> instance);

    }
}
