using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface.Table;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement.Table
{

    public class Budget_LoginUploadReportService : IBudget_LoginUploadReportService
    {
        private IRepository<Budget_LoginUploadReport> _repository;

        public Budget_LoginUploadReportService(IRepository<Budget_LoginUploadReport> repository)
        {
            this._repository = repository;
        }
        public string Create(Budget_LoginUploadReport instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(Budget_LoginUploadReport instance)
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

        public Budget_LoginUploadReport GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }
        public IEnumerable<Budget_LoginUploadReport> GetAll()
        {
            return this._repository.GetAll();
        }
        public IEnumerable<Budget_LoginUploadReport> GetByAccount(String account)
        {
            return this._repository.GetAll().Where(x => x.Account == account);

        }

    }
}