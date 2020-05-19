using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Models.CustomModel;
using NLog;
using WebApi.ThirdParty.SAP;
using WebApi.DataModel.CustomModel.SAP;
namespace WebApi.Controllers
{
    // [JwtAuthActionFilter]
    //[JwtAuthActionForCustomer]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class IndexController : ApiController
    {
        private ISapService _sapService;
        private ISapConnectorInterface _sapConnectorInterface;
        public IndexController(ISapService sapService, ISapConnectorInterface sapConnectorInterface)
        {
            this._sapService = sapService;
            this._sapConnectorInterface = sapConnectorInterface;
        }
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// call RFC to get budget table
        /// </summary>
        /// <param name="id">P180001CLB</param>
        /// <param name="kokrs">1000</param>
        /// <returns> </returns>
        public WebApi.Models.CustomModel.SAP.Budget Get(string id, string kokrs)
        {
            WebApi.Models.CustomModel.SAP.Budget _budget = this._sapService.budgetStart(id, kokrs);
            return _budget;
        }
        //GET: api/Index/5
        public string Get(int id)
        {

            // WebApi.Models.CustomModel.SAP.Budget _budget = this._sapService.budgetStart("P180001CLB", "1000");
            //this._sapService.Sap_CreateOrder( );
            // this._sapConnectorInterface.CreateOrder("QAS");
            // string _BusinessPartnerNumber = this._sapConnectorInterface.CreateBusinessPartner();

            //create bp
            string _BusinessPartnerNumber=this._sapConnectorInterface.CreateBP();//test ok
            this._sapConnectorInterface.CreateSalesArea(_BusinessPartnerNumber);
            this._sapConnectorInterface.CreateCustomer(_BusinessPartnerNumber);
            logger.Log(LogLevel.Error, "test");
            //create bp
            return "ok"; 
        }
        // POST: api/Index
        public string PostInsertOrder(SapSalesOrder sapSalesOrder)
        {
            string _OrderNumber = "";
            _OrderNumber = this._sapConnectorInterface.CreateOrder(sapSalesOrder);
            return _OrderNumber;
        }
        // PUT: api/Index/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Index/5
        public void Delete(int id)
        {
        }
    }
}
