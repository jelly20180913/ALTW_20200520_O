using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Models.CustomModel;
using NLog;
using WebApi.ThirdParty.SAP;
using WebApi.DataModel.CustomModel.SAP;
using WebApi.Service.Interface;
namespace WebApi.Controllers.RFC
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BapiOrderController : ApiController
    {
        private ISapEDIService _sapEDIService;
        private ISapConnectorInterface _sapConnectorInterface;
        private IEdiService _ediSerivice;
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public BapiOrderController(ISapEDIService sapEDIService, ISapConnectorInterface sapConnectorInterface,IEdiService ediService)
        {
            this._sapEDIService = sapEDIService;
            this._sapConnectorInterface = sapConnectorInterface;
            this._ediSerivice = ediService;
        }
        // POST: api/Index
        /// <summary>
        /// create order 
        /// send email to responsible sales
        /// </summary>
        /// <param name="sapSalesOrder"></param>
        /// <returns></returns>
        public string PostInsertOrder(SapSalesOrder sapSalesOrder)
        {
            string _OrderNumber = "";
            _OrderNumber = this._sapConnectorInterface.CreateOrder(sapSalesOrder);
            if (_OrderNumber != "") this._sapEDIService.Email(_OrderNumber, sapSalesOrder);
            this._ediSerivice.InsertSapSalesOrder(sapSalesOrder,_OrderNumber);
            return _OrderNumber;
        }
    }
}
