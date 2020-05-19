using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace TestWebApi.Model
{
   public class SapPriceListExcel
    { 
        [Description("Customer ID")]
        public string CustomerId { get; set; }
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("End Customer ID")]
        public string EndCustomerId { get; set; }
        [Description("End Customer Name")]
        public string EndCustomerName { get; set; }
        [Description("Part Number")]
        public string PartNumber { get; set; }
        [Description("Currency")]
        public string Currency { get; set; }
        [Description("MOQ 1")]
        public Nullable<int> MOQ1 { get; set; }
        [Description("Price 1")]
        public Nullable<decimal> Price1 { get; set; }
        [Description("Price 1 Tax")]
        public Nullable<int> Tax1 { get; set; }
        [Description("MOQ 2")]
        public Nullable<int> MOQ2 { get; set; }
        [Description("Price 2")]
        public Nullable<decimal> Price2 { get; set; }
        [Description("Price 2 Tax")]
        public Nullable<int> Tax2 { get; set; }
        [Description("MOQ 3")]
        public Nullable<int> MOQ3 { get; set; }
        [Description("Price 3")]
        public Nullable<decimal> Price3 { get; set; }
        [Description("Price 3 Tax")]
        public Nullable<int> Tax3 { get; set; }
        [Description("MOQ 4")]
        public Nullable<int> MOQ4 { get; set; }
        [Description("Price 4")]
        public Nullable<decimal> Price4 { get; set; }
        [Description("Price 4 Tax")]
        public Nullable<int> Tax4 { get; set; }
        [Description("MOQ 5")]
        public Nullable<int> MOQ5 { get; set; }
        [Description("Price 5")]
        public Nullable<decimal> Price5 { get; set; }
        [Description("Price 5 Tax")]
        public Nullable<int> Tax5 { get; set; }
        [Description("MOQ 6")]
        public Nullable<int> MOQ6 { get; set; }
        [Description("Price 6")]
        public Nullable<decimal> Price6 { get; set; }
        [Description("Price 6 Tax")]
        public Nullable<int> Tax6 { get; set; }
        [Description("MOQ 7")]
        public Nullable<int> MOQ7 { get; set; }
        [Description("Price 7")]
        public Nullable<decimal> Price7 { get; set; }
        [Description("Price 7 Tax")]
        public Nullable<int> Tax7 { get; set; }
        [Description("MOQ 8")]
        public Nullable<int> MOQ8 { get; set; }
        [Description("Price 8")]
        public Nullable<decimal> Price8 { get; set; }
        [Description("Price 8 Tax")]
        public Nullable<int> Tax8 { get; set; }
        [Description("MOQ 9")]
        public Nullable<int> MOQ9 { get; set; }
        [Description("Price 9")]
        public Nullable<decimal> Price9 { get; set; }
        [Description("Price 9 Tax")]
        public Nullable<int> Tax9 { get; set; }
        [Description("MOQ 10")]
        public Nullable<int> MOQ10 { get; set; }
        [Description("Price 10")]
        public Nullable<decimal> Price10 { get; set; }
        [Description("Price 10 Tax")]
        public Nullable<int> Tax10 { get; set; }
        
    }
}
