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
    public class EdiCustomerController : ApiController
    {
        private IEdiService _ediService;
        public EdiCustomerController(IEdiService ediService)
        {
            this._ediService = ediService;
        }
        public List<Edi_Customer> Get(bool mode)
        {
            bool _Mode = Convert.ToBoolean(mode);
            List<Edi_Customer> _Edi_CustomerList = this._ediService.GetEdi_CustomerList(_Mode);
            return _Edi_CustomerList;
        }
    }
}