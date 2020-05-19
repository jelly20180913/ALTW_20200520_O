using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IBudget_ScrapService
    {
        string Create(Budget_Scrap instance);

        void Update(Budget_Scrap instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_Scrap GetByID(int Id);

        IEnumerable<Budget_Scrap> GetAll();
        List<string> MiltiCreate(List<Budget_Scrap> instance);

    }
}
