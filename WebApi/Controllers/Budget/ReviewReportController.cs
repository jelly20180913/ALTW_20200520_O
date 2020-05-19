using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using WebApi.Service.Interface;

namespace WebApi.Controllers.Budget
{
    public class MyQueryJson
    {
        public string Account { get; set; }
        public string Report { get; set; }
        public string BudgetYear { get; set; }
        public string[] LTDept { get; set; }
        public string[] KSZDept { get; set; }
        public string QueryGroupby { get; set; }
        public string TBH { get; set; }
    }
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReviewReportController : ApiController
    {
        private IBudgetReviewReportService _budgetReviewReportService;

        public ReviewReportController(IBudgetReviewReportService budgetReviewReportService)
        {
            this._budgetReviewReportService = budgetReviewReportService;
        }

        // GET: api/ReviewReport
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ReviewReport/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ReviewReport
        public Object Post([FromBody]MyQueryJson JsonObj)
        {
            string JsonStr = JsonConvert.SerializeObject(JsonObj, Formatting.Indented);
            return this._budgetReviewReportService.GetReviewReport(JsonStr);
        }

        // PUT: api/ReviewReport/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ReviewReport/5
        public void Delete(int id)
        {
        }
    }
}
