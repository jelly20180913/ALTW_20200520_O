using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
namespace WebApi.DataModel.CustomModel.SAP
{
    public class BpXY
    {
        [Description("Year")]
        public string Year { get; set; }
        [Description("Customer ID")]
        public string KUNNR { get; set; }
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("Customer Type")]
        public string CustomerType { get; set; }
        [Description("Country/State")]
        public string CountryState { get; set; }
        [Description("Region")]
        public string Region { get; set; }
        [Description("Currency (NTD) Revenue")]
        public string Currency_NTD_Revenue { get; set; }
        [Description("Currency (USD) Revenue")]
        public string Currency_USD_Revenue { get; set; }
        [Description("Currency (JPD) Revenue")]
        public string Currency_JPD_Revenue { get; set; }
        [Description("Currency (EUR) Revenue")]
        public string Currency_EUR_Revenue { get; set; }
        [Description("Currency (RMB) Revenue")]
        public string Currency_RMB_Revenue { get; set; }
        [Description("Address")]
        public string Address { get; set; }
       
        [Description("No. of Visiting")]
        public string Visiting { get; set; }
    }
}