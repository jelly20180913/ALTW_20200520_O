using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface.Table;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement.Table
{
    public class Budget_CostTravelingMappingService : IBudget_CostTravelingMappingService
    {
        private IRepository<Budget_CostTravelingMapping> _repository;
        public Budget_CostTravelingMappingService(IRepository<Budget_CostTravelingMapping> repository)
        {
            this._repository = repository;
        }
        public string Create(Budget_CostTravelingMapping instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(Budget_CostTravelingMapping instance)
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

        public Budget_CostTravelingMapping GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<Budget_CostTravelingMapping> GetAll()
        {
            return this._repository.GetAll();
        }
        public List<string> MiltiCreate(List<Budget_CostTravelingMapping> instance)
        {
            List<string> _ListError = new List<string>();
            _ListError = this._repository.CreateBatch(instance);
            return _ListError;
        }
    }
}