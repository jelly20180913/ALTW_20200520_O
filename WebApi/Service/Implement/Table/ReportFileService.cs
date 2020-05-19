using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface.Table;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement.Table
{
    public class ReportFileService : IReportFileService
    {
        private IRepository<ReportFile> _repository;
        public ReportFileService(IRepository<ReportFile> repository)
        {
            this._repository = repository;
        }
        public string Create(ReportFile instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(ReportFile instance)
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

        public ReportFile GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<ReportFile> GetAll()
        {
            return this._repository.GetAll();
        }
        public IEnumerable<ReportFile> GetAllByDate(string date)
        {
            return this._repository.GetAll().Where(x => x.Date == date).OrderBy(x=>x.FileName);
        }
        public ReportFile GetByFileNameAndDate(string fileName,string date,string flag)
        {
            return this._repository.Get(x => x.FileName == fileName && x.Date == date && x.Flag == flag);
        }
    }
}