using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
using WebApi.DataModel.CustomModel.Budget;
using WebApi.Service.Interface.Table;
using Newtonsoft.Json;
namespace WebApi.Common.BudgetAdapter
{
    public class BudgetExcelDeptKPI : BudgetBase
    {
        public BudgetExcelDeptKPI()
        {

        }
        /// <summary>
        /// p.s mapping name maybe need to change
        /// </summary>
        /// <param name="filePath"></param> 
        /// <returns></returns>
        public override Budget Parse(string filePath)
        {
            Budget _Budget = new Budget();
            List<Budget_DeptKPI> _Budget_DeptKPIList = new List<Budget_DeptKPI>();
            IQueryable<Budget_DeptKPI> _DeptKPIList;
            LinqToExcel.ExcelQueryFactory _Excel = new LinqToExcel.ExcelQueryFactory(filePath);
            _Excel.AddMapping<Budget_DeptKPI>(d => d.GoalName, " GoalName");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.PIC_Name, "PIC_Name");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.LastYear, "LastYear");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.YTD, "YTD");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.Jan, "Jan");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.Feb, "Feb");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.Mar, "Mar");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.Apr, "Apr");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.May, "May");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.Jun, "Jun");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.Jul, "Jul");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.Aug, "Aug");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.Sep, "Sep");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.Oct, "Oct");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.Nov, "Nov");
            _Excel.AddMapping<Budget_DeptKPI>(d => d.Dec, "Dec");
            // every time get first sheet data
            _DeptKPIList = from x in _Excel.Worksheet<Budget_DeptKPI>("Upload")
                           select x;
            int _Start = 2;
            foreach (Budget_DeptKPI c in _DeptKPIList)
            {
                if (_Start == 2 || c.GoalName == null)
                {
                    _Start++;
                    continue;
                }
                try
                {
                    Budget_DeptKPI _Budget_DeptKPI = new Budget_DeptKPI();
                    _Budget_DeptKPI.GoalName = c.GoalName;
                    _Budget_DeptKPI.PIC_Name = c.PIC_Name;
                    _Budget_DeptKPI.LastYear = c.LastYear;
                    _Budget_DeptKPI.YTD = c.YTD;
                    _Budget_DeptKPI.Jan = c.Jan;
                    _Budget_DeptKPI.Feb = c.Feb;
                    _Budget_DeptKPI.Mar = c.Mar;
                    _Budget_DeptKPI.Apr = c.Apr;
                    _Budget_DeptKPI.May = c.May;
                    _Budget_DeptKPI.Jun = c.Jun;
                    _Budget_DeptKPI.Jul = c.Jul;
                    _Budget_DeptKPI.Aug = c.Aug;
                    _Budget_DeptKPI.Sep = c.Sep;
                    _Budget_DeptKPI.Oct = c.Oct;
                    _Budget_DeptKPI.Nov = c.Nov;
                    _Budget_DeptKPI.Dec = c.Dec;
                    _Budget_DeptKPI.Date = (DateTime.Now.Year + 1).ToString();
                    _Budget_DeptKPI.DepartmentId = base.DepartmentId;
                    _Budget_DeptKPI.Version = base.Version;
                    _Budget_DeptKPI.Factory = base.Factory;
                    _Budget_DeptKPIList.Add(_Budget_DeptKPI);
                }
                catch (Exception ex)
                {
                    string _C = JsonConvert.SerializeObject(c);
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:" + _C);
                }
                _Start++;
            }
            _Budget.DeptKPI = _Budget_DeptKPIList;
            return _Budget;
        }
    }
}