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
    public class BudgetExcelScrap : BudgetBase
    {
        IItemCatalogService _itemCatalogService;
        ICommonFileService _commonFileService;
        public BudgetExcelScrap(IItemCatalogService itemCatalogService, ICommonFileService commonFileService)
        {
            this._itemCatalogService = itemCatalogService;
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
            List<Budget_Scrap> _Budget_ScrapList = new List<Budget_Scrap>();
            IQueryable<Scrap> _ScrapList;
            LinqToExcel.ExcelQueryFactory _Excel = new LinqToExcel.ExcelQueryFactory(filePath);
            _Excel.AddMapping<Scrap>(d => d.ScrapType, "ScrapType");
            _Excel.AddMapping<Scrap>(d => d.PartNumber, "PartNumber");
            _Excel.AddMapping<Scrap>(d => d.Reason, "Reason");
            _Excel.AddMapping<Scrap>(d => d.Month, "Month");
            _Excel.AddMapping<Scrap>(d => d.Quantity, "Quantity");
            _Excel.AddMapping<Scrap>(d => d.PurchasePrice, "PurchasePrice");
            _Excel.AddMapping<Scrap>(d => d.BookValue, "BookValue");

            // every time get first sheet data
            _ScrapList = from x in _Excel.Worksheet<Scrap>(0)
                         select x;
            int _Start = 2;
            foreach (Scrap c in _ScrapList)
            {
                if (_Start == 2 || _Start == 3 || c.PartNumber == null)
                {
                    _Start++;
                    continue;
                }
                try
                {
                    Budget_Scrap _Budget_Scrap = new Budget_Scrap();
                    _Budget_Scrap.ItemId_ScrapType = _ItemCatalog.Where(x => x.ClassName == "ScrapType" && x.Name.Trim() == c.ScrapType.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == "ScrapType" && x.Name.Trim() == c.ScrapType.Trim()).First().ItemId : "";
                    _Budget_Scrap.PartNumber = c.PartNumber;
                    _Budget_Scrap.Reason = c.Reason;
                    _Budget_Scrap.Month = c.Month;
                    _Budget_Scrap.Quantity = this._commonFileService.GetExcelMinusNumber(c.Quantity  );
                    _Budget_Scrap.PurchasePrice = this._commonFileService.GetExcelMinusNumber(c.PurchasePrice );
                    _Budget_Scrap.BookValue = this._commonFileService.GetExcelMinusNumber(c.BookValue );
                    _Budget_Scrap.Date = (DateTime.Now.Year + 1).ToString();
                    _Budget_Scrap.DepartmentId = base.DepartmentId;
                    _Budget_Scrap.Version = base.Version;
                    _Budget_Scrap.Factory = base.Factory;
                    _Budget_ScrapList.Add(_Budget_Scrap);
                }
                catch (Exception ex)
                {
                    string _c = JsonConvert.SerializeObject(c);
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:" + _c);
                }
                _Start++;
            }
            _Budget.Scrap = _Budget_ScrapList;
            return _Budget;
        }
    }
}