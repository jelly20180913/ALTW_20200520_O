using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.DataModel.CustomModel.SAP;
namespace WebApi.Service.Interface
{
   public interface ISapEDIService
    {
        IEnumerable<SAP_PriceList> GetSAP_PriceLists(string date);
        IEnumerable<SAP_PriceList> GetSAP_PriceListsExcludeTax( );
        bool BatchInsert(SapMiddleData sapMiddleData);
        bool Email(string ordernumber, SapSalesOrder sapSalesOrder);
    }
}
