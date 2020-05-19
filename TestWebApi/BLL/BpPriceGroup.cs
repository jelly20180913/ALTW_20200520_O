using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace TestWebApi.BLL
{
    public class BpPriceGroup
    {
        /// <summary>
        /// BU_SORT1:chinese simple customer name
        /// BU_SORT2:english simple customer name
        /// excluded duplicate kunnr
        /// </summary>
        public List<string> ListError = new List<string>();
        public List<WebApi.DataModel.CustomModel.SAP.BpPriceGroup> Parse(DataTable dt)
        {
            List<WebApi.DataModel.CustomModel.SAP.BpPriceGroup> _BpPriceGroupList = new List<WebApi.DataModel.CustomModel.SAP.BpPriceGroup>();
            int _Start = 1;
            string _TempKUNNR = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                { 
                    WebApi.DataModel.CustomModel.SAP.BpPriceGroup _BpPriceGroup = new WebApi.DataModel.CustomModel.SAP.BpPriceGroup();
                    _BpPriceGroup.KUNNR = dt.Rows[i]["KUNNR"].ToString();
                    string _CustomerName = "";
                    if (dt.Rows[i]["BU_SORT2"].ToString() == dt.Rows[i]["BU_SORT1"].ToString()) { _CustomerName = dt.Rows[i]["BU_SORT2"].ToString(); }
                    else { _CustomerName = dt.Rows[i]["BU_SORT2"].ToString() +" "+ dt.Rows[i]["BU_SORT1"].ToString(); }
                    _BpPriceGroup.CustomerName = _CustomerName;
                    _BpPriceGroup.PriceGroup = dt.Rows[i]["PriceGroup"].ToString();
                    _BpPriceGroup.BAHNE = dt.Rows[i]["BAHNE"].ToString();
                    if (dt.Rows[i]["KUNNR"].ToString() != _TempKUNNR) _BpPriceGroupList.Add(_BpPriceGroup);
                    _TempKUNNR = dt.Rows[i]["KUNNR"].ToString();
                }
                catch (Exception ex)
                {
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:");
                }
                _Start++;
            }
            return _BpPriceGroupList;
        }
    }
}
