using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface ISAP_PriceListService
    {
        void Create(SAP_PriceList instance);

        void Update(SAP_PriceList instance);

        void Delete(int Id);

        bool IsExists(int Id);

        SAP_PriceList GetByID(int Id);

        IEnumerable<SAP_PriceList> GetAll();
        List<string> MiltiCreate(List<SAP_PriceList> instance); 
    }
}
