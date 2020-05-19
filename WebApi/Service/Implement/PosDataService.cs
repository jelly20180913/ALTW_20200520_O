using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement
{
    public class PosDataService : IPosDataService
    { 
        private IRepository<PosData> _repository; 
        public PosDataService(IRepository<PosData> repository)
        {
            this._repository = repository;
        }
        public void Create(PosData instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            this._repository.Create(instance);
        }

        public void Update(PosData instance)
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

        public PosData GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<PosData> GetAll()
        {
            return this._repository.GetAll();
        }
        /// <summary>
        /// insert excel/txt data
        /// transfer   colomn named MonthYear for string type 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="fK_LoginId"></param>
        public List<string> MiltiCreate(IQueryable<PosData> instance, int fK_LoginId)
        {
           // int _N = 1;
            List<string> _ListError = new List<string>();
            List<PosData> _ListPosData = new List<PosData>();
            //foreach (PosData p in instance)
            //{
            //    _N++;
            //    p.FK_LoginId = fK_LoginId;
            //    p.MonthYear = p.MonthYear != "" ? Convert.ToDateTime(p.MonthYear).ToString("yyyy/MM/dd") : p.MonthYear;
            //    _Err = this.repository.Create(p);
            //    if (_Err != "") _ListError.Add("第" + _N.ToString() + "筆的資料格式有誤:" + _Err+"\r\n");
            //} 
            foreach (PosData p in instance)
            {
                p.FK_LoginId = fK_LoginId;
                p.MonthYear = p.MonthYear != "" ? Convert.ToDateTime(p.MonthYear).ToString("yyyy/MM/dd") : p.MonthYear;
                p.UpdateTime = DateTime.Now;
                _ListPosData.Add(p);
            }
            _ListError = this._repository.CreateBatch(_ListPosData);
            //_ListError = this.repository.test(_ListPosData);
            return _ListError;
        }
    }
}