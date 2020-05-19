using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace TestWebApi.BLL
{
    public class SapMOQ
    {
        public List<string> ListError = new List<string>();
        public List<WebApi.DataModel.CustomModel.SAP.SapLessMoqOrder> Parse(DataTable dt, DataTable dtMoq, DataTable dtDepartment, string tag)
        {
            List<WebApi.DataModel.CustomModel.SAP.SapLessMoqOrder> _SapLessMoqOrderList = new List<WebApi.DataModel.CustomModel.SAP.SapLessMoqOrder>();
            int _Start = 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    WebApi.DataModel.CustomModel.SAP.SapLessMoqOrder _SapLessMoqOrder = new WebApi.DataModel.CustomModel.SAP.SapLessMoqOrder();
                    _SapLessMoqOrder.KUNNR = dt.Rows[i]["KUNNR"].ToString();
                    _SapLessMoqOrder.NAME1 = dt.Rows[i]["NAME1"].ToString();
                    _SapLessMoqOrder.SPART = dt.Rows[i]["SPART"].ToString();
                    _SapLessMoqOrder.DepartmentName = dtDepartment.AsEnumerable().Where(x => x.Field<string>("SPART") == _SapLessMoqOrder.SPART).First().Field<string>("VTEXT");
                    _SapLessMoqOrder.SNAME = dt.Rows[i]["SNAME"].ToString();
                    _SapLessMoqOrder.VBELN = dt.Rows[i]["VBELN"].ToString();
                    _SapLessMoqOrder.MATNR = dt.Rows[i]["MATNR"].ToString();
                    _SapLessMoqOrder.KWMENG = dt.Rows[i]["KWMENG"].ToString();
                    // _SapLessMoqOrder.ZMENG = dt.Rows[i]["ZMENG"].ToString();
                    _SapLessMoqOrder.WAERK = dt.Rows[i]["WAERK"].ToString();
                    _SapLessMoqOrder.NETPR = dt.Rows[i]["NETPR"].ToString();
                    _SapLessMoqOrder.KPEIN = dt.Rows[i]["KPEIN"].ToString();
                    _SapLessMoqOrder.Cost = dt.Rows[i]["Cost"].ToString();
                    _SapLessMoqOrder.WERKS = dt.Rows[i]["WERKS"].ToString();
                    if (dtMoq.AsEnumerable().Where(x => x.Field<string>("MATNR") == _SapLessMoqOrder.MATNR).Count() > 0)
                    {
                        if (tag == "MARC")
                        {
                            _SapLessMoqOrder.NORBM = dtMoq.AsEnumerable().Where(x => x.Field<string>("MATNR") == _SapLessMoqOrder.MATNR && x.Field<string>("WERKS") == _SapLessMoqOrder.WERKS).First().Field<decimal>("BSTMI").ToString();
                        }
                        else
                        {
                            _SapLessMoqOrder.MINBM = dtMoq.AsEnumerable().Where(x => x.Field<string>("MATNR") == _SapLessMoqOrder.MATNR).First().Field<decimal>("MINBM").ToString();
                            _SapLessMoqOrder.NORBM = dtMoq.AsEnumerable().Where(x => x.Field<string>("MATNR") == _SapLessMoqOrder.MATNR).First().Field<decimal>("NORBM").ToString();
                        }
                        if (Convert.ToDecimal(_SapLessMoqOrder.KWMENG) < Convert.ToDecimal(_SapLessMoqOrder.NORBM)) _SapLessMoqOrder.LessMOQ = "小於MOQ";
                    }
                    _SapLessMoqOrder.AUART = dt.Rows[i]["AUART"].ToString();
                    _SapLessMoqOrder.ERDAT = dt.Rows[i]["ERDAT"].ToString();
                    _SapLessMoqOrderList.Add(_SapLessMoqOrder);
                }
                catch (Exception ex)
                {
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:");
                }
                _Start++;
            }
            return _SapLessMoqOrderList;
        }
    }
}
