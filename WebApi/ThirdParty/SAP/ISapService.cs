using WebApi.Models.CustomModel.SAP;
using SAP.Middleware.Connector;
namespace WebApi.ThirdParty.SAP
{
    public interface ISapService
    {
        Budget budgetStart(string id, string kokrs);
       void Sap_CreateOrder(); 
    }
}
