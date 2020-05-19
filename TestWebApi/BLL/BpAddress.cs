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
    public class BpAddress
    {
        public List<string> ListError = new List<string>();
        public List<WebApi.DataModel.CustomModel.SAP.BpAddress> Parse(DataTable dt  )
        {  
            List<WebApi.DataModel.CustomModel.SAP.BpAddress> _BpAddressList = new List<WebApi.DataModel.CustomModel.SAP.BpAddress>(); 
            int _Start =1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                { 
                        WebApi.DataModel.CustomModel.SAP.BpAddress _BpAddress = new WebApi.DataModel.CustomModel.SAP.BpAddress();
                        _BpAddress.KUNNR = dt.Rows[i]["KUNNR"].ToString();
                        _BpAddress.NAME1 = dt.Rows[i]["NAME1"].ToString();
                        _BpAddress.STR_SUPPL1 = dt.Rows[i]["STR_SUPPL1"].ToString() + dt.Rows[i]["STR_SUPPL2"].ToString(); 
                    _BpAddressList.Add(_BpAddress); 
                }
                catch (Exception ex)
                {
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:");
                }
                _Start++;
            } 
            return _BpAddressList;
        }
    }
}
