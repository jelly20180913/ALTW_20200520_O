using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
namespace WebApi.DataModel.CustomModel.SAP.SalesOrder
{
    public class SalesPartner
    {
        [Description("PartnerRole")]
        public string PARTN_ROLE { get; set; }
        [Description("PartnerNo")]
        public string PARTN_NUMB { get; set; }
        [Description("Street")]
        public string STREET { get; set; }
        [Description("City")]
        public string CITY { get; set; }
        [Description("Region")]
        public string REGION { get; set; }
        [Description("PostlCode")]
        public string POSTL_CODE { get; set; }
        [Description("Country")]
        public string COUNTRY { get; set; }
        [Description("Name")]
        public string NAME { get; set; }
        [Description("Telephone")]
        public string TELEPHONE { get; set; }
    }
}