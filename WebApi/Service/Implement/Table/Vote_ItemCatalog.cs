using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface.Table;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
using WebApi.Controllers.Budget;
using Newtonsoft.Json;

namespace WebApi.Service.Implement.Table
{
    public class Vote_ItemCatalogService : IVote_ItemCatalogService
    {
        private IRepository<Vote_ItemCatalog> _repository;

        public Vote_ItemCatalogService(IRepository<Vote_ItemCatalog> repository)
        {
            this._repository = repository;
        }
        public string Create(Vote_ItemCatalog instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(Vote_ItemCatalog instance)
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

        public Vote_ItemCatalog GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<Vote_ItemCatalog> GetAll()
        {
            return this._repository.GetAll();
        }
        public List<string> MiltiCreate(List<Vote_ItemCatalog> instance)
        {
            List<string> _ListError = new List<string>();
            _ListError = this._repository.CreateBatch(instance);
            return _ListError;
        }

    }
}