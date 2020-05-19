using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface IItemCatalogService
    {
        void Create(ItemCatalog instance);

        void Update(ItemCatalog instance);

        void Delete(int Id);

        bool IsExists(int Id);

        ItemCatalog GetByID(int Id);

        IEnumerable<ItemCatalog> GetAll();
        List<string> MiltiCreate(List<ItemCatalog> instance);
        ItemCatalog GetByItemIdInClassName(string itemId, string className);
    }
}
