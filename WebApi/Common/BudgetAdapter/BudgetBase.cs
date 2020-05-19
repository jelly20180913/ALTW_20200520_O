using System.Collections.Generic;
using WebApi.DataModel.CustomModel.Budget;
namespace WebApi.Common.BudgetAdapter
{
    public class BudgetBase  
    {
        public List<string> ListError = new List<string>();
        public string DepartmentId { get; set; }
        public string Factory { get; set; }
        public string Version { get; set; }
        public bool CommonBudget { get; set; }
        public bool PartTime { get; set; }
        public string ItemId_CostCommon { get; set; }
        public virtual  Budget  Parse(string _FilePath)
        {
            Budget  _Budget = new  Budget();
            return _Budget;
        } 
    }
}