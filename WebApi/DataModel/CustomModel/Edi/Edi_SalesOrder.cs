using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
namespace WebApi.DataModel.CustomModel.Edi
{
    public class Edi_SalesOrder_855
    {
        public Edi_SalesHeader_855 Header { get; set; }
        public List<Edi_SalesItem_855> ItemList { get; set; } 
        public List<Edi_SalesSchedule_855> ScheduleList { get; set; } 
    }
}