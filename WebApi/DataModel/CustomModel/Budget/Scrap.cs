using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DataModel.CustomModel.Budget
{
    public class Scrap
    {
        public int Id { get; set; }
        public string ScrapType { get; set; }
        public string PartNumber { get; set; }
        public string Reason { get; set; }
        public string Month { get; set; }
        public string Quantity { get; set; }
        public string PurchasePrice { get; set; }
        public string BookValue { get; set; }
        public string DisposalAmount { get; set; }
        public string Date { get; set; }
        public string DepartmentId { get; set; }
        public string Version { get; set; }
        public string Factory { get; set; }
    }
}