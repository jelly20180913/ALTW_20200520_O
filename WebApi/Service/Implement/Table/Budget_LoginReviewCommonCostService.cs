using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface.Table;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement.Table
{

    public class Budget_LoginReviewCommonCostService : IBudget_LoginReviewCommonCostService
    {
        private IRepository<Budget_LoginReviewCommonCost> _repository;

        public Budget_LoginReviewCommonCostService(IRepository<Budget_LoginReviewCommonCost> repository)
        {
            this._repository = repository;
        }
        public string Create(Budget_LoginReviewCommonCost instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(Budget_LoginReviewCommonCost instance)
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

        public Budget_LoginReviewCommonCost GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }
        public IEnumerable<Budget_LoginReviewCommonCost> GetAll()
        {
            return this._repository.GetAll();
        }
        public IEnumerable<Budget_LoginReviewCommonCost> GetByAccount(String account)
        {
            return this._repository.GetAll().Where(x => x.Account == account);

        }

    }
}