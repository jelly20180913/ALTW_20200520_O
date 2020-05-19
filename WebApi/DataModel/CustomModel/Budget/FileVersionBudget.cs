using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DataModel.CustomModel.Budget
{
    public class FileVersionBudget
    {
        public int Id { get; set; }
        public string Account { get; set; } 
        public string Date { get; set; }
        public string Version { get; set; }
        public Nullable<bool> Approve { get; set; } 
        public string Factory { get; set; } 
        public string AltwName { get; set; }
        public string BudgetName { get; set; }
        public string DepartmentName { get; set; }
    }
}