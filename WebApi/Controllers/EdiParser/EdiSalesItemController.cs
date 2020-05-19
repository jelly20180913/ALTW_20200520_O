using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http; 
using System.Web.Http.Cors;
using WebApi.Service.Interface;
using WebApi.Models;
using WebApi.DataModel.CustomModel.Edi;
using Newtonsoft.Json;
namespace WebApi.Controllers.EDI
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EdiSalesItemController : ApiController
    {
        private IEdiService _ediService;
        public EdiSalesItemController(IEdiService ediService)
        {
            this._ediService = ediService;
        }
        public List<Edi_SalesItem> Get(string orderNumber)
        {
            List<Edi_SalesItem> _Edi_SalesItemList = this._ediService.GetEdi_SalesItemList(  orderNumber);
            return _Edi_SalesItemList;
        }
    }
}