using System.Linq;
using WebApi.Models;
using WebApi.Service.Interface;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace WebApi.Common.FileAdapter
{
    public class FileExcel:FileBase
    {
        private IPosColumnMapService _posColumnMapService;
        public FileExcel( IPosColumnMapService posColumnMapService )
        { 
            this._posColumnMapService = posColumnMapService; 
        } 
        public override IQueryable<PosData> Parse(int _Model, string filePath)
        {
            IQueryable<PosData> _PosList;
            PosColumnMap _PosColumnMap = _posColumnMapService.GetByID(_Model);
            LinqToExcel.ExcelQueryFactory _Excel = new LinqToExcel.ExcelQueryFactory(filePath);
            _Excel.AddMapping<PosData>(d => d.Year, _PosColumnMap.Year);
            _Excel.AddMapping<PosData>(d => d.MonthYear, _PosColumnMap.MonthYear);
            _Excel.AddMapping<PosData>(d => d.Distributor, _PosColumnMap.Distributor);
            _Excel.AddMapping<PosData>(d => d.Customer, _PosColumnMap.Customer);
            _Excel.AddMapping<PosData>(d => d.ISOCountryCode, _PosColumnMap.ISOCountryCode);
            _Excel.AddMapping<PosData>(d => d.Country, _PosColumnMap.Country);
            _Excel.AddMapping<PosData>(d => d.SalesArea, _PosColumnMap.SalesArea);
            _Excel.AddMapping<PosData>(d => d.SalesManager, _PosColumnMap.SalesManager);
            _Excel.AddMapping<PosData>(d => d.City, _PosColumnMap.City);
            _Excel.AddMapping<PosData>(d => d.PostCode, _PosColumnMap.PostCode);
            _Excel.AddMapping<PosData>(d => d.PartNo, _PosColumnMap.PartNo);
            _Excel.AddMapping<PosData>(d => d.BaseCurrency, _PosColumnMap.BaseCurrency);
            _Excel.AddMapping<PosData>(d => d.Qty, _PosColumnMap.Qty.ToString());
            _Excel.AddMapping<PosData>(d => d.TotalSalesBaseCurreny, _PosColumnMap.TotalSalesBaseCurrency);
            _Excel.AddMapping<PosData>(d => d.TotalSalesEUR, _PosColumnMap.TotalSalesEUR);
            _Excel.AddMapping<PosData>(d => d.ProductSeries, _PosColumnMap.ProductSeries);
            // every time get first sheet data
            _PosList = from x in _Excel.Worksheet<PosData>(0)
                       select x;
            return _PosList;
        }
        public override IQueryable<Edi_Pos> ParseEdiPos(  string filePath)
        {
            IQueryable<Edi_Pos> _Edi_PosQuery;
            List<Edi_Pos> _Edi_PosList = new List<Edi_Pos>();
            LinqToExcel.ExcelQueryFactory _Excel = new LinqToExcel.ExcelQueryFactory(filePath); 
            _Excel.AddMapping<Edi_Pos>(d => d.Type, "Type");
            _Excel.AddMapping<Edi_Pos>(d => d.Distributor, "DIST"); 
            _Excel.AddMapping<Edi_Pos>(d => d.Address, "Address");
            _Excel.AddMapping<Edi_Pos>(d => d.City, "CITY");
            _Excel.AddMapping<Edi_Pos>(d => d.State, "STATE");
            _Excel.AddMapping<Edi_Pos>(d => d.ZIP, "ZIP");
            _Excel.AddMapping<Edi_Pos>(d => d.Series, "Series");
            _Excel.AddMapping<Edi_Pos>(d => d.PartNo, "PARTNO");
            _Excel.AddMapping<Edi_Pos>(d => d.Quantity, "QTY");
            _Excel.AddMapping<Edi_Pos>(d => d.Cost, "COST");
            _Excel.AddMapping<Edi_Pos>(d => d.Price, "PRICE");
            _Excel.AddMapping<Edi_Pos>(d => d.ResellingExt, "RESEXT");
            _Excel.AddMapping<Edi_Pos>(d => d.ACCT, "ACCT");
            _Excel.AddMapping<Edi_Pos>(d => d.CustomerName, "NAME");
            _Excel.AddMapping<Edi_Pos>(d => d.MarketCode, "Corporate Market");
            _Excel.AddMapping<Edi_Pos>(d => d.MarketCode, "Market Code");
            _Excel.AddMapping<Edi_Pos>(d => d.Market, "Market Segment");
            _Excel.AddMapping<Edi_Pos>(d => d.SubSegmentCode, "Sub-Segment Code");
            _Excel.AddMapping<Edi_Pos>(d => d.SubSegment, "Sub-Segment");
            _Excel.AddMapping<Edi_Pos>(d => d.Remarks, "Remarks");
            _Excel.AddMapping<Edi_Pos>(d => d.ShipDate, "SHIP DATE");
            _Excel.AddMapping<Edi_Pos>(d => d.ShipDate2, "SHIP DATE 2");
            _Excel.AddMapping<Edi_Pos>(d => d.ShipMonth, "SHIP MONTH");
            _Excel.AddMapping<Edi_Pos>(d => d.ShipQuarter, "SHIP QUARTER");
            _Excel.AddMapping<Edi_Pos>(d => d.YY, "YY");
            _Excel.AddMapping<Edi_Pos>(d => d.CountryCode, "COUNTRY CODE");
            _Excel.AddMapping<Edi_Pos>(d => d.Country, "COUNTRY ");
            _Excel.AddMapping<Edi_Pos>(d => d.Region, "Region");
            int _Start = 1;
           // _Edi_PosQuery = from x in _Excel.Worksheet<Edi_Pos>(0)
           //            select x;
            _Edi_PosQuery = from x in _Excel.Worksheet<Edi_Pos>("Data")
                            select x;
            foreach (Edi_Pos c in _Edi_PosQuery)
            { 
                try
                { 
                    c.ShipDate2 = c.ShipDate2!=null?DateTime.Parse(c.ShipDate2).ToString("yyyy/MM/dd"):"";
                    c.Status = "T";
                    _Edi_PosList.Add(c);
                }
                catch (Exception ex)
                {
                    string _c = JsonConvert.SerializeObject(c);
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:" + _c);
                }
                _Start++;
            }
            _Edi_PosQuery = _Edi_PosList.AsQueryable();
            return _Edi_PosQuery;
        }
    }
}