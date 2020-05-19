using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Service.Interface.Table;
using WebApi.Models;
namespace WebApi.Controllers
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ItemCatalogController : ApiController
    {
        private IItemCatalogService _itemCatalogService;

        public ItemCatalogController(IItemCatalogService itemCatalogService)
        {
            this._itemCatalogService = itemCatalogService;
        }
        public List<ItemCatalog> Get(string className)
        {
            List<ItemCatalog> _ItemCatalog = this._itemCatalogService.GetAll().Where(x => x.ClassName == className&&x.IsDel==false).ToList();
            return _ItemCatalog;
        }
    }
}
