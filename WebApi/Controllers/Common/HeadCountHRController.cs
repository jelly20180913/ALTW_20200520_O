using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Service.Interface.Table;
using WebApi.Models;
using WebApi.Service.Interface.Common;
using WebApi.Models.CustomModel;
using WebApi.Service.Interface;
using WebApi.DataModel.CustomModel.Budget;
namespace WebApi.Controllers.Common
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HeadCountHRController : ApiController
    {
        private IBudgetExcelUploadService _budgetExcelUploadService;

        public HeadCountHRController(IBudgetExcelUploadService budgetExcelUploadService)
        {
            this._budgetExcelUploadService = budgetExcelUploadService;
        }
        public List<HeadCountHR> Get()
        {
            List<HeadCountHR> _HeadCountHRList = this._budgetExcelUploadService.GetHeadCountHRList();
            return _HeadCountHRList;
        }
        public bool Put(HeadCountHR headCountHR)
        {
            bool _OK = false; 
            this._budgetExcelUploadService.UpdateHeadCount(headCountHR);
            this._budgetExcelUploadService.UpdateHeadCountHR(headCountHR);
            _OK = true;
            return _OK;
        }
    }
}
