using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement
{
    public class PosColumnMapService : IPosColumnMapService
    { 
        private IRepository<PosColumnMap> _repository;
        public PosColumnMapService(IRepository<PosColumnMap> repository )
        {
            this._repository = repository; 
        }

        public void Create(PosColumnMap instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            this._repository.Create(instance);
        }

        public void Update(PosColumnMap instance)
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

        public PosColumnMap GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<PosColumnMap> GetAll()
        {
            return this._repository.GetAll();
        }
    }
}