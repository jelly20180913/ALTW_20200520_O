using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface.Table;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement.Table
{
    public class PosService : IPosService
    {
        private IRepository<Pos> _repository;
        public PosService(IRepository<Pos> repository)
        {
            this._repository = repository;
        }

        public void Create(Pos instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            this._repository.Create(instance);
        }

        public void Update(Pos instance)
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

        public Pos GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<Pos> GetAll()
        {
            return this._repository.GetAll();
        }
        public List<string> MiltiCreate(IQueryable<Pos> instance, int fK_LoginId)
        {
            // int _N = 1;
            List<string> _ListError = new List<string>();
            List<Pos> _ListPos = new List<Pos>();
            //foreach (PosData p in instance)
            //{
            //    _N++;
            //    p.FK_LoginId = fK_LoginId;
            //    p.MonthYear = p.MonthYear != "" ? Convert.ToDateTime(p.MonthYear).ToString("yyyy/MM/dd") : p.MonthYear;
            //    _Err = this.repository.Create(p);
            //    if (_Err != "") _ListError.Add("第" + _N.ToString() + "筆的資料格式有誤:" + _Err+"\r\n");
            //} 
            foreach (Pos p in instance)
            {
                p.FK_LoginId = fK_LoginId;
                p.UpdateTime = DateTime.Now;
                _ListPos.Add(p);
            }
            _ListError = this._repository.CreateBatch(_ListPos);
            //_ListError = this.repository.test(_ListPosData);
            return _ListError;
        }
    }
}