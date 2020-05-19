using System.Collections.Generic;
using WebApi.DataModel.CustomModel.Budget;
using WebApi.Models;
namespace WebApi.Service.Interface
{
  public  interface ISapExcelUploadService
    {
        List<string> UploadFile(); 
    }
}
