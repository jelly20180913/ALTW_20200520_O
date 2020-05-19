using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Service.Interface;
using WebApi.DataModel.CustomModel.Budget;
namespace WebApi.Controllers
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BudgetLoginUploadReportController : ApiController
    {
        private IBudgetExcelUploadService _budgetExcelUploadService;

        public BudgetLoginUploadReportController(IBudgetExcelUploadService budgetExcelUploadService)
        {
            this._budgetExcelUploadService = budgetExcelUploadService;
        } 
        public List<DropDownDepartment> Get(string account, string factory)
        {
            return this._budgetExcelUploadService.GetDepartmentByAccount(account, factory); 
        }
    }
}
