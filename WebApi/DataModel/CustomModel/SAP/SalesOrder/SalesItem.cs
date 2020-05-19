using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
namespace WebApi.DataModel.CustomModel.SAP.SalesOrder
{
    public class SalesItem
    {
        [Description("ItemNumber")]
        public string ITM_NUMBER { get; set; }
        [Description("PartNumber")]
        public string MATERIAL { get; set; }
        [Description("Quantity")]
        public string TARGET_QTY { get; set; }
        [Description("CustomerPartNumber")]
        public string CUST_MAT35 { get; set; }
        [Description("CustomerItemNumber")]
        public string CustomerItemNumber{ get; set; }
        [Description("CustomerUnit")]
        public string CustomerUnit { get; set; }
        [Description("CustomerPrice")]
        public string CustomerPrice { get; set; }
        [Description("CustomerUnitOfPrice")]
        public string CustomerUnitOfPrice { get; set; }
    }
}