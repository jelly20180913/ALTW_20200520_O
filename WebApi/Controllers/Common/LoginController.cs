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
namespace WebApi.Controllers
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        private ICommonService _commonService;

        public LoginController(ICommonService commonService)
        {
            this._commonService = commonService;
        }
        public bool Post([FromBody]Login login )
        {
            bool _Success = false;
            this._commonService.UpdatePassword(login.Id, login.Password);
            _Success = true;
            return _Success;
        }
    }
}
