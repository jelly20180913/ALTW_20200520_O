using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Service.Interface;
using WebApi.DataModel.CustomModel.Budget;
namespace WebApi.Controllers
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BudgetExcelUploadController : ApiController
    {
        private IBudgetExcelUploadService _budgetExcelUploadService;

        public BudgetExcelUploadController(IBudgetExcelUploadService budgetExcelUploadService)
        {
            this._budgetExcelUploadService = budgetExcelUploadService;
        }
        /// <summary>
        /// upload file 
        /// </summary>
        /// <returns></returns>
        public List<string> Post()
        { 
            return  this._budgetExcelUploadService.UploadFile();
        } 
    }
}
