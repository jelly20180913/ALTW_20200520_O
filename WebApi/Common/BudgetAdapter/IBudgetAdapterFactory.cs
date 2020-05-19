namespace WebApi.Common.BudgetAdapter
{
    public  interface IBudgetAdapterFactory
    {
        BudgetBase CreateBudgetAdapter(string budgetType);
    }
}
