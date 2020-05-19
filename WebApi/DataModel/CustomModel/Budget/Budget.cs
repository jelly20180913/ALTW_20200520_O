using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.DataModel.CustomModel.Budget
{
    public class Budget
    {
        public List<Budget_HeadCountHR>  HeadCountHR { get; set; }
        public List<Budget_Capex>  Capex { get; set; }
        public List<Budget_DeptExpense> DeptExpense { get; set; }
        public List<Budget_DeptKPI>  DeptKPI { get; set; }
        public List<Budget_HeadCount>  HeadCount { get; set; }
        public List<Budget_Scrap> Scrap { get; set; }
        public List<Budget_Traveling> Traveling { get; set; }
        public List<ItemCatalog>  ItemCatalog { get; set; }
    }
}