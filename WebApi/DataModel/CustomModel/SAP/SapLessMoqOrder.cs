using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
namespace WebApi.DataModel.CustomModel.SAP
{
    public class SapLessMoqOrder
    {
        [Description("Customer Id")]
        public string KUNNR { get; set; }
        [Description("Customer Name")]
        public string NAME1 { get; set; }
        [Description("Department")]
        public string SPART { get; set; }
        [Description("Department Name")]
        public string DepartmentName { get; set; }
        [Description("Sales")]
        public string SNAME { get; set; }
        [Description("Order")]
        public string VBELN { get; set; }
        [Description("Part Number")]
        public string MATNR { get; set; }
        [Description("Quantity")]
        public string KWMENG { get; set; }
       // [Description("Target Quantity")]
       // public string ZMENG { get; set; }
        [Description("Currency")]
        public string WAERK { get; set; }
        [Description("Sap Price")]
        public string NETPR { get; set; }
        [Description("Per")]
        public string KPEIN { get; set; }
        [Description("Price / Per")]
        public string Cost { get; set; }
        [Description("Min MOQ")]
        public string MINBM { get; set; }
        [Description("MOQ")]
        public string NORBM { get; set; }
        [Description("Order Type")]
        public string AUART { get; set; }
        [Description("Date")]
        public string ERDAT { get; set; }
        [Description("Less MOQ")]
        public string LessMOQ { get; set; }
        [Description("Factory")]
        public string WERKS { get; set; }
    }
}