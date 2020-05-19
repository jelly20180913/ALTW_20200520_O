using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Service.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement
{
    public class ButtonLogService : IButtonLogService
    {
        private IRepository<ButtonLog> _repository;
        public ButtonLogService(IRepository<ButtonLog> repository)
        {
            this._repository = repository;
        }
        public string Create(ButtonLog instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(ButtonLog instance)
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

        public ButtonLog GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<ButtonLog> GetAll()
        {
            return this._repository.GetAll();
        } 
        
    }
}