using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using WebApi.DataModel.CustomModel.SAP.Mapping;
using WebApi.DataModel.CustomModel.SAP;
namespace TestWebApi.BLL
{
    public class BpXY
    {
        public List<string> ListError = new List<string>();
        /// <summary>
        /// delete duplicate data (because sales area has  2000/1000)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtRegion">US/CN use </param> 
        /// <returns></returns>
        public List<WebApi.DataModel.CustomModel.SAP.BpXY> Parse(DataTable dt, DataTable dtRegion)
        {
            List<WebApi.DataModel.CustomModel.SAP.BpXY> _BpXYList = new List<WebApi.DataModel.CustomModel.SAP.BpXY>();
            List<CustomerType> _CustomerTypeList = TestWebApi.Common.Mapping.CustomerTypeList;
            List<CountryRegion> _CountryRegionList = TestWebApi.Common.Mapping.CountryRegionList;
            int _Start = 1;
            string _TempKUNNR = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    WebApi.DataModel.CustomModel.SAP.BpXY _BpXY = new WebApi.DataModel.CustomModel.SAP.BpXY();
                    _BpXY.KUNNR = dt.Rows[i]["KUNNR"].ToString();
                    string _CustomerName = "";
                    if (dt.Rows[i]["BU_SORT2"].ToString() == dt.Rows[i]["BU_SORT1"].ToString()) { _CustomerName = dt.Rows[i]["BU_SORT2"].ToString(); }
                    else { _CustomerName = dt.Rows[i]["BU_SORT2"].ToString() + " " + dt.Rows[i]["BU_SORT1"].ToString(); }
                    _BpXY.CustomerName = _CustomerName; 
                    _BpXY.Address = dt.Rows[i]["NAME_CO"].ToString();
                    if (_CustomerTypeList.Where(x => x.CGrp == dt.Rows[i]["KDGRP"].ToString()) != null)
                        _BpXY.CustomerType = _CustomerTypeList.Where(x => x.CGrp == dt.Rows[i]["KDGRP"].ToString()).First().OfficialName;
                    if (dt.Rows[i]["LAND1"].ToString() == "CN" || dt.Rows[i]["LAND1"].ToString() == "US")
                    {
                        if (dt.Rows[i]["REGION"].ToString() != "")
                            _BpXY.CountryState = dtRegion.AsEnumerable().Where(x => x.Field<string>("BLAND") == dt.Rows[i]["REGION"].ToString()).First().Field<string>("BEZEI");
                        else _BpXY.CountryState = "沒維護";
                    }
                    else _BpXY.CountryState = dt.Rows[i]["LANDX"].ToString();
                    if (_CountryRegionList.Where(x => x.Ctr == dt.Rows[i]["LAND1"].ToString()) != null)
                        _BpXY.Region = _CountryRegionList.Where(x => x.Ctr == dt.Rows[i]["LAND1"].ToString()).First().OfficialRegion;
                    if (dt.Rows[i]["KUNNR"].ToString() != _TempKUNNR) _BpXYList.Add(_BpXY);
                    _TempKUNNR = dt.Rows[i]["KUNNR"].ToString();

                }
                catch (Exception ex)
                {
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:");
                }
                _Start++;
            }
            return _BpXYList;
        }
        public List<WebApi.DataModel.CustomModel.SAP.BpXY> ParseRevenue(List<WebApi.DataModel.CustomModel.SAP.BpXY> bpXY, DataTable dtCE11000)
        {
            int _Start = 1;
            List<WebApi.DataModel.CustomModel.SAP.BpXY> _BpXYList = new List<WebApi.DataModel.CustomModel.SAP.BpXY>();
            List<BpXY_GroupBy> _BpXY_GroupByList = new List<BpXY_GroupBy>();
            string[] _Year = new string[] { "2018", "2019" };
            foreach (string y in _Year)
            {
                //tune performance  350(s) to 4(s)
                _BpXY_GroupByList = dtCE11000.AsEnumerable().Where(x => x.Field<string>("GJAHR") == y).ToList().GroupBy(x => x.Field<string>("KNDNR")).Select(x => new BpXY_GroupBy
                {
                    KNDNR = x.Key,
                    Sum_ERLOS = x.Sum(z => z.Field<decimal>("ERLOS"))
                }).ToList();
                foreach (WebApi.DataModel.CustomModel.SAP.BpXY c in bpXY)
                {
                    try
                    {
                        WebApi.DataModel.CustomModel.SAP.BpXY _BpXY = new WebApi.DataModel.CustomModel.SAP.BpXY();
                        _BpXY.Year = y;
                        decimal _ERLOS = 0;
                        if (_BpXY_GroupByList.Any(x => x.KNDNR == c.KUNNR))
                            _ERLOS = _BpXY_GroupByList.Where(x => x.KNDNR == c.KUNNR).First().Sum_ERLOS * 100;
                        _BpXY.Currency_NTD_Revenue = _ERLOS.ToString();
                        _BpXY.KUNNR = c.KUNNR;
                        _BpXY.CustomerName = c.CustomerName;
                        _BpXY.CustomerType = c.CustomerType;
                        _BpXY.CountryState = c.CountryState;
                        _BpXY.Region = c.Region;
                        _BpXY.Address = c.Address;
                        _BpXYList.Add(_BpXY);
                    }
                    catch (Exception ex)
                    {
                        this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:");
                    }
                    _Start++;

                }
            }
            return _BpXYList;
        }
    }
}
