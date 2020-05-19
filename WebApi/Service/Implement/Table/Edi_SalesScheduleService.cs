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
    public class Edi_SalesScheduleService : IEdi_SalesScheduleService
    {
        private IRepository<Edi_SalesSchedule> _repository;

        public Edi_SalesScheduleService(IRepository<Edi_SalesSchedule> repository)
        {
            this._repository = repository;
        }
        public string Create(Edi_SalesSchedule instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(Edi_SalesSchedule instance)
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

        public Edi_SalesSchedule GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<Edi_SalesSchedule> GetAll()
        {
            return this._repository.GetAll();
        } 

     }
}