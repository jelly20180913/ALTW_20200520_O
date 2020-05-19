using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DataModel.CustomModel.Edi
{
    public class EdiStatus
    {
        public string MANDT { get; set; }
        public string VBELN { get; set; }
        public string SAP_PROC { get; set; }
        public string SEQ { get; set; }
        public string LAEDA { get; set; }
        public string AENAM { get; set; }
    }
}