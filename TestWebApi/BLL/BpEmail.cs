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
    public class BpEmail
    {
        public List<string> ListError = new List<string>();
        public List<WebApi.DataModel.CustomModel.SAP.BpEmail> Parse(DataTable dt ,DataTable dtBpEmail)
        {  
            List<WebApi.DataModel.CustomModel.SAP.BpEmail> _BpEmailList = new List<WebApi.DataModel.CustomModel.SAP.BpEmail>(); 
            int _Start =1;
            for (int i = 0; i < dtBpEmail.Rows.Count; i++)
            {
                try
                {
                    if (dtBpEmail.Rows[i]["PARTNER1"].ToString().Substring(0, 5) != "00000")
                    {
                        WebApi.DataModel.CustomModel.SAP.BpEmail _BpEmail = new WebApi.DataModel.CustomModel.SAP.BpEmail();
                        _BpEmail.PARTNER1 = dtBpEmail.Rows[i]["PARTNER1"].ToString();
                        _BpEmail.BU_SORT1 = dtBpEmail.Rows[i]["BU_SORT1"].ToString();
                        _BpEmail.NAME_FIRST = dtBpEmail.Rows[i]["NAME_FIRST"].ToString();
                        _BpEmail.SMTP_ADDR = dtBpEmail.Rows[i]["SMTP_ADDR"].ToString();
                        _BpEmail.NAME_ORG = dt.AsEnumerable().Where(x => x.Field<string>("partner") == _BpEmail.PARTNER1).First().Field<string>("name_org1"); 
                        _BpEmailList.Add(_BpEmail);
                    }
                }
                catch (Exception ex)
                {
                    this.ListError.Add(" row : " + _Start.ToString() + " , row data has error format:" + ex.Message + "\r\n data:");
                }
                _Start++;
            } 
            return _BpEmailList;
        }
    }
}
