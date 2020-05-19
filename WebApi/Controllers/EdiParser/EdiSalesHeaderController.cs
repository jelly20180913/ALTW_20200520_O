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
    public class EdiSalesHeaderController : ApiController
    {
        private IEdiService _ediService;
        public EdiSalesHeaderController(IEdiService ediService)
        {
            this._ediService = ediService;
        }
        public List<Edi_SalesHeader> Get( )
        {
            List<Edi_SalesHeader> _Edi_SalesHeaderList = this._ediService.GetEdi_SalesHeaderList();
            return _Edi_SalesHeaderList;
        }
    }
}