using WebApi.Service.Interface.Table;
using WebApi.Service.Interface.Common;
namespace WebApi.Common.BudgetAdapter
{
    public class BudgetAdapterFactory : IBudgetAdapterFactory
    {
        IItemCatalogService _itemCatalogService;
        IBudget_HeadCountHRService _budget_HeadCountHRService;
        IBudget_CostService _budget_CostService;
        IBudget_DeptExpenseService _budget_DeptExpenseService;
        IBudget_CostTravelingMappingService _budget_CostTravelingMappingService;
        ICommonFileService _commonFileService;
        public BudgetAdapterFactory(IItemCatalogService itemCatalogService, IBudget_HeadCountHRService budget_HeadCountHRService, IBudget_CostService budget_CostService,IBudget_DeptExpenseService budget_DeptExpenseService, IBudget_CostTravelingMappingService budget_CostTravelingMappingService, ICommonFileService commonFileService)
        {
            this._itemCatalogService = itemCatalogService;
            this._budget_HeadCountHRService = budget_HeadCountHRService;
            this._budget_CostService = budget_CostService;
            this._budget_DeptExpenseService = budget_DeptExpenseService;
            this._budget_CostTravelingMappingService = budget_CostTravelingMappingService;
            this._commonFileService = commonFileService;
        }
        /// <summary>
        /// according to item id new mapping object
        /// </summary>
        /// <param name=" itemId"></param>
        ///  Headcount	 00090001
        ///  Scrap	     00090002
        ///  DeptKPI	 00090003
        ///  DeptExpense 00090004
        ///  Capex	     00090005
        ///  Travelling  00090006
        /// <returns></returns>
        public BudgetBase CreateBudgetAdapter(string itemId)
        {
            if (itemId == "Headcount" || itemId == "PartTime")
                return new BudgetExcelHeadCount(this._budget_HeadCountHRService, this._itemCatalogService);
            else if (itemId == "Scrap")
                return new BudgetExcelScrap(this._itemCatalogService, this._commonFileService);
            else if (itemId == "KPI")
                return new BudgetExcelDeptKPI();
            else if (itemId == "DeptExpense"||itemId == "CostCommon"  )
                return new BudgetExcelDeptExpense(this._budget_CostService,this._budget_DeptExpenseService, this._commonFileService);
            else if (itemId == "Capex")
                return new BudgetExcelCapex(this._itemCatalogService,this._commonFileService);
            else if (itemId == "Travelling")
                return new BudgetExcelTravelling(this._budget_HeadCountHRService, this._itemCatalogService, this._budget_CostTravelingMappingService, this._commonFileService);
            else if (itemId == "ItemCatalog")
                return new BudgetItemCatalog();
            return new BudgetExcelHeadCountHR(this._itemCatalogService);
        }

    }
}