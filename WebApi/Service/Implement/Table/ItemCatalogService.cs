using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
using WebApi.Service.Interface.Table;
namespace WebApi.Service.Implement
{
    public class ItemCatalogService : IItemCatalogService
    {
        private IRepository<ItemCatalog> _repository;
        public ItemCatalogService(IRepository<ItemCatalog> repository)
        {
            this._repository = repository;
        }
        public void Create(ItemCatalog instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            this._repository.Create(instance);
        }

        public void Update(ItemCatalog instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            this._repository.Update(instance);
        }

        public void Delete(int Id)
        {
            var instance = this.GetByID(Id);
            this._repository.Delete(instance);
        }

        public bool IsExists(int Id)
        {
            return this._repository.GetAll().Any(x => x.Id == Id);
        }

        public ItemCatalog GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<ItemCatalog> GetAll()
        {
            return this._repository.GetAll();
        }
        public List<string> MiltiCreate(List<ItemCatalog> instance)
        {
            List<string> _ListError = new List<string>();
            _ListError = this._repository.CreateBatch(instance);
            return _ListError;
        }
        public ItemCatalog GetByItemIdInClassName(string itemId,string className)
        {
            return this._repository.Get(x => x.ItemId == itemId&&x.ClassName==className);
        }
    }
}