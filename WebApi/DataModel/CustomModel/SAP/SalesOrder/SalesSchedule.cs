using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
namespace WebApi.DataModel.CustomModel.SAP.SalesOrder
{
    public class SalesSchedule
    {
        [Description("Quantity")]
        public string REQ_QTY { get; set; }
        [Description("ItemNumber")]
        public string ITM_NUMBER { get; set; }
        [Description("ScheduleLine")]
        public string SCHED_LINE { get; set; }
        [Description("Date")]
        public string REQ_DATE { get; set; }

    }
}