using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
using WebApi.DataModel.CustomModel.Budget;
using WebApi.Service.Interface.Table;
using Newtonsoft.Json;
using System.Text;
using WebApi.Service.Interface.Common;
namespace WebApi.Common.BudgetAdapter
{
    public class BudgetExcelCapex : BudgetBase
    {
        IItemCatalogService _itemCatalogService;
        ICommonFileService _commonFileService;
        public BudgetExcelCapex(IItemCatalogService itemCatalogService, ICommonFileService commonFileService)
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
            List<Budget_Capex> _Budget_CapexList = new List<Budget_Capex>();
            IQueryable<Capex> _CapexList;
            LinqToExcel.ExcelQueryFactory _Excel = new LinqToExcel.ExcelQueryFactory(filePath);
            _Excel.AddMapping<Capex>(d => d.AssetExp, "AssetExp");
            _Excel.AddMapping<Capex>(d => d.ProjectName, "ProjectName");
            _Excel.AddMapping<Capex>(d => d.ProjectRemark, "ProjectRemark");
            _Excel.AddMapping<Capex>(d => d.AssetExpType, "AssetExpType");
            _Excel.AddMapping<Capex>(d => d.JulLast, "JulLast");
            _Excel.AddMapping<Capex>(d => d.AugLast, "AugLast");
            _Excel.AddMapping<Capex>(d => d.Purpose, "Purpose");
            _Excel.AddMapping<Capex>(d => d.SepLast, "SepLast");
            _Excel.AddMapping<Capex>(d => d.OctLast, "OctLast");
            _Excel.AddMapping<Capex>(d => d.NovLast, "NovLast");
            _Excel.AddMapping<Capex>(d => d.DecLast, "DecLast");
            _Excel.AddMapping<Capex>(d => d.Jan, "Jan");
            _Excel.AddMapping<Capex>(d => d.Feb, "Feb");
            _Excel.AddMapping<Capex>(d => d.Mar, "Mar");
            _Excel.AddMapping<Capex>(d => d.Apr, "Apr");
            _Excel.AddMapping<Capex>(d => d.May, "May");
            _Excel.AddMapping<Capex>(d => d.Jun, "Jun");
            _Excel.AddMapping<Capex>(d => d.Jul, "Jul");
            _Excel.AddMapping<Capex>(d => d.Aug, "Aug");
            _Excel.AddMapping<Capex>(d => d.Sep, "Sep");
            _Excel.AddMapping<Capex>(d => d.Oct, "Oct");
            _Excel.AddMapping<Capex>(d => d.Nov, "Nov");
            _Excel.AddMapping<Capex>(d => d.Dec, "Dec");
            _Excel.AddMapping<Capex>(d => d.InternalOrder, "InternalOrder");
            _Excel.AddMapping<Capex>(d => d.ProjectAmount, "ProjectAmount");
            // every time get first sheet data
            _CapexList = from x in _Excel.Worksheet<Capex>(0)
                         select x;
            int _Start = 2;
            foreach (Capex c in _CapexList)
            {
                if (_Start == 2 || _Start == 3 || c.ProjectName == null)
                {
                    _Start++;
                    continue;
                }
                try
                {
                    Budget_Capex _Budget_Capex = new Budget_Capex();
                    _Budget_Capex.ItemId_AssetExp = _ItemCatalog.Where(x => x.ClassName == "AssetExp" && x.Name.Trim() == c.AssetExp.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == "AssetExp" && x.Name.Trim() == c.AssetExp.Trim()).First().ItemId : "";
                    if (Encoding.Default.GetByteCount(c.ProjectName) > 40) throw new Exception("project name exceed 40 words");
                    _Budget_Capex.ProjectName = c.ProjectName;
                    _Budget_Capex.ProjectRemark = c.ProjectRemark;
                    _Budget_Capex.ItemId_AssetExpType = _ItemCatalog.Where(x => x.ClassName == "AssetExpType" && x.Name.Trim() == c.AssetExpType.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == "AssetExpType" && x.Name.Trim() == c.AssetExpType.Trim()).First().ItemId : "";
                    _Budget_Capex.ItemId_Purpose = _ItemCatalog.Where(x => x.ClassName == "AssetExpPurpose" && x.Name.Trim() == c.Purpose.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == "AssetExpPurpose" && x.Name.Trim() == c.Purpose.Trim()).First().ItemId : "";
                    _Budget_Capex.JulLast = this._commonFileService.GetExcelMinusNumber(c.JulLast);
                    _Budget_Capex.AugLast = this._commonFileService.GetExcelMinusNumber(c.AugLast);
                    _Budget_Capex.SepLast = this._commonFileService.GetExcelMinusNumber(c.SepLast);
                    _Budget_Capex.OctLast = this._commonFileService.GetExcelMinusNumber(c.OctLast);
                    _Budget_Capex.NovLast = this._commonFileService.GetExcelMinusNumber(c.NovLast);
                    _Budget_Capex.DecLast = this._commonFileService.GetExcelMinusNumber(c.DecLast);
                    _Budget_Capex.Jan = this._commonFileService.GetExcelMinusNumber(c.Jan);
                    _Budget_Capex.Feb = this._commonFileService.GetExcelMinusNumber(c.Feb);
                    _Budget_Capex.Mar = this._commonFileService.GetExcelMinusNumber(c.Mar);
                    _Budget_Capex.Apr = this._commonFileService.GetExcelMinusNumber(c.Apr);
                    _Budget_Capex.May = this._commonFileService.GetExcelMinusNumber(c.May);
                    _Budget_Capex.Jun = this._commonFileService.GetExcelMinusNumber(c.Jun);
                    _Budget_Capex.Jul = this._commonFileService.GetExcelMinusNumber(c.Jul);
                    _Budget_Capex.Aug = this._commonFileService.GetExcelMinusNumber(c.Aug);
                    _Budget_Capex.Sep = this._commonFileService.GetExcelMinusNumber(c.Sep);
                    _Budget_Capex.Oct = this._commonFileService.GetExcelMinusNumber(c.Oct);
                    _Budget_Capex.Nov = this._commonFileService.GetExcelMinusNumber(c.Nov);
                    _Budget_Capex.Dec = this._commonFileService.GetExcelMinusNumber(c.Dec);
                    _Budget_Capex.Date = (DateTime.Now.Year + 1).ToString();
                    _Budget_Capex.DepartmentId = base.DepartmentId;
                    _Budget_Capex.Version = base.Version;
                    _Budget_Capex.Factory = base.Factory;
                    //add two column
                    _Budget_Capex.InternalOrder = c.InternalOrder;
                    _Budget_Capex.ProjectAmount = this._commonFileService.GetExcelMinusNumber(c.ProjectAmount);
                    _Budget_CapexList.Add(_Budget_Capex);
                }
                catch (Exception ex)
                {
                    string _c = JsonConvert.SerializeObject(c);
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:" + _c);
                }
                _Start++;
            }
            _Budget.Capex = _Budget_CapexList;
            return _Budget;
        }
    }
}