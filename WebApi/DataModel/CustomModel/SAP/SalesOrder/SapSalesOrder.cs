using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DataModel.CustomModel.SAP.SalesOrder;
namespace WebApi.DataModel.CustomModel.SAP
{
    public class SapSalesOrder
    {
        public SalesHeader Header { get; set; }
        public List<SalesItem> ItemList { get  ; set; }
        public List<SalesPartner> PartnerList { get; set; }
        public List<SalesSchedule> ScheduleList { get; set; }

        public string CustomerName { get; set; }
        public string SalesEmail { get; set; }
        public string CreateBy { get; set; }
    }
}