using System.Collections.Generic;
using WebApi.DataModel.CustomModel.Budget;
using WebApi.Models;
using System;
namespace WebApi.Service.Interface
{
    public interface IBudgetExcelUploadService
    {
        List<string> UploadFile();
        List<DropDownDepartment> GetDepartmentByAccount(string account, string factory);
        List<Budget_DepartmentReport> GetBudgetTypeByDepartmentId(string factory, string departmentId);
        List<DropDownItemCatalog> GetCommonCostByAccount(string account, string factory);
        bool UpdateFileVersionBudgetApprove(int Id);
        List<FileVersionBudget> GetFileVersionBudgetList();
        List<HeadCountHR> GetHeadCountHRList();
        bool UpdateHeadCountHR(HeadCountHR headCountHR);
        List<Budget_DepartmentReport> GetDepartment();
        bool UpdateHeadCount(HeadCountHR headCountHR);
    } 
}
