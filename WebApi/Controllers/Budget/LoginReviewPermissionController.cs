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
    public class MyJson
    {
        public string Account { get; set; }
        public string Report { get; set; }
    }
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginReviewPermissionController : ApiController
    {

        private IBudgetLoginReviewService _budgetLoginReviewService;

        public LoginReviewPermissionController(IBudgetLoginReviewService budgetLoginReviewService)
        {
            this._budgetLoginReviewService = budgetLoginReviewService;
        }

        // GET: api/LoginReviewPermission
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LoginReviewPermission/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LoginReviewPermission
        public Object Post([FromBody]MyJson JsonObj)
        {
            string JsonStr = JsonConvert.SerializeObject(JsonObj, Formatting.Indented);
            //string retstr = this._budgetLoginReviewService.GetLoginReviewDepartment(JsonStr);
            return this._budgetLoginReviewService.GetLoginReviewDepartment(JsonStr);
            //return "OK";
        }

        // PUT: api/LoginReviewPermission/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LoginReviewPermission/5
        public void Delete(int id)
        {
        }
    }
}
