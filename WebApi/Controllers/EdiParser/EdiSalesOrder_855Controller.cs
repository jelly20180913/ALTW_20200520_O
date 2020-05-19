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
    public class EdiSalesOrder_855Controller : ApiController
    {
        private IEdiService _ediService;
        public EdiSalesOrder_855Controller(IEdiService ediService)
        {
            this._ediService = ediService;
        }
        public bool PostEdiSalesOrder_855(Edi_SalesOrder_855 sapSalesOrder_855)
        { 
             this._ediService.InsertEdi_SalesOrder(sapSalesOrder_855);
            return true;
        }
    }
}