using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataModel.CustomModel.SAP;
using EdiEngine.Runtime;
using WebApi.Models;
using WebApi.DataModel.CustomModel.SAP.SalesOrder;
using EdiEngine.Standards.X12_004010.Maps;
using NLog;
using Newtonsoft.Json;
using System.Data;
using TestWebApi.DAO;
using TestWebApi.Common;
namespace TestWebApi.BLL.Edi
{
    public class Edi_X12_850_Parser : EdiBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Edi edi = new Edi();
        /// <summary>
        /// for mouser
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public override string Parse(EdiBatch b, Edi_Customer c, string parserFile)
        { 
            int _ItemNumber = 1;
            string _CustomerName = "";
            string _Log = "";
            SapSalesOrder _SapSalesOrder = new SapSalesOrder();
            List<SalesItem> _ItemList = new List<SalesItem>();
            List<SalesPartner> _PartnerList = new List<SalesPartner>();
            List<SalesSchedule> _ScheduleList = new List<SalesSchedule>();
            SalesHeader _Header = new SalesHeader();
            _Header.DOC_TYPE = c.DOC_TYPE;//need to discuss 
            _Header.SALES_ORG = c.SALES_ORG;//need to discuss
            _Header.DISTR_CHAN = c.DISTR_CHAN;//need to discuss
            _Header.DIVISION = c.DIVISION;//need to discuss
            try
            {
                EdiTrans _PO = b.Interchanges[0].Groups[0].Transactions[0];
                EdiSegment _BEG = (EdiSegment)_PO.Content.FirstOrDefault(l => l.Definition.Name == "BEG");
                if (_BEG != null)
                {
                    _Header.PURCH_NO_C = _BEG.Content[2].ToString();
                    _Header.PURCH_DATE = _BEG.Content[4].ToString();
                }
                _SapSalesOrder.Header = _Header;

                var listL_N1 = _PO.Content.Where(l => l.Definition.GetType() == typeof(M_850.L_N1)).Select(l => l).ToList();
                foreach (EdiLoop n1 in listL_N1)
                {
                    SalesPartner _Partner = new SalesPartner();
                    // var _N1Loop = (EdiLoop)n1;
                    EdiSegment _N1 = (EdiSegment)n1.Content.FirstOrDefault(l => l.Definition.Name == "N1");
                    if (_N1 != null)
                    {
                        if ((_N1.Content[0].ToString() == "BT") || (_N1.Content[0].ToString() == "BY")) _Partner.PARTN_ROLE = "AG";
                        else if (_N1.Content[0].ToString() == "ST") _Partner.PARTN_ROLE = "WE";

                        if (c.EdiType == 1) _Partner.PARTN_NUMB = c.SapCustomerId;//how to know what code in altw sap ,loop edi table
                        else _Partner.PARTN_NUMB = _N1.Content[3].ToString();
                        _CustomerName = _N1.Content[1].ToString();
                    }
                    EdiSegment _N3 = (EdiSegment)n1.Content.FirstOrDefault(l => l.Definition.Name == "N3");
                    if (_N3 != null)
                        _Partner.STREET = _N3.Content[0].ToString();
                    EdiSegment _N4 = (EdiSegment)n1.Content.FirstOrDefault(l => l.Definition.Name == "N4");
                    if (_N4 != null)
                    {
                        _Partner.CITY = _N4.Content[0].ToString();
                        _Partner.POSTL_CODE = _N4.Content[2].ToString();
                        _Partner.COUNTRY = _N4.Content[3].ToString();
                        _Partner.REGION = _N4.Content[1].ToString();
                    }
                    EdiSegment _PER = (EdiSegment)n1.Content.FirstOrDefault(l => l.Definition.Name == "PER");
                    if (_PER != null)
                    {
                        _Partner.NAME = _PER.Content[1].ToString();
                        _Partner.TELEPHONE = _PER.Content[3].ToString();
                    }
                    _PartnerList.Add(_Partner);
                }
                _SapSalesOrder.PartnerList = _PartnerList;
                var listL_PO1 = _PO.Content.Where(l => l.Definition.GetType() == typeof(M_850.L_PO1)).Select(l => l).ToList();
                foreach (EdiLoop po1 in listL_PO1)
                {
                    int _SchedLine = 1;
                    SalesItem _Item = new SalesItem();
                    // var _PO1Loop = (EdiLoop)po1;
                    EdiSegment _PO1 = (EdiSegment)po1.Content.FirstOrDefault(l => l.Definition.Name == "PO1");
                    if (_PO1 != null)
                    {
                        _Item.ITM_NUMBER = (_ItemNumber * 10).ToString().PadLeft(6, '0');//need use altw rule 000010
                                                                                         //mouser special logic PO111 is part number                                                              
                                                                                         // if(c.SapCustomerId== "0010000136") _Item.MATERIAL = _PO1.Content[10].ToString();
                                                                                         //else _Item.MATERIAL = _PO1.Content[8].ToString();
                        _Item.MATERIAL = _PO1.Content[Convert.ToInt32(c.PartNumberIndex)].ToString();
                        _Item.TARGET_QTY = _PO1.Content[1].ToString();
                        _Item.CUST_MAT35 = _PO1.Content[6].ToString();
                        _Item.CustomerItemNumber = _PO1.Content[0].ToString();
                        _Item.CustomerPrice = _PO1.Content[3].ToString();
                        _Item.CustomerUnit = _PO1.Content[2].ToString();
                        _Item.CustomerUnitOfPrice = _PO1.Content[4].ToString();
                    }
                    _ItemList.Add(_Item);
                    var listL_SCH = po1.Content.Where(l => l.Definition.GetType() == typeof(M_850.L_SCH)).Select(l => l).ToList();
                    foreach (EdiLoop sch in listL_SCH)
                    {

                        SalesSchedule _Schedule = new SalesSchedule();
                        // var _SCHLoop = (EdiLoop)sch;
                        EdiSegment _SCH = (EdiSegment)sch.Content.FirstOrDefault(l => l.Definition.Name == "SCH");
                        if (_SCH != null)
                        {
                            _Schedule.REQ_QTY = _SCH.Content[0].ToString();
                            _Schedule.ITM_NUMBER = _Item.ITM_NUMBER;
                            _Schedule.SCHED_LINE = _SchedLine.ToString().PadLeft(4, '0');
                            _Schedule.REQ_DATE = _SCH.Content[5].ToString();
                        }
                        _ScheduleList.Add(_Schedule);
                        _SchedLine++;
                    }
                    _ItemNumber++;
                }
                _SapSalesOrder.ItemList = _ItemList;
                _SapSalesOrder.ScheduleList = _ScheduleList;
                _SapSalesOrder.CustomerName = _CustomerName;
                _SapSalesOrder.CreateBy = "Edi";
                _SapSalesOrder.SalesEmail = c.SalesEmail;
                edi.SapSalesOrder = _SapSalesOrder;
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " setSalesOrderByEdi: Customer Id:" + c.SapCustomerId + " File :" + parserFile + "\r\n" + ex.Message;
                logger.Error(_Log);
                edi.Log= _Log; 
            }
            return _Log;
        }
        public override string  Process(  Edi_Customer c, string s, string _EdiBase)
        {
            string _Log = "";
            string _ParserFile = "";
            string _OrderNumber =FormJob.creatOrder(edi.SapSalesOrder);
            if (_OrderNumber != "")
            {
                insertZSDT046(_OrderNumber);
                if (c.Mode == true) FtpFile.RemoveFile(c.RSSBus_PortId + "/Receive", _EdiBase, s);
            }
            else
            {
                _Log = _Log + " File :" + _ParserFile + "\r\n" + "Create order fail";
                logger.Error(_Log); 
            }
            return _Log;
        }
        private void insertZSDT046(string orderNumber)
        {
            string _Date = DateTime.Now.ToString("yyyyMMdd");
            string _SqlTax = @"insert into  ZSDT046(MANDT,VBELN,SAP_PROC,SEQ,LAEDA,AENAM) values('888','" + orderNumber + "','','','" + _Date + "','Edi')  ";
            DataTable dtTax = OdbcHelper.GetDataTableText(_SqlTax, null);
        }
    }
}
