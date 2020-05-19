using System;
using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface IBudget_LoginReviewReportService
    {
        string Create(Budget_LoginReviewReport instance);

        void Update(Budget_LoginReviewReport instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_LoginReviewReport GetByID(int Id);

        IEnumerable<Budget_LoginReviewReport> GetAll();

        Object GetReviewDepartment(string JsonStr);

    }
}
