using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DataModel.CustomModel.SAP;
using WebApi.Models;
using Newtonsoft.Json;
namespace WebApi.Common.SapAdapter
{
    public class SapExcelPriceList : SapBase
    {
        public SapExcelPriceList()
        {
        }
        public override SapMiddleData Parse(string filePath)
        {
            List<SapRate> _SapRateList = initSapRate();
            SapMiddleData _SapMiddleData = new SapMiddleData();
            List<SAP_PriceList> _SAP_PriceList = new List<SAP_PriceList>();
            IQueryable<PriceList> _PriceList;
            LinqToExcel.ExcelQueryFactory _Excel = new LinqToExcel.ExcelQueryFactory(filePath);
            _Excel.AddMapping<PriceList>(d => d.KUNNR, "KUNNR");
            _Excel.AddMapping<PriceList>(d => d.NAME1, "NAME1");
            if (base.PriceType == "終端客戶價格表A501")
            {
                _Excel.AddMapping<PriceList>(d => d.ZZENDCUST, "ZZENDCUST");
                _Excel.AddMapping<PriceList>(d => d.ZNAME1, "ZNAME1");
            }
            _Excel.AddMapping<PriceList>(d => d.MATNR, "MATNR");
            _Excel.AddMapping<PriceList>(d => d.KONWA, "KONWA");
            _Excel.AddMapping<PriceList>(d => d.KSTBM, "KSTBM");
            _Excel.AddMapping<PriceList>(d => d.KBETR, "KBETR");
            _Excel.AddMapping<PriceList>(d => d.KLFN1, "KLFN1");
            _Excel.AddMapping<PriceList>(d => d.KPEIN, "KPEIN");
            _Excel.AddMapping<PriceList>(d => d.KNUMH, "KNUMH");
            _Excel.AddMapping<PriceList>(d => d.KSCHL, "KSCHL");
            // every time get first sheet data
            _PriceList = from x in _Excel.Worksheet<PriceList>(0)
                         select x;
            int _Start = 1,_Order=1;
            string _KNUMH = "";
            SAP_PriceList _Sap_Price = new SAP_PriceList();
            foreach (PriceList c in _PriceList)
            {
                try
                {
                    int _KSTBM = 0;
                    decimal? _KBETR = 0, _Price = 0, _UnitPrice;
                    if (c.KNUMH != _KNUMH)
                    {
                        _Order = 1;
                        if (_Start != 1) _SAP_PriceList.Add(_Sap_Price);
                        _Sap_Price = new SAP_PriceList();
                        _KNUMH = c.KNUMH;//group price 
                        _Sap_Price.CustomerId = c.KUNNR;
                        _Sap_Price.CustomerName = c.NAME1;
                        _Sap_Price.EndCustomerId = c.ZZENDCUST;
                        _Sap_Price.EndCustomerName = c.ZNAME1;
                        _Sap_Price.PartNumber = c.MATNR;
                        _Sap_Price.Currency = c.KONWA;
                        _Sap_Price.Date = DateTime.Now.Date.ToString("yyyyMMdd");
                        _Sap_Price.Rate = Convert.ToDecimal(_SapRateList.Where(x => x.Currency == c.KONWA.Trim()).First().Rate);
                        _Sap_Price.Per = c.KPEIN == null ? 0 : int.Parse(c.KPEIN, System.Globalization.NumberStyles.AllowThousands);
                        _Sap_Price.TaxType = c.KSCHL;
                    }
                    _KSTBM = c.KSTBM == null ? 0 : int.Parse(c.KSTBM, System.Globalization.NumberStyles.AllowThousands);
                    if (c.KONWA == "TWD"|| c.KONWA == "JPY") _KBETR = Convert.ToDecimal(c.KBETR) * 100;
                    else _KBETR = Convert.ToDecimal(c.KBETR);
                    _UnitPrice = Convert.ToDecimal(_KBETR / _Sap_Price.Per);
                    _Price = _UnitPrice * _Sap_Price.Rate;
                    if (_Order  == 1)
                    {
                        _Sap_Price.MOQ1 = _KSTBM;
                        _Sap_Price.Price1 = _Price;
                    }
                    else if (_Order == 2)
                    {
                        _Sap_Price.MOQ2 = _KSTBM;
                        _Sap_Price.Price2 = _Price;
                    }
                    else if (_Order == 3)
                    {
                        _Sap_Price.MOQ3 = _KSTBM;
                        _Sap_Price.Price3 = _Price;
                    }
                    else if (_Order == 4)
                    {
                        _Sap_Price.MOQ4 = _KSTBM;
                        _Sap_Price.Price4 = _Price;
                    }
                    else if (_Order == 5)
                    {
                        _Sap_Price.MOQ5 = _KSTBM;
                        _Sap_Price.Price5 = _Price;
                    }
                    else if (_Order == 6)
                    {
                        _Sap_Price.MOQ6 = _KSTBM;
                        _Sap_Price.Price6 = _Price;
                    }
                    else if (_Order == 7)
                    {
                        _Sap_Price.MOQ7 = _KSTBM;
                        _Sap_Price.Price7 = _Price;
                    }
                    else if (_Order ==8)
                    {
                        _Sap_Price.MOQ8 = _KSTBM;
                        _Sap_Price.Price8 = _Price;
                    }
                    else if (_Order == 9)
                    {
                        _Sap_Price.MOQ9 = _KSTBM;
                        _Sap_Price.Price9 = _Price;
                    }
                    else if (_Order == 10)
                    {
                        _Sap_Price.MOQ10 = _KSTBM;
                        _Sap_Price.Price10 = _Price;
                    }
                    _Order++;
                    if (_Start == _PriceList.Count()) _SAP_PriceList.Add(_Sap_Price);
                }
                catch (Exception ex)
                {
                    string _c = JsonConvert.SerializeObject(c);
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:" + _c);
                }
                _Start++;
            }
            _SapMiddleData.PriceList = _SAP_PriceList;
            return _SapMiddleData;
        }
        private List<SapRate> initSapRate()
        {
            List<SapRate> _SapRateList = new List<SapRate> {
                new SapRate { Type = "Q", Currency = "CNY", Rate = 4.1 },
                 new SapRate { Type = "Q", Currency = "RMB", Rate = 4.1 },
                  new SapRate { Type = "Q", Currency = "EUR", Rate = 33 },
                    new SapRate { Type = "Q", Currency = "HKD", Rate = 3.6 },
                      new SapRate { Type = "Q", Currency = "JPY", Rate = 0.2 },
                        new SapRate { Type = "Q", Currency = "USD", Rate = 27 },
                             new SapRate { Type = "Q", Currency = "TWD", Rate = 1 }
            };
            return _SapRateList;
        }
    }
}