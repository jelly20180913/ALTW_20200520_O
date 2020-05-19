using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Service.Interface;
using WebApi.Models;
using WebApi.DataModel.CustomModel.SAP;
namespace WebApi.Controllers
{
    //[JwtAuthActionForCustomer]
    //[JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SAP_PriceListController : ApiController
    {
        private ISapEDIService _sapEDIService;

        public SAP_PriceListController(ISapEDIService sapEDIService)
        {
            this._sapEDIService = sapEDIService;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SAP_PriceList> Get(string date)
        {
            return this._sapEDIService.GetSAP_PriceLists(date);
        }
        public IEnumerable<SAP_PriceList> GetExcludeTax()
        {
            return this._sapEDIService.GetSAP_PriceListsExcludeTax();
        }
        public bool PostSapMiddleData(SapMiddleData sapMiddleData)
        {
            bool _OK = false;
            _OK = this._sapEDIService.BatchInsert(sapMiddleData);
            return _OK;
        }
    }
}