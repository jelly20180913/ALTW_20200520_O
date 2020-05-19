using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DataModel.CustomModel.Budget
{
    public class DropDownBudgetType
    {
        public string DeptExpense { get; set; }
        public string Scrap { get; set; }
        public string Travelling { get; set; }
        public string KPI { get; set; }
        public string Headcount { get; set; }
        public string Capex { get; set; }
    }
}