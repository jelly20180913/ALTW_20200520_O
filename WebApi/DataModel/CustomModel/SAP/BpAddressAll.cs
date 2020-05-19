using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
namespace WebApi.DataModel.CustomModel.SAP
{
    public class BpAddressAll
    {
        [Description("Customer ID")]
        public string KUNNR { get; set; }
        [Description("Customer Name")]
        public string NAME1 { get; set; }
        [Description("Address1")]
        public string STR_SUPPL1 { get; set; }
        [Description("Address2")]
        public string STR_SUPPL2 { get; set; }
        [Description("Street")]
        public string Street { get; set; }
        [Description("Street 5")]
        public string LOCATION { get; set; }
        [Description("Zip code")]
        public string POST_CODE1 { get; set; }
        [Description("City")]
        public string CITY1 { get; set; }
        [Description("Country")]
        public string COUNTRY { get; set; }
        [Description("Region")]
        public string REGION { get; set; }
        [Description("Time Zone")]
        public string TIME_ZONE { get; set; }
        [Description("Address3")]
        public string CITY2 { get; set; }
        [Description("Street 4")]
        public string str_suppl3 { get; set; }
        [Description("Diff City")]
        public string HOME_CITY { get; set; }
        [Description("Sales Area")]
        public string VKORG { get; set; }
    }
}