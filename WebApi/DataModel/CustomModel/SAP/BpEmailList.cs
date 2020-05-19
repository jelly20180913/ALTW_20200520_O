using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
namespace WebApi.DataModel.CustomModel.SAP
{
    public class BpEmail
    {
        [Description("Customer ID")]
        public string PARTNER1 { get; set; }
        [Description("Name")]
        public string BU_SORT1 { get; set; }
        [Description("Nickname")]
        public string NAME_FIRST { get; set; }
        [Description("Email")]
        public string SMTP_ADDR { get; set; }
        [Description("Company Name")]
        public string NAME_ORG { get; set; }
    }
}