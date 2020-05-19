using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface.Table;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement.Table
{

    public class Budget_LoginUploadCommonCostService : IBudget_LoginUploadCommonCostService
    {
        private IRepository<Budget_LoginUploadCommonCost> _repository;

        public Budget_LoginUploadCommonCostService(IRepository<Budget_LoginUploadCommonCost> repository)
        {
            this._repository = repository;
        }
        public string Create(Budget_LoginUploadCommonCost instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(Budget_LoginUploadCommonCost instance)
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

        public Budget_LoginUploadCommonCost GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }
        public IEnumerable<Budget_LoginUploadCommonCost> GetAll()
        {
            return this._repository.GetAll();
        }
        public IEnumerable<Budget_LoginUploadCommonCost> GetByAccount(String account)
        {
            return this._repository.GetAll().Where(x => x.Account == account);

        }

    }
}