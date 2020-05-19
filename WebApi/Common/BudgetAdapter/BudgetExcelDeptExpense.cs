using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
using WebApi.DataModel.CustomModel.Budget;
using WebApi.Service.Interface.Table;
using Newtonsoft.Json;
using WebApi.Service.Interface.Common;
namespace WebApi.Common.BudgetAdapter
{
    public class BudgetExcelDeptExpense : BudgetBase
    {
        IBudget_CostService _budget_CostService;
        IBudget_DeptExpenseService _budget_DeptExpenseService;
        ICommonFileService _commonFileService;
        public BudgetExcelDeptExpense(IBudget_CostService budget_CostService, IBudget_DeptExpenseService budget_DeptExpenseService, ICommonFileService commonFileService)
        {
            this._budget_CostService = budget_CostService;
            this._budget_DeptExpenseService = budget_DeptExpenseService;
            this._commonFileService = commonFileService;
        }
        /// <summary>
        /// p.s mapping name maybe need to change
        /// </summary>
        /// <param name="filePath"></param> 
        /// <returns></returns>
        public override Budget Parse(string filePath)
        {
            Budget _Budget = new Budget();
            List<Budget_Cost> _Budget_Cost = this._budget_CostService.GetAll().ToList();
            List<Budget_DeptExpense> _Budget_DeptExpenseList = new List<Budget_DeptExpense>();
            IQueryable<DeptExpense> _DeptExpenseList;
            List<Budget_DeptExpense> _Budget_DeptExpenseList_Common = new List<Budget_DeptExpense>();
            LinqToExcel.ExcelQueryFactory _Excel = new LinqToExcel.ExcelQueryFactory(filePath);
            _Excel.AddMapping<DeptExpense>(d => d.ItemId_CostElement, "CostCode");
            _Excel.AddMapping<DeptExpense>(d => d.YTD, "YTD");
            _Excel.AddMapping<DeptExpense>(d => d.Jan, "Jan");
            _Excel.AddMapping<DeptExpense>(d => d.Feb, "Feb");
            _Excel.AddMapping<DeptExpense>(d => d.Mar, "Mar");
            _Excel.AddMapping<DeptExpense>(d => d.Apr, "Apr");
            _Excel.AddMapping<DeptExpense>(d => d.May, "May");
            _Excel.AddMapping<DeptExpense>(d => d.Jun, "Jun");
            _Excel.AddMapping<DeptExpense>(d => d.Jul, "Jul");
            _Excel.AddMapping<DeptExpense>(d => d.Aug, "Aug");
            _Excel.AddMapping<DeptExpense>(d => d.Sep, "Sep");
            _Excel.AddMapping<DeptExpense>(d => d.Oct, "Oct");
            _Excel.AddMapping<DeptExpense>(d => d.Nov, "Nov");
            _Excel.AddMapping<DeptExpense>(d => d.Dec, "Dec");
            if (base.CommonBudget)
            {
                _Excel.AddMapping<DeptExpense>(d => d.Factory, "Factory");
                _Excel.AddMapping<DeptExpense>(d => d.DepartmentId, "DepartmentId");
            }
            // every time get first sheet data
            _DeptExpenseList = from x in _Excel.Worksheet<DeptExpense>("Upload")
                               select x;
            int _Start = 2;
            foreach (DeptExpense c in _DeptExpenseList)
            {
                if (_Start == 2 || _Start == 3 || c.ItemId_CostElement == null)
                {
                    _Start++;
                    continue;
                }
                try
                {
                    Budget_DeptExpense _Budget_DeptExpense = new Budget_DeptExpense();
                    _Budget_DeptExpense.CostCode = c.ItemId_CostElement;
                    _Budget_DeptExpense.YTD = this._commonFileService.GetExcelMinusNumber(c.YTD);
                    _Budget_DeptExpense.Jan = this._commonFileService.GetExcelMinusNumber(c.Jan);
                    _Budget_DeptExpense.Feb = this._commonFileService.GetExcelMinusNumber(c.Feb);
                    _Budget_DeptExpense.Mar = this._commonFileService.GetExcelMinusNumber(c.Mar);
                    _Budget_DeptExpense.Apr = this._commonFileService.GetExcelMinusNumber(c.Apr);
                    _Budget_DeptExpense.May = this._commonFileService.GetExcelMinusNumber(c.May);
                    _Budget_DeptExpense.Jun = this._commonFileService.GetExcelMinusNumber(c.Jun);
                    _Budget_DeptExpense.Jul = this._commonFileService.GetExcelMinusNumber(c.Jul);
                    _Budget_DeptExpense.Aug = this._commonFileService.GetExcelMinusNumber(c.Aug);
                    _Budget_DeptExpense.Sep = this._commonFileService.GetExcelMinusNumber(c.Sep);
                    _Budget_DeptExpense.Oct = this._commonFileService.GetExcelMinusNumber(c.Oct);
                    _Budget_DeptExpense.Nov = this._commonFileService.GetExcelMinusNumber(c.Nov);
                    _Budget_DeptExpense.Dec = this._commonFileService.GetExcelMinusNumber(c.Dec);
                    _Budget_DeptExpense.Date = (DateTime.Now.Year + 1).ToString();
                    _Budget_DeptExpense.DepartmentId = base.CommonBudget ? c.DepartmentId : base.DepartmentId;
                    _Budget_DeptExpense.Factory = base.CommonBudget ? c.Factory : base.Factory;
                    _Budget_DeptExpense.Version = base.Version;
                    //00120000:部門費用
                    _Budget_DeptExpense.ItemId_CostCommon = base.CommonBudget ? base.ItemId_CostCommon : "00120000";
                    _Budget_DeptExpenseList.Add(_Budget_DeptExpense);
                    //6136010000:訓練費
                    if (c.ItemId_CostElement == "6136010000" && base.CommonBudget)
                        _Budget_DeptExpenseList_Common.Add(_Budget_DeptExpense);

                }
                catch (Exception ex)
                {
                    string _c = JsonConvert.SerializeObject(c);
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:" + _c);
                }
                _Start++;
            }
            if (this.ListError.Count == 0)
            {
                foreach (Budget_DeptExpense budget_DeptExpense in _Budget_DeptExpenseList_Common)
                {
                    updateDeptExpense(budget_DeptExpense);
                }
            }
            _Budget.DeptExpense = _Budget_DeptExpenseList;
            return _Budget;
        }
        /// <summary>
        /// let common expense add to department expense
        /// 00120000:department expense
        /// </summary>
        /// <param name="budget_DeptExpense"></param>
        private void updateDeptExpense(Budget_DeptExpense budget_DeptExpense)
        {
            List<Budget_DeptExpense> _Budget_DeptExpenseList = new List<Budget_DeptExpense>();
            Budget_DeptExpense _Budget_DeptExpense = new Budget_DeptExpense();
            if (this._budget_DeptExpenseService.GetAll().Where(x => x.Factory == budget_DeptExpense.Factory && x.DepartmentId == budget_DeptExpense.DepartmentId && x.CostCode == budget_DeptExpense.CostCode && x.ItemId_CostCommon == "00120000").ToList().Count > 0)
            {
                _Budget_DeptExpenseList = this._budget_DeptExpenseService.GetAll().Where(x => x.Factory == budget_DeptExpense.Factory && x.DepartmentId == budget_DeptExpense.DepartmentId && x.CostCode == budget_DeptExpense.CostCode && x.ItemId_CostCommon == "00120000").ToList();
                foreach (Budget_DeptExpense c in _Budget_DeptExpenseList)
                {
                    c.YTD += budget_DeptExpense.YTD;
                    c.Jan += budget_DeptExpense.Jan;
                    c.Feb += budget_DeptExpense.Feb;
                    c.Mar += budget_DeptExpense.Mar;
                    c.Apr += budget_DeptExpense.Apr;
                    c.May += budget_DeptExpense.May;
                    c.Jun += budget_DeptExpense.Jun;
                    c.Jul += budget_DeptExpense.Jul;
                    c.Aug += budget_DeptExpense.Aug;
                    c.Sep += budget_DeptExpense.Sep;
                    c.Oct += budget_DeptExpense.Oct;
                    c.Nov += budget_DeptExpense.Nov;
                    c.Dec += budget_DeptExpense.Dec;
                    c.Date = (DateTime.Now.Year + 1).ToString();
                    this._budget_DeptExpenseService.Update(c);
                }
            }
        }
    }
}