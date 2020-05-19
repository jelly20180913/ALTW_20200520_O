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
    public class EdiSalesScheduleController : ApiController
    {
        private IEdiService _ediService;
        public EdiSalesScheduleController(IEdiService ediService)
        {
            this._ediService = ediService;
        }
        public List<Edi_SalesSchedule> Get(string orderNumber)
        {
            List<Edi_SalesSchedule> _Edi_SalesScheduleList = this._ediService.GetEdi_SalesScheduleList(  orderNumber);
            return _Edi_SalesScheduleList;
        }
    }
}