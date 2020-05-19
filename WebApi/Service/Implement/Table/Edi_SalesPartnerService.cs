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
    public class Edi_SalesPartnerService : IEdi_SalesPartnerService
    {
        private IRepository<Edi_SalesPartner> _repository;

        public Edi_SalesPartnerService(IRepository<Edi_SalesPartner> repository)
        {
            this._repository = repository;
        }
        public string Create(Edi_SalesPartner instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(Edi_SalesPartner instance)
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

        public Edi_SalesPartner GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<Edi_SalesPartner> GetAll()
        {
            return this._repository.GetAll();
        } 

     }
}