using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DataModel.CustomModel.Edi
{
    public class SOrder
    {
        public Decimal PRICE { get; set; }
        public string VBELN { get; set; }
        public Decimal KWMENG { get; set; }
        public Decimal NETPR { get; set; }
        public string KUNNR { get; set; }
        public string POSNR { get; set; }
        public string ABGRU { get; set; }
        public string MATNR { get; set; }
        public string LIFSP { get; set; }
        public string EDATU { get; set; }
        public string LFSTA { get; set; }
    }
}