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
    public class Budget_ScrapService : IBudget_ScrapService
    {
        private IRepository<Budget_Scrap> _repository;

        public Budget_ScrapService(IRepository<Budget_Scrap> repository)
        {
            this._repository = repository;
        }
        public string Create(Budget_Scrap instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(Budget_Scrap instance)
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

        public Budget_Scrap GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<Budget_Scrap> GetAll()
        {
            return this._repository.GetAll();
        }
        public List<string> MiltiCreate(List<Budget_Scrap> instance)
        {
            List<string> _ListError = new List<string>();
            _ListError = this._repository.CreateBatch(instance);
            return _ListError;
        }

    }
}