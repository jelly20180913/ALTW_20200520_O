using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Service.Interface;
using WebApi.Models;
namespace WebApi.Controllers
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BudgetDepartmentReportController : ApiController
    {
        private IBudgetExcelUploadService _budgetExcelUploadService;

        public BudgetDepartmentReportController(IBudgetExcelUploadService budgetExcelUploadService)
        {
            this._budgetExcelUploadService = budgetExcelUploadService;
        }
        public List<Budget_DepartmentReport> Get( string factory, string departmentId)
        {
            return this._budgetExcelUploadService.GetBudgetTypeByDepartmentId( factory,departmentId);
        }
        public List<Budget_DepartmentReport> Get( )
        {
            return this._budgetExcelUploadService.GetDepartment();
        }
    }
}
