using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
using WebApi.DataModel.CustomModel.Budget;
using WebApi.Service.Interface.Table;
using Newtonsoft.Json;
namespace WebApi.Common.BudgetAdapter
{
    public class BudgetExcelHeadCountHR : BudgetBase
    {
        IItemCatalogService _itemCatalogService;
        public BudgetExcelHeadCountHR(IItemCatalogService itemCatalogService)
        {
            this._itemCatalogService = itemCatalogService;
        } 
        public override Budget Parse(string filePath)
        {
            Budget _Budget = new Budget();
            List<ItemCatalog> _ItemCatalog = this._itemCatalogService.GetAll().ToList();
            List<Budget_HeadCountHR> _Budget_HeadCountHRList = new List<Budget_HeadCountHR>();
            IQueryable<CustomHeadcount> _CustomHeadcountList;
            LinqToExcel.ExcelQueryFactory _Excel = new LinqToExcel.ExcelQueryFactory(filePath); 
            _Excel.AddMapping<CustomHeadcount>(d => d.Account, "Employee Code");
            _Excel.AddMapping<CustomHeadcount>(d => d.AltwName, "Employee Name"); 
            _Excel.AddMapping<CustomHeadcount>(d => d.HR, "銷/管/研"); 
            _Excel.AddMapping<CustomHeadcount>(d => d.DirectType, "DL/IDL");
            _Excel.AddMapping<CustomHeadcount>(d => d.Func, "Func");
            _Excel.AddMapping<CustomHeadcount>(d => d.Title, "Title");
            // every time get first sheet data
            _CustomHeadcountList = from x in _Excel.Worksheet<CustomHeadcount>(0)
                                   select x;
            int _Start = 2;
            string _JobFunction = base.Factory == "LT" ? "JobFunction" : "JobFunctionKSZ";
            string _Title= base.Factory == "LT" ? "Title" : "TitleKSZ";
            foreach (CustomHeadcount ch in _CustomHeadcountList)
            {
                try
                {
                    Budget_HeadCountHR _Budget_HeadCountHR = new Budget_HeadCountHR(); 
                    _Budget_HeadCountHR.Account = ch.Account;
                    _Budget_HeadCountHR.AltwName = ch.AltwName; 
                    string _ItemId_HR = "";
                    if (ch.HR!=null&&ch.HR!="") _ItemId_HR = _ItemCatalog.Where(x => x.ClassName == "HR" && x.Name.Trim() == ch.HR.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == "HR" && x.Name == ch.HR).First().ItemId : ""; 
                    _Budget_HeadCountHR.ItemId_HR = _ItemId_HR; 
                    _Budget_HeadCountHR.ItemId_DirectType = _ItemCatalog.Where(x => x.ClassName == "DirectType" && x.Name.Trim() == ch.DirectType.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == "DirectType" && x.Name.Trim() == ch.DirectType.Trim()).First().ItemId : "";  
                    _Budget_HeadCountHR.ItemId_JobFunction = _ItemCatalog.Where(x => x.ClassName == _JobFunction && x.Name.Trim() == ch.Func.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == _JobFunction && x.Name.Trim() == ch.Func.Trim()).First().ItemId : "";
                    _Budget_HeadCountHR.ItemId_Title = _ItemCatalog.Where(x => x.ClassName == _Title && x.Name.Trim() == ch.Title.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == _Title && x.Name.Trim() == ch.Title.Trim()).First().ItemId : "";
                    _Budget_HeadCountHR.IsDel = false;
                    _Budget_HeadCountHRList.Add(_Budget_HeadCountHR);
                }
                catch (Exception ex)
                {
                    string _Ch = JsonConvert.SerializeObject(ch);
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:" + _Ch);
                }
                _Start++;
            }
            _Budget.HeadCountHR = _Budget_HeadCountHRList;
            return _Budget;
        }
    }
}