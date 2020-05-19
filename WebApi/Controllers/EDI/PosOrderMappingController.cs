using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Service.Interface.Table;
using WebApi.Models;
namespace WebApi.Controllers.EDI
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PosOrderMappingController : ApiController
    {
        private IPosOrderMappingService _posOrderMappingService;

        public PosOrderMappingController(IPosOrderMappingService posOrderMappingService)
        {
            this._posOrderMappingService = posOrderMappingService;
        }
        public List<PosOrderMapping> Get()
        { 
            return this._posOrderMappingService.GetAll().ToList();
        }
    }
}
