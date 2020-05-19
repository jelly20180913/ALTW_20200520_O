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
    public class BudgetExcelTravelling : BudgetBase
    {
        IBudget_HeadCountHRService _budget_HeadCountHRService;
        IItemCatalogService _itemCatalogService;
        IBudget_CostTravelingMappingService _budget_CostTravelingMappingService;
        ICommonFileService _commonFileService;
        public BudgetExcelTravelling(IBudget_HeadCountHRService budget_HeadCountHRService, IItemCatalogService itemCatalogService, IBudget_CostTravelingMappingService budget_CostTravelingMappingService, ICommonFileService commonFileService)
        {
            this._budget_HeadCountHRService = budget_HeadCountHRService;
            this._itemCatalogService = itemCatalogService;
            this._budget_CostTravelingMappingService = budget_CostTravelingMappingService;
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
            List<ItemCatalog> _ItemCatalog = this._itemCatalogService.GetAll().ToList();
            List<Budget_Traveling> _Budget_TravelingList = new List<Budget_Traveling>();
            IQueryable<Traveling> _TravelingList;
            LinqToExcel.ExcelQueryFactory _Excel = new LinqToExcel.ExcelQueryFactory(filePath);
            _Excel.AddMapping<Traveling>(d => d.AltwName, "AltwName");
            _Excel.AddMapping<Traveling>(d => d.Country, "Country");
            _Excel.AddMapping<Traveling>(d => d.Days, "Days");
            _Excel.AddMapping<Traveling>(d => d.Purpose, "Purpose");
            _Excel.AddMapping<Traveling>(d => d.Type, "Type");
            _Excel.AddMapping<Traveling>(d => d.Remark, "Remark");
            _Excel.AddMapping<Traveling>(d => d.Jan, "Jan");
            _Excel.AddMapping<Traveling>(d => d.Feb, "Feb");
            _Excel.AddMapping<Traveling>(d => d.Mar, "Mar");
            _Excel.AddMapping<Traveling>(d => d.Apr, "Apr");
            _Excel.AddMapping<Traveling>(d => d.May, "May");
            _Excel.AddMapping<Traveling>(d => d.Jun, "Jun");
            _Excel.AddMapping<Traveling>(d => d.Jul, "Jul");
            _Excel.AddMapping<Traveling>(d => d.Aug, "Aug");
            _Excel.AddMapping<Traveling>(d => d.Sep, "Sep");
            _Excel.AddMapping<Traveling>(d => d.Oct, "Oct");
            _Excel.AddMapping<Traveling>(d => d.Nov, "Nov");
            _Excel.AddMapping<Traveling>(d => d.Dec, "Dec");

            // every time get first sheet data
            _TravelingList = from x in _Excel.Worksheet<Traveling>(0)
                             select x;
            int _Start = 2;
            foreach (Traveling c in _TravelingList)
            {
                if (_Start == 2 || _Start == 3 || c.AltwName == null)
                {
                    _Start++;
                    continue;
                }
                try
                {
                    Budget_Traveling _Budget_Traveling = new Budget_Traveling();
                    _Budget_Traveling.ItemId_TravelingPurpose = _ItemCatalog.Where(x => x.ClassName == "TravelingPurpose" && x.Name.Trim() == c.Purpose.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == "TravelingPurpose" && x.Name.Trim() == c.Purpose.Trim()).First().ItemId : "";
                    //  _Budget_Traveling.Account = this._budget_HeadCountHRService.GetAll().Where(x => x.AltwName == c.AltwName).First().Account;
                    _Budget_Traveling.AltwName = c.AltwName;
                    _Budget_Traveling.Country = c.Country;
                    _Budget_Traveling.Days = c.Days == null ? 0 : int.Parse(c.Days, System.Globalization.NumberStyles.AllowThousands);
                    _Budget_Traveling.ItemId_TravelingType = _ItemCatalog.Where(x => x.ClassName == "TravelingType" && x.Name.Trim() == c.Type.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == "TravelingType" && x.Name.Trim() == c.Type.Trim()).First().ItemId : "";
                    _Budget_Traveling.Remark = c.Remark;
                    _Budget_Traveling.Jan = this._commonFileService.GetExcelMinusNumber(c.Jan);
                    _Budget_Traveling.Feb = this._commonFileService.GetExcelMinusNumber(c.Feb);
                    _Budget_Traveling.Mar = this._commonFileService.GetExcelMinusNumber(c.Mar);
                    _Budget_Traveling.Apr = this._commonFileService.GetExcelMinusNumber(c.Apr);
                    _Budget_Traveling.May = this._commonFileService.GetExcelMinusNumber(c.May);
                    _Budget_Traveling.Jun = this._commonFileService.GetExcelMinusNumber(c.Jun);
                    _Budget_Traveling.Jul = this._commonFileService.GetExcelMinusNumber(c.Jul);
                    _Budget_Traveling.Aug = this._commonFileService.GetExcelMinusNumber(c.Aug);
                    _Budget_Traveling.Sep = this._commonFileService.GetExcelMinusNumber(c.Sep);
                    _Budget_Traveling.Oct = this._commonFileService.GetExcelMinusNumber(c.Oct);
                    _Budget_Traveling.Nov = this._commonFileService.GetExcelMinusNumber(c.Nov);
                    _Budget_Traveling.Dec = this._commonFileService.GetExcelMinusNumber(c.Dec);
                    _Budget_Traveling.Date = (DateTime.Now.Year + 1).ToString();
                    _Budget_Traveling.DepartmentId = base.DepartmentId;
                    _Budget_Traveling.Version = base.Version;
                    _Budget_Traveling.Factory = base.Factory;
                    _Budget_TravelingList.Add(_Budget_Traveling);
                }
                catch (Exception ex)
                {
                    string _c = JsonConvert.SerializeObject(c);
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:" + _c);
                }
                _Start++;
            }
            if (_Budget_TravelingList.Count > 0)
                _Budget.DeptExpense = setDeptExpense(_Budget_TravelingList);
            _Budget.Traveling = _Budget_TravelingList;
            return _Budget;
        }
        /// <summary>
        /// group by traveling budget by costcode culumn and sum amount by every month culumn
        /// set into deptexpense list and return 
        /// </summary>
        /// <param name="budget_TravelingList"></param>
        private List<Budget_DeptExpense> setDeptExpense(List<Budget_Traveling> budget_TravelingList)
        {
            List<Budget_DeptExpense> _Budget_DeptExpenseList = new List<Budget_DeptExpense>();
            var results = budget_TravelingList.Join(this._budget_CostTravelingMappingService.GetAll(),
                TravelingList => TravelingList.ItemId_TravelingType,
                CostTravelingMapping => CostTravelingMapping.ItemId_TravelingType,
                (TravelingList, CostTravelingMapping) => new
                {
                    TravelingList,
                    CostTravelingMapping
                }).GroupBy(x => new { x.CostTravelingMapping.CostCode }).Select(x => new
                {
                    CostCode = x.Key.CostCode,
                    Jan = x.Sum(y => y.TravelingList.Jan),
                    Feb = x.Sum(y => y.TravelingList.Feb),
                    Mar = x.Sum(y => y.TravelingList.Mar),
                    Apr = x.Sum(y => y.TravelingList.Apr),
                    May = x.Sum(y => y.TravelingList.May),
                    Jun = x.Sum(y => y.TravelingList.Jun),
                    Jul = x.Sum(y => y.TravelingList.Jul),
                    Aug = x.Sum(y => y.TravelingList.Aug),
                    Sep = x.Sum(y => y.TravelingList.Sep),
                    Oct = x.Sum(y => y.TravelingList.Oct),
                    Nov = x.Sum(y => y.TravelingList.Nov),
                    Dec = x.Sum(y => y.TravelingList.Dec)
                }).ToList();
            foreach (var r in results)
            {
                Budget_DeptExpense _Budget_DeptExpense = new Budget_DeptExpense();
                _Budget_DeptExpense.CostCode = r.CostCode;
                _Budget_DeptExpense.Jan = r.Jan;
                _Budget_DeptExpense.Feb = r.Feb;
                _Budget_DeptExpense.Mar = r.Mar;
                _Budget_DeptExpense.Apr = r.Apr;
                _Budget_DeptExpense.May = r.May;
                _Budget_DeptExpense.Jun = r.Jun;
                _Budget_DeptExpense.Jul = r.Jul;
                _Budget_DeptExpense.Aug = r.Aug;
                _Budget_DeptExpense.Sep = r.Sep;
                _Budget_DeptExpense.Oct = r.Oct;
                _Budget_DeptExpense.Nov = r.Nov;
                _Budget_DeptExpense.Dec = r.Dec;
                _Budget_DeptExpense.Date = (DateTime.Now.Year + 1).ToString();
                _Budget_DeptExpense.DepartmentId = base.DepartmentId;
                _Budget_DeptExpense.Factory = base.Factory;
                _Budget_DeptExpense.Version = base.Version;
                _Budget_DeptExpense.ItemId_CostCommon = "00120000";
                _Budget_DeptExpenseList.Add(_Budget_DeptExpense);
            }
            return _Budget_DeptExpenseList;
        }
    }
}