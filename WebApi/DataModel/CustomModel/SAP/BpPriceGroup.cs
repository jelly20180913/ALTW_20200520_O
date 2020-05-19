using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
namespace WebApi.DataModel.CustomModel.SAP
{
    public class BpPriceGroup
    {
        [Description("Customer ID")]
        public string KUNNR { get; set; }
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("價格表類型")]
        public string PriceGroup { get; set; }
        [Description("加價率")]
        public string BAHNE { get; set; }
    }
}