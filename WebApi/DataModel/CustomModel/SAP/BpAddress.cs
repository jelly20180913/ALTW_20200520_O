using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
namespace WebApi.DataModel.CustomModel.SAP
{
    public class BpAddress
    {
        [Description("Customer ID")]
        public string KUNNR { get; set; }
        [Description("Customer Name")]
        public string NAME1 { get; set; }
        [Description("Address1")]
        public string STR_SUPPL1 { get; set; }
        [Description("Address2")]
        public string STR_SUPPL2 { get; set; } 
    }
}