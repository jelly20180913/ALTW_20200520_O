using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DataModel.CustomModel.Budget
{
    public class DeptExpense
    { 
        public string DescName { get; set; }
        public string YTD { get; set; }
        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sep { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }
        public string Date { get; set; }
        public string DepartmentId { get; set; }
        public string Version { get; set; }
        public string Factory { get; set; }
        public string ItemId_CostElement { get; set; }
    }
}