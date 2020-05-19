using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using WebApi.Models.CustomModel;
using Newtonsoft.Json;
using WebApi;
using WebApi.DataModel.CustomModel.SAP.SalesOrder;
using System.Data.OleDb;
using Spire.Xls;
using EdiEngine;
using EdiEngine.Runtime;
using EdiEngine.Common.Definitions;
using EdiEngine.Standards.X12_004010.Maps;
using EdiEngine.Standards.X12_004010.Segments;
using WebApi.DataModel.CustomModel.Edi;
using WebApi.Models;
using TestWebApi.Common;
using WebApi.DataModel.CustomModel.SAP;
using NLog;
using TestWebApi.DAO;
using indice.Edi.Serialization;
using indice.Edi.Utilities;
using indice.Edi;
using System.IO;
using TestWebApi.BLL.Edi;

namespace TestWebApi
{
    public partial class FormJob : Form
    {
        private static string _ConnectionString = "";
        List<Edi_Customer> _edi_Customerlist;
        private string _Token = "";
        private static Logger logger = LogManager.GetCurrentClassLogger();
        bool _Start = false;
        bool _EdiMode = false;
        private string _OdbcConnectionString = "";
        private IEdiAdapterFactory _ediAdapterFactory = new EdiAdapterFactory();
        public FormJob()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            string _Log = "";
            if (_Start)
            {
                _Start = false;
                btnStart.BackColor = Color.Purple;
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "  EDI parser program stop";
            }
            else
            {
                _Start = true;
                btnStart.BackColor = Color.Yellow;
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "  EDI parser program start up";
            }
            logger.Error(_Log);
            txtLog.Text += "\r\n" + _Log;
        }

        private void TimerCreateOrder_Tick(object sender, EventArgs e)
        {
            if (_Start)
            {
                btnCreateOrder.PerformClick();
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string _Username = ConfigurationManager.AppSettings["Account"];
            string _Password = ConfigurationManager.AppSettings["Password"];
            LoginData _LoginData = new LoginData();
            _LoginData.Username = _Username;
            _LoginData.Password = _Password;
            _LoginData.Origin = "Owner";//very important
            string json = JsonConvert.SerializeObject(_LoginData);
            string _Action = "Token";
            string _Uri = _ConnectionString + _Action;
            _Token = CallWebAPI.Login(json, _Uri);
            btnLogin.BackColor = Color.Blue;
            btnLogin.Enabled = false;
        }

        private void BtnGet_Click(object sender, EventArgs e)
        {
            string _Action = "EdiCustomer";
            string _Uri = _ConnectionString + _Action + "?mode=" + _EdiMode.ToString();
            ApiResultEntity _ApiResult = CallWebAPI.Get(_Uri, _Token);
            this._edi_Customerlist = JsonConvert.DeserializeObject<List<Edi_Customer>>(_ApiResult.Data.ToString());
        }
        /// <summary> _ediAdapterFactory
        /// read edi message to map order list
        /// </summary>
        /// <param name="parserFile"></param>
        /// <returns></returns>
        private void readEdi(string parserFile, Edi_Customer c,string s)
        {
            string _EdiBasePath = ConfigurationManager.AppSettings["EdiBase"];
            string edi = System.IO.File.ReadAllText(parserFile);
            EdiDataReader r = new EdiDataReader();
            EdiBatch b = r.FromString(edi);
            EdiTrans _Trans = b.Interchanges[0].Groups[0].Transactions[0];
            EdiBase _EdiBase = _ediAdapterFactory.CreateEdiAdapter(_Trans.Definition.ToString());
            string _Log  = _EdiBase.Parse(b, c, parserFile);
            txtLog.Text += _Log;
            _Log= _EdiBase.Process(  c,s, _EdiBasePath);
            txtLog.Text += _Log; 
        }
        private void readEdi()
        {
            string edi = txtEDIinput.Text;
            EdiDataReader r = new EdiDataReader();
            EdiBatch b = r.FromString(edi);
            MessageBox.Show(b.Interchanges.Count.ToString());
        }
        private void readEdibyEdifact()
        {
            //var grammar = EdiGrammar.NewTradacoms();
            //var interchange = default(Interchange);
            //using (var stream = new StreamReader(@"D:\Users\L180067\Desktop\EDI_C4782x_ORDERS_ATWE_20200423_sample.dat"))
            //{
            //    interchange = new EdiSerializer().Deserialize<Interchange>(stream, grammar);
            //}

            // return setSalesOrderByEdi(b, c, parserFile);

            string edi = txtEDIinput.Text;
            EdiDataReader r = new EdiDataReader();
            EdiBatch b = r.FromString(edi);
        } 
        /// <summary>
        /// call webapi to create order & return order number
        /// </summary>
        /// <param name="sapSalesOrder"></param>
        /// <returns></returns>
        public static string creatOrder(SapSalesOrder sapSalesOrder)
        {
            string json = JsonConvert.SerializeObject(sapSalesOrder);
            string _Action = "BapiOrder";
            string _Uri = _ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebAPI.Post(json, _Uri);
            return _ApiResult.Data.ToString();
        } 
        private void updateZSDT046(string orderNumber)
        {
            string _Date = DateTime.Now.ToString("yyyyMMdd");
            string _SqlTax = @"update  ZSDT046 set  SAP_PROC='S' ,LAEDA='" + _Date + "',AENAM='Edi'  where vbeln=" + orderNumber;
            DataTable dtTax = OdbcHelper.GetDataTableText(_SqlTax, null);
        }
        private List<Edi_SalesHeader> getSalesHeaderList()
        {
            string _Action = "EdiSalesHeader";
            string _Uri = _ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebAPI.Get(_Uri, _Token);
            return JsonConvert.DeserializeObject<List<Edi_SalesHeader>>(_ApiResult.Data.ToString());
        }
        /// <summary>
        /// test sap_proc='S'
        /// </summary>
        /// <returns></returns>
        private List<EdiStatus> GetSapEdiStatusList()
        {
            string _Sql = @"select   
                               mandt,vbeln,sap_proc,seq,laeda,aenam
                               from sapabap1.zsdt046  
                                where    sap_proc = ''    ";
            DataTable dt = OdbcHelper.GetDataTableText(_Sql, null);
            IList<EdiStatus> _EdiStatusIList = dt.ToList<EdiStatus>();
            List<EdiStatus> _EdiStatusList = _EdiStatusIList as List<EdiStatus>;
            return _EdiStatusList;
        }
        /// <summary>
        /// if you join VBEP ,you get repeat data
        /// test    -- and txt04='S'
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        private List<SOrder> GetSapSalesHeaderList(string orderNumber)
        {
            string _Sql = @"select   
                               case a.waerk  
                               when 'TWD' then    b.netpr/b.kpein*100 
                               when 'JPY' then    b.netpr/b.kpein*100
                               else  b.netpr/b.kpein
                               end as price,  
                                a.vbeln,b.KWMENG,b.ZMENG,b.NETPR, a.kunnr,a.erdat,a.waerk,a.spart, 
                               b.matnr, 
                               b.werks,
                               b.ABGRU, 
                               h.EDATU,
                               h.LIFSP,
                               f.TXT04,
                               b.POSNR,
                               b.LFSTA
                               from sapabap1.vbak a
                               join sapabap1.vbap b on a.vbeln=b.vbeln
                               join sapabap1.kna1 e on a.kunnr=e.kunnr
                               join sapabap1.JEST g on g.objnr=a.objnr
                               join sapabap1.TJ30T f on f.STSMA ='Z0000001' and f.estat=g.stat 
                               join sapabap1.VBEP h on a.vbeln=h.vbeln and b.posnr=h.posnr ";
            if (_EdiMode)
                _Sql += " and txt04='S' ";
            _Sql += "  where       a.vbeln='" + orderNumber + "'" +
                               " order by a.vbeln  ,b.posnr ";
            DataTable dt = OdbcHelper.GetDataTableText(_Sql, null);
            IList<SOrder> _SOrderIList = dt.ToList<SOrder>();
            List<SOrder> _SOrderList = _SOrderIList as List<SOrder>;
            return _SOrderList;
        }
        private List<Edi_SalesItem> getSalesItemList(string orderNumber)
        {
            string _Action = "EdiSalesItem";
            string _Uri = _ConnectionString + _Action + "?orderNumber=" + orderNumber;
            ApiResultEntity _ApiResult = CallWebAPI.Get(_Uri, _Token);
            return JsonConvert.DeserializeObject<List<Edi_SalesItem>>(_ApiResult.Data.ToString());
        }
        private List<Edi_SalesSchedule> getSalesScheduleList(string orderNumber)
        {
            string _Action = "EdiSalesSchedule";
            string _Uri = _ConnectionString + _Action + "?orderNumber=" + orderNumber;
            ApiResultEntity _ApiResult = CallWebAPI.Get(_Uri, _Token);
            return JsonConvert.DeserializeObject<List<Edi_SalesSchedule>>(_ApiResult.Data.ToString());
        }
        private string insertEdi_855(Edi_SalesOrder_855 sapSalesOrder)
        {
            string json = JsonConvert.SerializeObject(sapSalesOrder);
            string _Action = "EdiSalesOrder_855";
            string _Uri = _ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebAPI.Post(json, _Uri);
            return _ApiResult.Data.ToString();
        }
        /// <summary>
        /// get Sap ZSDT046 table SAP_PROC = '' 
        /// get status = 'S'  sap order
        /// 855 message:according to 850 table & sap order data 
        /// insert 855 data to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerOrderComfirm_Tick(object sender, EventArgs e)
        {
            if (_Start)
            {
                btnOrderComfirm.PerformClick();
            }
        }

        private void FormJob_Load(object sender, EventArgs e)
        {
            string _Version = "___" + ConfigurationManager.AppSettings["Version"];
            var _Ip = WebApi.Common.Function.GetIpAddresses();
            if (_Ip[_Ip.Count() - 1].ToString() == ConfigurationManager.AppSettings["ServerIp"])
            {
                _ConnectionString = ConfigurationManager.AppSettings["ApiServer"];
                _EdiMode = Convert.ToBoolean(ConfigurationManager.AppSettings["EdiMode"]);
                this.Text = "EDI解析程式_正式機";
            }
            else
            {
                _ConnectionString = ConfigurationManager.AppSettings["ApiServerTest"];
                _EdiMode = Convert.ToBoolean(ConfigurationManager.AppSettings["EdiModeTest"]);
                this.Text = "EDI解析程式_測試機";
            }
            _OdbcConnectionString = ConfigurationManager.AppSettings["OdbcConnection"];
            txtJob.Text = "60";
            txtInformation.Text = "IP : " + _Ip[_Ip.Count() - 1];
            txtInformation.Text += "\r\nApi Server : " + _ConnectionString;
            txtInformation.Text += "\r\nEdi Mode : " + _EdiMode;
            this.Text += _Version;
        }
        /// <summary>
        /// test edi job business
        /// 1. get edi customer table prd/test 
        /// 2. download edi message file &copy to ./Edi
        /// 3. parser edi message to set sap order 
        /// 4. create order 
        /// 4.1. insert to edi order table
        /// 4.2. success/fail status
        /// 5. success:delete source file
        /// 6. email to key user
        /// 7. response 855 message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTest_Click(object sender, EventArgs e)
        {
            string _Log = "";
            string _CustomerId = "";
            string _ParserFile = "";
            try
            {
                btnLogin.PerformClick();
                btnGet.PerformClick();
                string _EdiBase = ConfigurationManager.AppSettings["EdiBase"];
                foreach (Edi_Customer c in _edi_Customerlist)
                {
                    _CustomerId = c.SapCustomerId;
                    _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " CreateOrder: Customer Id:" + _CustomerId;
                    if (c.Mode == _EdiMode)//PRD
                    {
                        List<string> _FileList = FtpFile.GetList(c.RSSBus_PortId + "/Receive", _EdiBase);
                        if (FtpFile.ListError.Count == 0)
                        {
                            foreach (string s in _FileList)
                            {
                                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " CreateOrder: Customer Id:" + _CustomerId;
                                _ParserFile = FtpFile.Download("Edi", c.RSSBus_PortId + "/Receive", _EdiBase, s);
                                if (FtpFile.ListError.Count == 0)
                                {
                                   readEdi(_ParserFile, c,s);  
                                }
                                else
                                {
                                    _Log = _Log + " File :" + _ParserFile + "\r\n" + FtpFile.ListError[0];
                                    logger.Error(_Log);
                                    txtLog.Text += _Log;
                                }
                            }
                        }
                        else
                        {
                            _Log = _Log + "\r\n" + FtpFile.ListError[0];
                            logger.Error(_Log);
                            txtLog.Text += _Log;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " CreateOrder: Customer Id:" + _CustomerId + " File :" + _ParserFile + "\r\n" + ex.Message;
                logger.Error(_Log);
                txtLog.Text += _Log;
            }
        }
        /// <summary>
        /// get Sap ZSDT046 table SAP_PROC = '' 
        /// get status = 'S'  sap order
        /// 855 message:according to 850 table & sap order data 
        /// insert 855 data to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOrderComfirm_Click(object sender, EventArgs e)
        {
            string _Log = "";
            string _OrderNumber = "";
            try
            {
                string _EdiBase = ConfigurationManager.AppSettings["EdiBase"];
                btnLogin.PerformClick();
                btnGet.PerformClick();
                List<Edi_SalesHeader> _SalesHeaderList = getSalesHeaderList();
                Edi_SalesOrder_855 _Edi_SalesOrder_855 = new Edi_SalesOrder_855();
                TestWebApi.BLL.Edi.Edi_SalesHeader_855 _855 = new TestWebApi.BLL.Edi.Edi_SalesHeader_855();
                List<EdiStatus> _EdiStatusList = GetSapEdiStatusList();
                foreach (EdiStatus s in _EdiStatusList)
                {
                    _OrderNumber = s.VBELN;
                    _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " CreateOrderComfirm: Order Number:" + _OrderNumber;
                    List<SOrder> _SOrderList = GetSapSalesHeaderList(s.VBELN);
                    List<Edi_SalesItem> _SalesItemList = getSalesItemList(s.VBELN);
                    List<Edi_SalesSchedule> _SalesScheduleList = getSalesScheduleList(s.VBELN);
                    if (_SOrderList.Count > 0)
                    {
                        Edi_SalesHeader _Edi_SalesHeader = _SalesHeaderList.Where(x => x.OrderNumber == s.VBELN).First();
                        if (_edi_Customerlist.Where(x => x.SapCustomerId == _SOrderList.First().KUNNR).ToList().Count > 0)
                        {
                            Edi_Customer c = _edi_Customerlist.Where(x => x.SapCustomerId == _SOrderList.First().KUNNR).First();
                            _Edi_SalesOrder_855 = _855.LineItemStatus(_Edi_SalesHeader, _SOrderList, _SalesItemList, _SalesScheduleList);
                            if (_855.ListError.Count == 0)
                            {
                                string _Mode = c.Mode == true ? "P" : "T";
                                if (c.Mode == _EdiMode)//PRD 
                                {
                                    string _855Msg = _855.X12_855(_Edi_SalesOrder_855, _Mode, c.ReceiverId);
                                    insertEdi_855(_Edi_SalesOrder_855);
                                    string _FileName = "855_" + _Edi_SalesOrder_855.Header.PURCH_NO_C + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString("N") + ".edi";
                                    string _EdiCustomerSend = c.RSSBus_PortId + "/Send";
                                    //write edi file 
                                    FtpFile.CopyTo("Edi", _EdiCustomerSend, _FileName, _855Msg);
                                    //upload to RSSBus send directory
                                    FtpFile.Upload("Edi", _EdiCustomerSend, _EdiBase, _FileName);
                                    if (c.Mode == true) updateZSDT046(s.VBELN);
                                }
                            }
                            else
                            {
                                _Log = _Log + "\r\n" + _855.ListError[0];
                                logger.Error(_Log);
                                txtLog.Text += _Log;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " OrderComfirm:Order Number:" + _OrderNumber + "\r\n" + ex.Message;
                logger.Error(_Log);
                txtLog.Text += _Log;
            }
        }

        private void BtnSet_Click(object sender, EventArgs e)
        {
            int _JobInterval = txtJob.Text != "" ? Convert.ToInt32(txtJob.Text) * 60 * 1000 : 5000;
            timerCreateOrder.Interval = _JobInterval;
            timerOrderComfirm.Interval = _JobInterval;
            string _Job = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "  EDI parser program job set " + txtJob.Text + " minute";
            txtLog.Text += _Job;
        }

        private void BtnEdifact_Click(object sender, EventArgs e)
        {

            string _Log = "";
            string _CustomerId = "";
            string _ParserFile = "";
            try
            {
                readEdibyEdifact();
                //readEdi();
                MessageBox.Show("OK");
                //btnLogin.PerformClick();
                //btnGet.PerformClick();
                //string _EdiBase = ConfigurationManager.AppSettings["EdiBase"];
                //foreach (Edi_Customer c in _edi_Customerlist)
                //{
                // _CustomerId = c.SapCustomerId;
                // if (c.Mode == _EdiMode)//PRD
                //{
                // List<string> _FileList = FtpFile.GetList(c.RSSBus_PortId + "/Receive", _EdiBase);
                //if (FtpFile.ListError.Count > 0) throw new Exception(FtpFile.ListError[0]);
                // foreach (string s in _FileList)
                // {
                //   _ParserFile = FtpFile.Download("Edi", c.RSSBus_PortId + "/Receive", _EdiBase, s);
                //  if (FtpFile.ListError.Count > 0) throw new Exception(FtpFile.ListError[0]);
                // SapSalesOrder _SapSalesOrder = readEdi(_ParserFile, c);
                readEdibyEdifact();
                //_SapSalesOrder.SalesEmail = c.SalesEmail;
                //string _OrderNumber = creatOrder(_SapSalesOrder);
                //if (_OrderNumber == "") throw new Exception("CreateOrder Fail");
                //insertZSDT046(_OrderNumber);
                // if (c.Mode == true) FtpFile.RemoveFile(c.RSSBus_PortId + "/Receive", _EdiBase, s);
                // }
                // }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //_Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " CreateOrder: Customer Id:" + _CustomerId + " File :" + _ParserFile + "\r\n" + ex.Message;
                //logger.Error(_Log);
                //txtLog.Text += _Log;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        } 
    }
}
