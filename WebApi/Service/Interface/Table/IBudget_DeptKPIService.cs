using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IBudget_DeptKPIService
    {
        string Create(Budget_DeptKPI instance);

        void Update(Budget_DeptKPI instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_DeptKPI GetByID(int Id);

        IEnumerable<Budget_DeptKPI> GetAll();
        List<string> MiltiCreate(List<Budget_DeptKPI> instance);

    }
}
