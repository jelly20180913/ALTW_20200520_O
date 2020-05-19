using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DataModel.CustomModel.SAP
{
    public class SapRate
    {
        public string Type { get; set; }
        public string Currency { get; set; }
        public double Rate { get; set; }
        public SapRate()
        {
        }
    }
}