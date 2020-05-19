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
    public class Edi_SalesHeader_855Service : IEdi_SalesHeader_855Service
    {
        private IRepository<Edi_SalesHeader_855> _repository;

        public Edi_SalesHeader_855Service(IRepository<Edi_SalesHeader_855> repository)
        {
            this._repository = repository;
        }
        public string Create(Edi_SalesHeader_855 instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(Edi_SalesHeader_855 instance)
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

        public Edi_SalesHeader_855 GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<Edi_SalesHeader_855> GetAll()
        {
            return this._repository.GetAll();
        } 

     }
}