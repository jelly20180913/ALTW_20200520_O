using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Service.Interface;
using System.IO;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models.Repository.EDI.Implement;
using WebApi.Models;
using System.Data;
using LinqToExcel;
using WebApi.Common.BudgetAdapter;
using WebApi.Service.Interface.Table;
using WebApi.Service.Implement.Table;
using WebApi.Service.Interface.Common;
using WebApi.DataModel.CustomModel.Budget;
using Newtonsoft.Json;

namespace WebApi.Service.Implement
{
    public class JsonAccount
    {
        public string Account { get; set; }
        public string Report { get; set; }
    }

    public class BudgetLoginReviewService : IBudgetLoginReviewService
    {
        private IBudget_LoginReviewReportService _budget_LoginReviewReportService;

        public BudgetLoginReviewService(IBudget_LoginReviewReportService budget_LoginReviewReportService)
        {
            this._budget_LoginReviewReportService = budget_LoginReviewReportService;
        }

        public Object GetLoginReviewDepartment(string JsonStr)
        {
            return this._budget_LoginReviewReportService.GetReviewDepartment(JsonStr);


        }

        //public List<LoginReport> GetLoginReport(string account)
        //{
        //    List<LoginReport> ls = this._budget_LoginPermissionService.GetDepartmentReportByAccount(account);
        //    return ls;
        //}

    }
}