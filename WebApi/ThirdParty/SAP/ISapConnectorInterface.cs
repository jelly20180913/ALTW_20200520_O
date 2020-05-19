using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using WebApi.DataModel.CustomModel.SAP;
namespace WebApi.ThirdParty.SAP
{
   public interface ISapConnectorInterface
    {
        bool TestConnection(string destinationName);
        string CreateOrder(SapSalesOrder salesOrder);
        DataSet GetBudget(string destinationName);
        string CreateBusinessPartner();
        string CreateCustomer(string number);
        string CreateBP( );
        string CreateSalesArea(string number);
    }
}
