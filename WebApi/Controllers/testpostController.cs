using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.CustomModel;
using Jose;
using System.Text;
using WebApi.Service;
using System.Web.Http.Cors;
using WebApi.Service.Implement.Table;
using WebApi.Service.Interface;
using WebApi.Models;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]


    public class MyJson
    {
        public string account { get; set; }
        public string pwd { get; set; }
    }

    public class testpostController : ApiController
    {
        //private IBudgetLoginReportService _budgetLoginReportService;

        //public testpostController(IBudgetLoginReportService budgetLoginReportService)
        //{
        //    this._budgetLoginReportService = budgetLoginReportService;
        //}

        //public List<LoginReport> Post([FromBody]MyJson obj)
        //{

        //    //return obj.account + "," + obj.pwd;

        //    //return this._budgetLoginReportService.GetLoginReport(obj.account);


        //}

        //public List<LoginReport> Get([FromBody] MyJson obj)
        //{
        //    string account = obj.account;
        //    //List<LoginReport> lp = this._budgetLoginReportService.GetLoginReport(account);
        //    //for (int i = 0; i < lp.Count; i++)
        //    //{
        //    //    string dn = lp[i].DepartmentName;
        //    //}
        //    return lp;

        //}
    }
}
