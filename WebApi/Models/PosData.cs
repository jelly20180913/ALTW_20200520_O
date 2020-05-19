//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PosData
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string MonthYear { get; set; }
        public string Distributor { get; set; }
        public string Customer { get; set; }
        public string ISOCountryCode { get; set; }
        public string Country { get; set; }
        public string SalesArea { get; set; }
        public string SalesManager { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string PartNo { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<decimal> TotalSalesBaseCurreny { get; set; }
        public Nullable<decimal> TotalSalesEUR { get; set; }
        public string ProductSeries { get; set; }
        public int FK_LoginId { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
