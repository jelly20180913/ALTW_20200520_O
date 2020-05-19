using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IBudget_DeptExpenseService
    {
        string Create(Budget_DeptExpense instance);

        void Update(Budget_DeptExpense instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_DeptExpense GetByID(int Id);

        IEnumerable<Budget_DeptExpense> GetAll();
        List<string> MiltiCreate(List<Budget_DeptExpense> instance);

    }
}
