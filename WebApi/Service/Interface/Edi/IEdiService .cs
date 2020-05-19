using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.DataModel.CustomModel.Edi;
using System.Net.Http;
using WebApi.DataModel.CustomModel.SAP;
namespace WebApi.Service.Interface
{
    public interface IEdiService
    {
        List<Edi_Customer> GetEdi_CustomerList(bool mode);
        void InsertSapSalesOrder(SapSalesOrder sapSalesOrder, string orderNumber);
        List<Edi_SalesHeader> GetEdi_SalesHeaderList();
        List<Edi_SalesItem> GetEdi_SalesItemList(string orderNumber);
        List<Edi_SalesSchedule> GetEdi_SalesScheduleList(string orderNumber);
        void InsertEdi_SalesOrder(Edi_SalesOrder_855 edi_SalesOrder_855);
    }
}
