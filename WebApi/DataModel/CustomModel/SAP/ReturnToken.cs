using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DataModel.CustomModel.SAP
{
    public class ReturnToken
    {
        public string token { get; set; }

        public string loginId { get; set; }

        public string indexPage { get; set; }
    }
}