using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface.Table;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement.Table
{
    public class CountryService:ICountryService
    {
        private IRepository<Country> _repository;
        public CountryService(IRepository<Country> repository)
        {
            this._repository = repository;
        }

        public void Create(Country instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            this._repository.Create(instance);
        }

        public void Update(Country instance)
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

        public Country GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<Country> GetAll()
        {
            return this._repository.GetAll();
        }

        public Country GetByCode(string code)
        {
            return this._repository.Get(x => x.Code == code);
        }
    }
}