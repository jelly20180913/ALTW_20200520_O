using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface.Table;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement.Table
{
    public class PosOrderMappingService:IPosOrderMappingService
    {
        private IRepository<PosOrderMapping> _repository;
        public PosOrderMappingService(IRepository<PosOrderMapping> repository)
        {
            this._repository = repository;
        }

        public void Create(PosOrderMapping instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            this._repository.Create(instance);
        }

        public void Update(PosOrderMapping instance)
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

        public PosOrderMapping GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<PosOrderMapping> GetAll()
        {
            return this._repository.GetAll();
        }
        public PosOrderMapping GetByModel(int model)
        {
            return this._repository.Get(x => x.Model == model);
        }
    }
}