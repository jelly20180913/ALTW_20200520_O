using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
namespace WebApi.DataModel.CustomModel.SAP.SalesOrder
{
    public class SalesHeader
    {
        [Description("OrderType")]
        public string DOC_TYPE { get; set; }
        [Description("SalesArea")]
        public string SALES_ORG { get; set; }
        [Description("Distribut")]
        public string DISTR_CHAN { get; set; }
        [Description("Division")]
        public string DIVISION { get; set; }
        [Description("PurchaseNo")]
        public string PURCH_NO_C { get; set; }
        [Description("PurchaseDate")]
        public string PURCH_DATE { get; set; }
    }
}