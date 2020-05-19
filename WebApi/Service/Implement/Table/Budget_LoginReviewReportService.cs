using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface.Table;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
using Newtonsoft.Json;

namespace WebApi.Service.Implement.Table
{
    public class JsonAccount
    {
        public string Account { get; set; }
        public string Report { get; set; }
    }

    public class Budget_LoginReviewReportService : IBudget_LoginReviewReportService
    {
        private IRepository<Budget_LoginReviewReport> _repository;
        private IRepository<Budget_DepartmentReport> _repo_dr;

        public Budget_LoginReviewReportService(IRepository<Budget_LoginReviewReport> repository, IRepository<Budget_DepartmentReport> repo_dr)
        {
            this._repository = repository;
            _repo_dr = repo_dr;
        }
        public string Create(Budget_LoginReviewReport instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(Budget_LoginReviewReport instance)
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

        public Budget_LoginReviewReport GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }
        public IEnumerable<Budget_LoginReviewReport> GetAll()
        {
            return this._repository.GetAll();
        }
        public IEnumerable<Budget_LoginReviewReport> GetByAccount(String account)
        {
            return this._repository.GetAll().Where(x => x.Account == account);

        }

        public Object GetReviewDepartment(string JsonStr)
        {
            JsonAccount jaccount = JsonConvert.DeserializeObject<JsonAccount>(JsonStr);
            var rp = this._repository.GetAll();
            var dr = _repo_dr.GetAll();
            switch (jaccount.Report)
            {
                case "1":
                     dr = _repo_dr.GetAll().Where(x => x.DeptExpense == "1");
                    break;
                case "2":
                     dr = _repo_dr.GetAll().Where(x => x.Scrap == "1");
                    break;
                case "3":
                     dr = _repo_dr.GetAll().Where(x => x.Travelling == "1");
                    break;
                case "4":
                     dr = _repo_dr.GetAll().Where(x => x.KPI == "1");
                    break;
                case "5":
                    dr = _repo_dr.GetAll().Where(x => x.Headcount == "1");
                    break;
                case "6":
                    dr = _repo_dr.GetAll().Where(x => x.Capex == "1");
                    break;
            }
            var LoginReports = (from rpm in rp
                                join urm in dr on new { rpm.Factory,rpm.DepartmentId } equals new { urm.Factory,urm.DepartmentId}
                                where rpm.Account == jaccount.Account 
                                select new
                                {
                                    Factory = rpm.Factory,
                                    DepartmentId = rpm.DepartmentId,
                                    DepartmentName = urm.DepartmentName
                                });
           return LoginReports;

        }
    }
}