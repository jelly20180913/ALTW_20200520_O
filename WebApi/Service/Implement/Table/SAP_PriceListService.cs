using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
using WebApi.Service.Interface.Table;
namespace WebApi.Service.Implement
{
    public class SAP_PriceListService : ISAP_PriceListService
    {
        private IRepository<SAP_PriceList> _repository;
        public SAP_PriceListService(IRepository<SAP_PriceList> repository)
        {
            this._repository = repository;
        }
        public void Create(SAP_PriceList instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            this._repository.Create(instance);
        }

        public void Update(SAP_PriceList instance)
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

        public SAP_PriceList GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<SAP_PriceList> GetAll()
        {
            return this._repository.GetAll();
        }
        public List<string> MiltiCreate(List<SAP_PriceList> instance)
        {
            List<string> _ListError = new List<string>();
            _ListError = this._repository.CreateBatch(instance);
            return _ListError;
        } 
    }
}