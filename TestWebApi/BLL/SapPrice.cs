using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using WebApi.DataModel.CustomModel.SAP;
using WebApi.Models;
namespace TestWebApi.BLL
{
  public  class SapPrice
    {
        public List<string> ListError = new List<string>();
        public SapMiddleData Parse(DataTable dtPrice)
        {
            List<SapRate> _SapRateList = initSapRate();
            SapMiddleData _SapMiddleData = new SapMiddleData();
            List<SAP_PriceList> _SAP_PriceList = new List<SAP_PriceList>(); 
            int _Start = 1, _Order = 1;
            string _KNUMH = "";
            SAP_PriceList _Sap_Price = new SAP_PriceList();
            for(int i=0;i<dtPrice.Rows.Count;i++)
            {
                try
                {
                    int _KSTBM = 0;
                    decimal? _KBETR = 0, _Price = 0, _UnitPrice;
                    if (dtPrice.Rows[i]["KNUMH"].ToString() != _KNUMH)
                    {
                        _Order = 1;
                        if (_Start != 1) _SAP_PriceList.Add(_Sap_Price);
                        _Sap_Price = new SAP_PriceList();
                        _KNUMH = dtPrice.Rows[i]["KNUMH"].ToString();//group price 
                        _Sap_Price.CustomerId = dtPrice.Rows[i]["KUNNR"].ToString() ;
                        _Sap_Price.CustomerName = dtPrice.Rows[i]["NAME1"].ToString() ;
                        _Sap_Price.EndCustomerId = dtPrice.Rows[i]["ZZENDCUST"].ToString()  ;
                        _Sap_Price.EndCustomerName = dtPrice.Rows[i]["ZNAME1"].ToString() ;
                        _Sap_Price.PartNumber = dtPrice.Rows[i]["MATNR"].ToString() ;
                        _Sap_Price.Currency = dtPrice.Rows[i]["KONWA"].ToString() ;
                        _Sap_Price.Date = DateTime.Now.Date.ToString("yyyyMMdd");
                        _Sap_Price.Rate = Convert.ToDecimal(_SapRateList.Where(x => x.Currency == _Sap_Price.Currency.Trim()).First().Rate);
                        _Sap_Price.Per = dtPrice.Rows[i]["KPEIN"].ToString() =="" ? 0 : int.Parse(dtPrice.Rows[i]["KPEIN"].ToString(), System.Globalization.NumberStyles.AllowThousands);
                        _Sap_Price.TaxType = dtPrice.Rows[i]["KSCHL"].ToString();
                        _Sap_Price.EffectiveDate= dtPrice.Rows[i]["DATAB"].ToString();
                        _Sap_Price.InsertSapPrice = true;
                    }
                    _KSTBM = dtPrice.Rows[i]["KSTBM"].ToString() == "" ? 0 : int.Parse(dtPrice.Rows[i]["KSTBM"].ToString(), System.Globalization.NumberStyles.Float);
                    string _KONWA = dtPrice.Rows[i]["KONWA"].ToString();
                    if (_KONWA == "TWD" || _KONWA == "JPY") _KBETR = Convert.ToDecimal(dtPrice.Rows[i]["KBETR"].ToString()) * 100;
                    else _KBETR = Convert.ToDecimal(dtPrice.Rows[i]["KBETR"].ToString());
                    _UnitPrice = Convert.ToDecimal(_KBETR / _Sap_Price.Per);
                    _Price = _UnitPrice * _Sap_Price.Rate;
                    if (_Order == 1)
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
                    else if (_Order == 8)
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
                    if (_Start == dtPrice.Rows.Count) _SAP_PriceList.Add(_Sap_Price);
                }
                catch (Exception ex)
                { 
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:" );
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
