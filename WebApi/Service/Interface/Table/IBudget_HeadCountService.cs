using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IBudget_HeadCountService
    {
        string Create(Budget_HeadCount instance);

        void Update(Budget_HeadCount instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_HeadCount GetByID(int Id);

        IEnumerable<Budget_HeadCount> GetAll();
        List<string> MiltiCreate(List<Budget_HeadCount> instance);

    }
}
