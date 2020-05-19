using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
using WebApi.DataModel.CustomModel.Budget;
using WebApi.Service.Interface.Table;
using Newtonsoft.Json;
namespace WebApi.Common.BudgetAdapter
{
    public class BudgetItemCatalog : BudgetBase
    { 
        public BudgetItemCatalog( )
        { 
        } 
        public override Budget Parse(string filePath)
        {
            Budget _Budget = new Budget(); 
            List<ItemCatalog> _ItemCatalogList = new List<ItemCatalog>();
            IQueryable<ItemCatalog> _CustomItemCatalogList;
            LinqToExcel.ExcelQueryFactory _Excel = new LinqToExcel.ExcelQueryFactory(filePath);
            _Excel.AddMapping<ItemCatalog>(d => d.ClassName, "ClassName");
            _Excel.AddMapping<ItemCatalog>(d => d.ItemId, "ItemId");
            _Excel.AddMapping<ItemCatalog>(d => d.Name, "Name");
            // every time get first sheet data
            _CustomItemCatalogList = from x in _Excel.Worksheet<ItemCatalog>(0)
                                   select x;
            int _Start = 2;
            foreach (ItemCatalog ic in _CustomItemCatalogList)
            {
                try
                {
                    ItemCatalog _ItemCatalog = new ItemCatalog();
                    _ItemCatalog.ClassName = ic.ClassName;
                    _ItemCatalog.ItemId = ic.ItemId;
                    _ItemCatalog.Name = ic.Name;
                    _ItemCatalog.IsDel = false;
                    _ItemCatalogList.Add(_ItemCatalog);
                }
                catch (Exception ex)
                {
                    string _Ic = JsonConvert.SerializeObject(ic);
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:" + _Ic);
                }
                _Start++;
            }
            _Budget.ItemCatalog = _ItemCatalogList;
            return _Budget;
        }
    }
}