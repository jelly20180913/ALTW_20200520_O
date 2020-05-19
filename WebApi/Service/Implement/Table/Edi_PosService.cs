using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface.Table;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement.Table
{
    public class Edi_PosService : IEdi_PosService
    {
        private IRepository<Edi_Pos> _repository;
        public Edi_PosService(IRepository<Edi_Pos> repository)
        {
            this._repository = repository;
        }

        public void Create(Edi_Pos instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            this._repository.Create(instance);
        }

        public void Update(Edi_Pos instance)
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

        public Edi_Pos GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<Edi_Pos> GetAll()
        {
            return this._repository.GetAll();
        }
        public List<string> MiltiCreate(IQueryable<Edi_Pos> instance, int fK_LoginId)
        { 
            List<string> _ListError = new List<string>();
            List<Edi_Pos> _ListPos = new List<Edi_Pos>(); 
            foreach (Edi_Pos p in instance)
            {
                p.FK_LoginId = fK_LoginId;
                p.UpdateTime = DateTime.Now;
                _ListPos.Add(p);
            }
            _ListError = this._repository.CreateBatch(_ListPos); 
            return _ListError;
        }
    }
}