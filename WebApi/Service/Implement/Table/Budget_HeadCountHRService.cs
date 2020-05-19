using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface.Table;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement.Table
{
    public class Budget_HeadCountHRService : IBudget_HeadCountHRService
    {
        private IRepository<Budget_HeadCountHR> _repository;
        public Budget_HeadCountHRService(IRepository<Budget_HeadCountHR> repository)
        {
            this._repository = repository;
        }

        public void Create(Budget_HeadCountHR instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            this._repository.Create(instance);
        }

        public void Update(Budget_HeadCountHR instance)
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

        public Budget_HeadCountHR GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<Budget_HeadCountHR> GetAll()
        {
            return this._repository.GetAll();
        }
        public List<string> MiltiCreate(List<Budget_HeadCountHR> instance )
        { 
            List<string> _ListError = new List<string>(); 
            _ListError = this._repository.CreateBatch(instance); 
            return _ListError;
        }
        public Budget_HeadCountHR GetByName(string AltwName)
        {
            return this._repository.Get(x => x.AltwName == AltwName);
        }
    }
}