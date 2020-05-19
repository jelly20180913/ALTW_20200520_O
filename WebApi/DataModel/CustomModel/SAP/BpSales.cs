using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
namespace WebApi.DataModel.CustomModel.SAP
{
    public class BpSales
    {
        [Description("Sales Area")]
        public string vkorg { get; set; }
        [Description("Customer Id")]
        public string kunnr { get; set; }
        [Description("Customer Name")]
        public string name1 { get; set; }
        [Description("Customer Name2")]
        public string name2 { get; set; }
        [Description("Sales")]
        public string SNAME { get; set; } 
    }
}