using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
namespace WebApi.DataModel.CustomModel.SAP
{
    public class BpCountry
    {
        [Description("SPRAS")]
        public string SPRAS { get; set; }
        [Description("Country Code")]
        public string LAND1 { get; set; }
        [Description("Country")]
        public string LANDX { get; set; }
    }
}