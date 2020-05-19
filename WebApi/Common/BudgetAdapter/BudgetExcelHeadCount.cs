using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
using WebApi.DataModel.CustomModel.Budget;
using WebApi.Service.Interface.Table;
using Newtonsoft.Json;
namespace WebApi.Common.BudgetAdapter
{
    public class BudgetExcelHeadCount : BudgetBase
    {
        IBudget_HeadCountHRService _budget_HeadCountHRService;
        IItemCatalogService _itemCatalogService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="budget_HeadCountHRService"></param>
        public BudgetExcelHeadCount(IBudget_HeadCountHRService budget_HeadCountHRService, IItemCatalogService itemCatalogService)
        {
            this._budget_HeadCountHRService = budget_HeadCountHRService;
            this._itemCatalogService = itemCatalogService;
        }
        /// <summary>
        /// if import part time hr excel need get factor departmentId column
        /// </summary>
        /// <param name="filePath"></param> 
        /// <returns></returns>
        public override Budget Parse(string filePath)
        {
            Budget _Budget = new Budget();
            List<ItemCatalog> _ItemCatalog = this._itemCatalogService.GetAll().ToList();
            List<Budget_HeadCount> _Budget_HeadCountList = new List<Budget_HeadCount>();
            IQueryable<Headcount> _HeadcountList;
            LinqToExcel.ExcelQueryFactory _Excel = new LinqToExcel.ExcelQueryFactory(filePath);
            _Excel.AddMapping<Headcount>(d => d.AltwName, "AltwName");
            _Excel.AddMapping<Headcount>(d => d.JobFunction, "JobFunction");
            _Excel.AddMapping<Headcount>(d => d.Title, "Title");
            _Excel.AddMapping<Headcount>(d => d.DirectType, "DirectType");
            _Excel.AddMapping<Headcount>(d => d.Current, "Current");
            _Excel.AddMapping<Headcount>(d => d.JulLast, "JulLast");
            _Excel.AddMapping<Headcount>(d => d.AugLast, "AugLast");
            _Excel.AddMapping<Headcount>(d => d.SepLast, "SepLast");
            _Excel.AddMapping<Headcount>(d => d.OctLast, "OctLast");
            _Excel.AddMapping<Headcount>(d => d.NovLast, "NovLast");
            _Excel.AddMapping<Headcount>(d => d.DecLast, "DecLast");
            _Excel.AddMapping<Headcount>(d => d.Jan, "Jan");
            _Excel.AddMapping<Headcount>(d => d.Feb, "Feb");
            _Excel.AddMapping<Headcount>(d => d.Mar, "Mar");
            _Excel.AddMapping<Headcount>(d => d.Apr, "Apr");
            _Excel.AddMapping<Headcount>(d => d.May, "May");
            _Excel.AddMapping<Headcount>(d => d.Jun, "Jun");
            _Excel.AddMapping<Headcount>(d => d.Jul, "Jul");
            _Excel.AddMapping<Headcount>(d => d.Aug, "Aug");
            _Excel.AddMapping<Headcount>(d => d.Sep, "Sep");
            _Excel.AddMapping<Headcount>(d => d.Oct, "Oct");
            _Excel.AddMapping<Headcount>(d => d.Nov, "Nov");
            _Excel.AddMapping<Headcount>(d => d.Dec, "Dec");
            _Excel.AddMapping<Headcount>(d => d.Reason, "Reason");
            _Excel.AddMapping<Headcount>(d => d.Remark, "Remark");
            if (base.PartTime)
            {
                _Excel.AddMapping<DeptExpense>(d => d.Factory, "Factory");
                _Excel.AddMapping<DeptExpense>(d => d.DepartmentId, "DepartmentId");
            }
            // every time get first sheet data
            _HeadcountList = from x in _Excel.Worksheet<Headcount>(0)
                             select x;
            int _Start = 2;
            foreach (Headcount h in _HeadcountList)
            {
                if (_Start == 2 || _Start == 3 || h.AltwName == null && !base.PartTime)
                {
                    _Start++;
                    continue;
                }
                try
                {
                    Budget_HeadCount _Budget_HeadCount = new Budget_HeadCount();
                    string _FactoryType = base.Factory == "LT" ? "L" : "K"; 
                    string _NewEmployee = _FactoryType + Guid.NewGuid().ToString("N").Substring(0,18);
                    _Budget_HeadCount.Account = this._budget_HeadCountHRService.GetAll().Where(x => x.AltwName == h.AltwName).ToList().Count > 0 ? this._budget_HeadCountHRService.GetAll().Where(x => x.AltwName == h.AltwName).First().Account : _NewEmployee;
                    _Budget_HeadCount.JulLast = h.JulLast == "" ? 0 : h.JulLast == "(1)" ? -1 : Convert.ToInt32(h.JulLast);
                    _Budget_HeadCount.AugLast = h.AugLast == "" ? 0 : h.AugLast == "(1)" ? -1 : Convert.ToInt32(h.AugLast);
                    _Budget_HeadCount.SepLast = h.SepLast == "" ? 0 : h.SepLast == "(1)" ? -1 : Convert.ToInt32(h.SepLast);
                    _Budget_HeadCount.OctLast = h.OctLast == "" ? 0 : h.OctLast == "(1)" ? -1 : Convert.ToInt32(h.OctLast);
                    _Budget_HeadCount.NovLast = h.NovLast == "" ? 0 : h.NovLast == "(1)" ? -1 : Convert.ToInt32(h.NovLast);
                    _Budget_HeadCount.DecLast = h.DecLast == "" ? 0 : h.DecLast == "(1)" ? -1 : Convert.ToInt32(h.DecLast);
                    _Budget_HeadCount.Jan = h.Jan == "" ? 0 : h.Jan == "(1)" ? -1 : Convert.ToInt32(h.Jan);
                    _Budget_HeadCount.Feb = h.Feb == "" ? 0 : h.Feb == "(1)" ? -1 : Convert.ToInt32(h.Feb);
                    _Budget_HeadCount.Mar = h.Mar == "" ? 0 : h.Mar == "(1)" ? -1 : Convert.ToInt32(h.Mar);
                    _Budget_HeadCount.Apr = h.Apr == "" ? 0 : h.Apr == "(1)" ? -1 : Convert.ToInt32(h.Apr);
                    _Budget_HeadCount.May = h.May == "" ? 0 : h.May == "(1)" ? -1 : Convert.ToInt32(h.May);
                    _Budget_HeadCount.Jun = h.Jun == "" ? 0 : h.Jun == "(1)" ? -1 : Convert.ToInt32(h.Jun);
                    _Budget_HeadCount.Jul = h.Jul == "" ? 0 : h.Jul == "(1)" ? -1 : Convert.ToInt32(h.Jul);
                    _Budget_HeadCount.Aug = h.Aug == "" ? 0 : h.Aug == "(1)" ? -1 : Convert.ToInt32(h.Aug);
                    _Budget_HeadCount.Sep = h.Sep == "" ? 0 : h.Sep == "(1)" ? -1 : Convert.ToInt32(h.Sep);
                    _Budget_HeadCount.Oct = h.Oct == "" ? 0 : h.Oct == "(1)" ? -1 : Convert.ToInt32(h.Oct);
                    _Budget_HeadCount.Nov = h.Nov == "" ? 0 : h.Nov == "(1)" ? -1 : Convert.ToInt32(h.Nov);
                    _Budget_HeadCount.Dec = h.Dec == "" ? 0 : h.Dec == "(1)" ? -1 : Convert.ToInt32(h.Dec);
                    _Budget_HeadCount.Date = (DateTime.Now.Year + 1).ToString();
                    _Budget_HeadCount.Version = base.Version;
                    _Budget_HeadCount.DepartmentId = base.PartTime ? h.DepartmentId : base.DepartmentId;
                    _Budget_HeadCount.Factory = base.PartTime ? h.Factory : base.Factory;
                    _Budget_HeadCount.PartTime = base.PartTime;
                    _Budget_HeadCount.CurrentHC = h.Current == "1" ? true : false;
                    string _ItemId_Reason = "";
                    if (!base.PartTime)
                    {
                        if (h.Reason != "Pls Choose 請選擇") _ItemId_Reason = _ItemCatalog.Where(x => x.ClassName == "Reason" && x.Name.Trim() == h.Reason.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == "Reason" && x.Name.Trim() == h.Reason.Trim()).First().ItemId : "";
                        _Budget_HeadCount.ItemId_Reason = _ItemId_Reason;
                    }
                    _Budget_HeadCount.Remark = h.Remark;
                    _Budget_HeadCountList.Add(_Budget_HeadCount);

                    if (h.Current == null && !base.PartTime)
                    {
                        insertHeadCountHR(h, _ItemCatalog,_NewEmployee);
                    }
                }
                catch (Exception ex)
                {
                    string _h = JsonConvert.SerializeObject(h);
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:" + _h);
                }
                _Start++;
            }
            _Budget.HeadCount = _Budget_HeadCountList;
            return _Budget;
        }
        /// <summary>
        /// insertHeadCountHR
        /// </summary>
        /// <param name="headcount"></param>
        /// <param name="itemCatalog"></param>
        /// <param name="newEmployee"></param>
        private void insertHeadCountHR(Headcount headcount, List<ItemCatalog> itemCatalog,string newEmployee)
        {
            string _JobFunction = base.Factory == "LT" ? "JobFunction" : "JobFunctionKSZ";
            string _Title = base.Factory == "LT" ? "Title" : "TitleKSZ";
            Budget_HeadCountHR _Budget_HeadCountHR = this._budget_HeadCountHRService.GetByName(headcount.AltwName);
            if(_Budget_HeadCountHR==null) _Budget_HeadCountHR = new Budget_HeadCountHR(); 
            string _FactoryType = base.Factory == "LT" ? "L" : "K";
            _Budget_HeadCountHR.Account = this._budget_HeadCountHRService.GetAll().Where(x => x.AltwName == headcount.AltwName).ToList().Count > 0 ? this._budget_HeadCountHRService.GetAll().Where(x => x.AltwName == headcount.AltwName).First().Account : newEmployee;
          
            _Budget_HeadCountHR.AltwName = headcount.AltwName; 
            _Budget_HeadCountHR.ItemId_DirectType = itemCatalog.Where(x => x.ClassName == "DirectType" && x.Name.Trim() == headcount.DirectType.Trim()).ToList().Count > 0 ? itemCatalog.Where(x => x.ClassName == "DirectType" && x.Name.Trim() == headcount.DirectType.Trim()).First().ItemId : "";
            _Budget_HeadCountHR.ItemId_JobFunction = itemCatalog.Where(x => x.ClassName == _JobFunction && x.Name.Trim() == headcount.JobFunction.Trim()).ToList().Count > 0 ? itemCatalog.Where(x => x.ClassName == _JobFunction && x.Name.Trim() == headcount.JobFunction.Trim()).First().ItemId : "";
            _Budget_HeadCountHR.ItemId_Title = itemCatalog.Where(x => x.ClassName == _Title && x.Name.Trim().ToLower() == headcount.Title.Trim().ToLower()).ToList().Count > 0 ? itemCatalog.Where(x => x.ClassName == _Title && x.Name.Trim().ToLower() == headcount.Title.Trim().ToLower()).First().ItemId : "";
            _Budget_HeadCountHR.IsDel = false;
            if (_Budget_HeadCountHR.Id == 0) this._budget_HeadCountHRService.Create(_Budget_HeadCountHR);
            else this._budget_HeadCountHRService.Update(_Budget_HeadCountHR);
        }
    }
}