using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.DataModel.CustomModel.Budget;
using WebApi.Service.Interface;
namespace WebApi.Controllers
{
    [JwtAuthActionFilter]
   //[AllowAnonymous]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BudgetFileVersionBudgetController : ApiController
    {
        private IBudgetExcelUploadService _budgetExcelUploadService;

        public BudgetFileVersionBudgetController(IBudgetExcelUploadService budgetExcelUploadService)
        {
            this._budgetExcelUploadService = budgetExcelUploadService;
        }
        public bool Put([FromBody]FileVersionBudget fileVersionBudet)
        {
            return this._budgetExcelUploadService.UpdateFileVersionBudgetApprove(fileVersionBudet.Id);
        }
        public List<FileVersionBudget> Get( )
        {
            List<FileVersionBudget> _FileVersionBudgetList = this._budgetExcelUploadService.GetFileVersionBudgetList();
            return _FileVersionBudgetList;
        }
    }
}
