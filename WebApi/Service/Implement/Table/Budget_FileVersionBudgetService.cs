using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface.Table;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement.Table
{
    public class Budget_FileVersionBudgetService : IBudget_FileVersionBudgetService
    {
        private IRepository<Budget_FileVersionBudget> _repository;
        public Budget_FileVersionBudgetService(IRepository<Budget_FileVersionBudget> repository)
        {
            this._repository = repository;
        }
        public string Create(Budget_FileVersionBudget instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(Budget_FileVersionBudget instance)
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

        public Budget_FileVersionBudget GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<Budget_FileVersionBudget> GetAll()
        {
            return this._repository.GetAll();
        }
        public Budget_FileVersionBudget GetLastVersion(string itemId, string departmentId, string factory,string date)
        {
            return this._repository.Get(x => x.ItemId_BudgetName == itemId && x.DepartmentId == departmentId && x.Factory == factory&&x.Date==date);
        }
        public Budget_FileVersionBudget GetLastVersion(string itemId , string date, string factory)
        {
            return this._repository.Get(x => x.ItemId_BudgetName == itemId  && x.Date == date && x.Factory == factory);
        }
    }
}