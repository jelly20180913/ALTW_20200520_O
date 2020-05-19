using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
using WebApi.Service.Interface.Table;
namespace WebApi.Service.Implement
{
    public class ErrorLogService : IErrorLogService
    {
        private IRepository<ErrorLog> _repository;
        public ErrorLogService(IRepository<ErrorLog> repository)
        {
            this._repository = repository;
        }
        public void Create(ErrorLog instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            this._repository.Create(instance);
        }

        public void Update(ErrorLog instance)
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

        public ErrorLog GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<ErrorLog> GetAll()
        {
            return this._repository.GetAll();
        }
        public List<string> MiltiCreate(List<ErrorLog> instance)
        {
            List<string> _ListError = new List<string>();
            _ListError = this._repository.CreateBatch(instance);
            return _ListError;
        } 
    }
}