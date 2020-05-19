using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Data.Odbc;
using TestWebApi.DAO;
using TestWebApi.BLL;
using WebApi.DataModel.CustomModel.SAP;
using System.Diagnostics;
using TestWebApi.Common;
using WebApi.Models;
using TestWebApi.Model;
using System.Configuration;
using WebApi.Models.CustomModel;
using WebApi.Models.JWT;
using WebApi;
using System.Drawing;
using WebApi.DataModel.CustomModel.SAP.SalesOrder;
using System.Data.OleDb;
using Spire.Xls;
using EdiEngine;
using EdiEngine.Runtime;
using Newtonsoft.Json;
using EdiEngine.Common.Definitions;
using EdiEngine.Standards.X12_004010.Maps;
using EdiEngine.Standards.X12_004010.Segments;
using WebApi.DataModel.CustomModel.Edi; 
namespace TestWebApi
{
    public partial class Form1 : Form
    {
        private string _ConnectionString = "";
        private string _OdbcConnectionString = "";

        string filepath = $@"./Success/{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
        string filepathNoExt = $@"./Success/{DateTime.Now.ToString("yyyyMMddHHmmss")}";
        List<Edi_Customer> _edi_Customerlist;
        private string _Token = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void TestWebApi_Click(object sender, EventArgs e)
        {
            //string _Sql = "";
            //string _Competitor = "";
            //string _Target_Price = "";
            //string _Possibility = "";
            //string _Project_Type = "";
            //string _Annual_Qty = "";
            //string _Project_Size = ""; 
            //System.Data.OleDb.OleDbConnection _Conn = new System.Data.OleDb.OleDbConnection(@"Provider=sqloledb; Data Source=10.0.0.213; Initial Catalog=MIDDB_DEV; User Id =altwmis; Password=altwmis!@#");
            //_Conn.Open();
            //_Sql += "UPDATE ZPP_PLM01 SET Competitor = ?,Target_Price = ?,Annual_Qty = ?,  Project_Size = ? ,Possibility = ?,IsNewProduct = ? where MATNR = ?";
            //OleDbCommand cmd = new OleDbCommand(_Sql, _Conn);
            //cmd.Parameters.AddWithValue("@Competitor", _Competitor); 
            //cmd.Parameters.AddWithValue("@Target_Price",12); 
            //cmd.Parameters.AddWithValue("@Annual_Qty", 1); 
            //cmd.Parameters.AddWithValue("@Project_Size",100); 
            //cmd.Parameters.AddWithValue("@Possibility", "20%"); 
            //cmd.Parameters.AddWithValue("@IsNewProduct", true);
            //cmd.Parameters.AddWithValue("@MATNR", "AAC2-DDL07ZZ0044");
            // cmd.Parameters["@IsNewProduct"].Value = true;
            //  Item insResult = inn.applyMethod("ltw_sd01_db_operation","<sql>" + l_sql + "</sql>"); 
            // cmd.ExecuteNonQuery();
            //using (WebClient webClient = new WebClient())
            //// 從 url 讀取資訊至 stream
            //using (Stream stream = webClient.OpenRead(_ConnectionString + "index/1"))
            //// 使用 StreamReader 讀取 stream 內的字元
            //using (StreamReader reader = new StreamReader(stream))
            //{
            //    // 將 StreamReader 所讀到的字元轉為 string
            //    string request = reader.ReadToEnd();
            //    MessageBox.Show(request);
            //}
        }
        /// <summary>
        /// this method can connection to hana studio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                // HanaConnection conn = new HanaConnection();
                // conn.ConnectionString = "Server=sapprddb1:30215;UserID=SYSTEM;Password=Altw6888"; 
                //conn.Open();
                string sql = "select a.kunnr,b.name1,b.BAHNE from sapabap1.knvv a join sapabap1.kna1 b on a.kunnr = b.kunnr where a.KUNNR like '001%'";
                DataSet ds = new DataSet();
                OdbcCommand command = new OdbcCommand(sql);  //command  对象
                String connstring = _OdbcConnectionString; //ODBC连接字符串
                using (OdbcConnection connection = new OdbcConnection(connstring))  //创建connection连接对象
                {
                    command.Connection = connection;
                    connection.Open();  //打开链接
                    OdbcDataAdapter adapter = new OdbcDataAdapter(command);  //实例化dataadapter
                    adapter.Fill(ds);  //填充查询结果
                                       //  return ds;
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            SapMiddleData _SapMiddleData = initSapMiddleData();
            sw.Stop();
            lbSapPrice.Text = sw.ElapsedMilliseconds.ToString() + "毫秒";
            //  lbCount.Text = _SapMiddleData.PriceList.Count().ToString()+"筆";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //just get new sap price
            txtDate.Text = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            var _Ip = WebApi.Common.Function.GetIpAddresses();
            if (_Ip[_Ip.Count() - 1].ToString() == ConfigurationManager.AppSettings["ServerIp"])
                _ConnectionString = ConfigurationManager.AppSettings["ApiServer"];
            else
                _ConnectionString = ConfigurationManager.AppSettings["ApiServerTest"];
            _OdbcConnectionString = ConfigurationManager.AppSettings["OdbcConnection"];
        }

        private void ChkAll_Click(object sender, EventArgs e)
        {
            txtDate.Text = "";
        }
        /// <summary>
        /// get sap A501 & A306 ladder price 
        /// </summary>
        /// <returns></returns>
        private SapMiddleData GetSapPriceList()
        {
            //抓全部資料，由於不是參數化查詢，第二個參數傳null
            string _SqlA501 = @"select a.KUNNR ,d.name1,a.ZZENDCUST,e.name1  as zname1,a.MATNR  ,b.KONWA,c.KSTBM,c.KBETR,c.KLFN1
            ,a.KNUMH ,b.KPEIN ,a.DATAB ,a.kschl from  sapabap1.A501  a join  sapabap1.KONP  b on a.KNUMH=b.KNUMH join  sapabap1.KONM  c on  a.KNUMH=c.KNUMH join sapabap1.kna1 d on a.kunnr=d.kunnr join sapabap1.kna1 e on a.ZZENDCUST=e.kunnr 
             where DATBI='99991231'    ";
            if (txtDate.Text != "") _SqlA501 += " and DATAB > " + txtDate.Text;
            _SqlA501 += " order by a.knumh,a.KUNNR,a.ZZENDCUST,c.KLFN1,a.matnr";
            DataTable dtA501 = OdbcHelper.GetDataTableText(_SqlA501, null);
            string _SqlA306 = @"select a.KUNNR ,d.name1,  a.MATNR,   b.KONWA ,c.KSTBM,c.KBETR,c.KLFN1  ,a.KNUMH  ,b.KPEIN,a.DATAB
         ,a.kschl  from  sapabap1.A305  a join  sapabap1.KONP  b on a.KNUMH=b.KNUMH join  sapabap1.KONM  c on a.KNUMH=c.KNUMH 
         join sapabap1.kna1 d on a.kunnr=d.kunnr where DATBI='99991231'  ";
            if (txtDate.Text != "") _SqlA306 += " and DATAB > " + txtDate.Text;
            _SqlA306 += " order by a.knumh,a.KUNNR,a.matnr,c.KLFN1";
            DataTable dtA306 = OdbcHelper.GetDataTableText(_SqlA306, null);
            dtA501.Merge(dtA306);
            SapPrice _SapPrice = new SapPrice();
            SapMiddleData _SapMiddleData = _SapPrice.Parse(dtA501);
            return _SapMiddleData;
        }
        private SapMiddleData ProcessChinaPrice(SapMiddleData sapMiddleData, IList<QuotationTax> quotationTaxList)
        {
            bool _Has = false;
            int _Tax = 0;
            bool _AddNoTaxPrice = true;
            List<SAP_PriceList> _Sap_PriceList_CNY = sapMiddleData.PriceList.Where(x => x.Currency == "CNY").ToList();
            List<SAP_PriceList> _Sap_PriceList_PR00 = new List<SAP_PriceList>();
            SapMiddleData _SapMiddleData = new SapMiddleData();
            foreach (SAP_PriceList s in _Sap_PriceList_CNY)
            {
                _AddNoTaxPrice = s.TaxType == "PR01" ? true : false;
                //if no PR00 price
                _Has = IsHasNoTax(_Sap_PriceList_CNY, s);
                if (!_Has)
                {
                    _Tax = _AddNoTaxPrice ? GetTax(quotationTaxList, s) : 13;
                    SAP_PriceList _Sap_Price = new SAP_PriceList();
                    _Sap_Price.TaxRate = _Tax;
                    _Tax += 100;
                    _Sap_Price.CustomerId = s.CustomerId;
                    _Sap_Price.CustomerName = s.CustomerName;
                    _Sap_Price.EndCustomerId = s.EndCustomerId;
                    _Sap_Price.EndCustomerName = s.EndCustomerName;
                    _Sap_Price.PartNumber = s.PartNumber;
                    _Sap_Price.Currency = s.Currency;
                    _Sap_Price.MOQ1 = s.MOQ1;
                    if (s.MOQ1 != null)
                    {
                        _Sap_Price.Price1 = _AddNoTaxPrice ? s.Price1 * 100 / _Tax : s.Price1 * _Tax / 100;
                        _Sap_Price.Price1 = Convert.ToDecimal(System.Math.Round(Convert.ToDouble(_Sap_Price.Price1), 2));
                    }
                    _Sap_Price.MOQ2 = s.MOQ2;
                    if (s.MOQ2 != null)
                    {
                        _Sap_Price.Price2 = _AddNoTaxPrice ? s.Price2 * 100 / _Tax : s.Price2 * _Tax / 100;
                        _Sap_Price.Price2 = Convert.ToDecimal(System.Math.Round(Convert.ToDouble(_Sap_Price.Price2), 2));
                    }
                    _Sap_Price.MOQ3 = s.MOQ3;
                    if (s.MOQ3 != null)
                    {
                        _Sap_Price.Price3 = _AddNoTaxPrice ? s.Price3 * 100 / _Tax : s.Price3 * _Tax / 100;
                        _Sap_Price.Price3 = Convert.ToDecimal(System.Math.Round(Convert.ToDouble(_Sap_Price.Price3), 2));
                    }
                    _Sap_Price.MOQ4 = s.MOQ4;
                    if (s.MOQ4 != null)
                    {
                        _Sap_Price.Price4 = _AddNoTaxPrice ? s.Price4 * 100 / _Tax : s.Price4 * _Tax / 100;
                        _Sap_Price.Price4 = Convert.ToDecimal(System.Math.Round(Convert.ToDouble(_Sap_Price.Price4), 2));
                    }
                    _Sap_Price.MOQ5 = s.MOQ5;
                    if (s.MOQ5 != null)
                    {
                        _Sap_Price.Price5 = _AddNoTaxPrice ? s.Price5 * 100 / _Tax : s.Price5 * _Tax / 100;
                        _Sap_Price.Price5 = Convert.ToDecimal(System.Math.Round(Convert.ToDouble(_Sap_Price.Price5), 2));
                    }
                    _Sap_Price.MOQ6 = s.MOQ6;
                    if (s.MOQ6 != null)
                    {
                        _Sap_Price.Price6 = _AddNoTaxPrice ? s.Price6 * 100 / _Tax : s.Price6 * _Tax / 100;
                        _Sap_Price.Price6 = Convert.ToDecimal(System.Math.Round(Convert.ToDouble(_Sap_Price.Price6), 2));
                    }
                    _Sap_Price.MOQ7 = s.MOQ7;
                    if (s.MOQ7 != null)
                    {
                        _Sap_Price.Price7 = _AddNoTaxPrice ? s.Price7 * 100 / _Tax : s.Price7 * _Tax / 100;
                        _Sap_Price.Price7 = Convert.ToDecimal(System.Math.Round(Convert.ToDouble(_Sap_Price.Price7), 2));
                    }
                    _Sap_Price.MOQ8 = s.MOQ8;
                    if (s.MOQ8 != null)
                    {
                        _Sap_Price.Price8 = _AddNoTaxPrice ? s.Price8 * 100 / _Tax : s.Price8 * _Tax / 100;
                        _Sap_Price.Price8 = Convert.ToDecimal(System.Math.Round(Convert.ToDouble(_Sap_Price.Price8), 2));
                    }
                    _Sap_Price.MOQ9 = s.MOQ9;
                    if (s.MOQ9 != null)
                    {
                        _Sap_Price.Price9 = _AddNoTaxPrice ? s.Price9 * 100 / _Tax : s.Price9 * _Tax / 100;
                        _Sap_Price.Price9 = Convert.ToDecimal(System.Math.Round(Convert.ToDouble(_Sap_Price.Price9), 2));
                    }
                    _Sap_Price.MOQ10 = s.MOQ10;
                    if (s.MOQ10 != null)
                    {
                        _Sap_Price.Price10 = _AddNoTaxPrice ? s.Price10 * 100 / _Tax : s.Price10 * _Tax / 100;
                        _Sap_Price.Price10 = Convert.ToDecimal(System.Math.Round(Convert.ToDouble(_Sap_Price.Price10), 2));
                    }
                    _Sap_Price.Date = DateTime.Now.ToString("yyyyMMdd");
                    _Sap_Price.Rate = s.Rate;
                    _Sap_Price.Per = s.Per;
                    _Sap_Price.TaxType = _AddNoTaxPrice ? "PR00" : "PR01";
                    _Sap_Price.TaxRate = _Tax - 100;
                    _Sap_Price.EffectiveDate = s.EffectiveDate;
                    _Sap_Price.InsertSapPrice = false;
                    _Sap_PriceList_PR00.Add(_Sap_Price);
                }
            }
            _SapMiddleData.PriceList = _Sap_PriceList_PR00;
            //_SapMiddleData.PriceList = _Sap_PriceList_PR00.Where(x => x.TaxRate == 13).ToList();
            return _SapMiddleData;
        }
        private IList<QuotationTax> GetQuotationTaxList()
        {
            string _SqlTax = @"select a.kunnr,b.matnr  ,a.KUNNR1,a.LAEDA ,
            case a.ztax    when '1' then '0' when '2' then '5' when '3' then '16' when '4' then '13' when '' then '' end as tax
            from sapabap1.zsdt006 a join sapabap1.zsdt007 b on  a.zqtno=b.zqtno where a.ZSTATUS='F' and a.ztax <>1  ";
            if (txtDate.Text != "") _SqlTax += " and a.LAEDA > " + txtDate.Text;
            DataTable dtTax = OdbcHelper.GetDataTableText(_SqlTax, null);
            IList<QuotationTax> _TaxList = dtTax.ToList<QuotationTax>();
            return _TaxList;
        }
        /// <summary>
        /// if current sap price list has PR01 price,
        /// then has PR00  price ,return true
        /// then no PR00 price , return false
        /// </summary>
        /// <param name="sap_PriceList_CNY">the price list contans CNY currency </param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool IsHasNoTax(List<SAP_PriceList> sap_PriceList_CNY, SAP_PriceList s)
        {
            bool _Has = false;
            SAP_PriceList _Sap_PriceList = new SAP_PriceList();
            string _TaxCondition = s.TaxType == "PR01" ? "PR00" : "PR01";
            try
            {
                if (s.EndCustomerId != "")
                {
                    _Sap_PriceList = sap_PriceList_CNY.Where(x => x.CustomerId == s.CustomerId && x.EndCustomerId == s.EndCustomerId && x.PartNumber == s.PartNumber && x.TaxType == _TaxCondition).FirstOrDefault();
                }
                else
                {
                    _Sap_PriceList = sap_PriceList_CNY.Where(x => x.CustomerId == s.CustomerId && x.PartNumber == s.PartNumber && x.TaxType == _TaxCondition).FirstOrDefault();
                }
                _Has = _Sap_PriceList != null ? true : false;
            }
            catch (Exception ex)
            {

            }
            return _Has;
        }
        /// <summary>
        /// get sap zsdt006 tax
        /// </summary>
        /// <param name="quotationTaxList"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private int GetTax(IList<QuotationTax> quotationTaxList, SAP_PriceList s)
        {
            int _Tax = 0;
            QuotationTax _QuotationTax = new QuotationTax();
            try
            {
                if (s.EndCustomerId != "")
                {
                    _QuotationTax = quotationTaxList.Where(x => x.LAEDA == s.EffectiveDate && x.KUNNR == s.CustomerId && x.KUNNR1 == s.EndCustomerId && x.MATNR == s.PartNumber).FirstOrDefault();
                }
                else
                {
                    _QuotationTax = quotationTaxList.Where(x => x.LAEDA == s.EffectiveDate && x.KUNNR == s.CustomerId && x.MATNR == s.PartNumber).FirstOrDefault();
                }
                //the price tax from tip top system is 16% 
                _Tax = _QuotationTax != null ? Convert.ToInt32(_QuotationTax.TAX) : 16;
            }
            catch (Exception ex)
            {

            }

            return _Tax;
        }

        private void BtnPostSapPrice_Click(object sender, EventArgs e)
        {
            SapMiddleData _SapMiddleData = initSapMiddleData();
            using (WebClient webClient = new WebClient())
            {
                // 從 url 讀取資訊至 stream
                //using (Stream stream = webClient.OpenRead("http://localhost:52006/api/SAP_PriceList/1"))
                //using (Stream stream = webClient.OpenRead("http://localhost:52006/api/Token/1"))
                // 指定 WebClient 編碼
                webClient.Encoding = Encoding.UTF8;
                // 指定 WebClient 的 Content-Type header
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                // 指定 WebClient 的 authorization header
                //webClient.Headers.Add("authorization", "token {apitoken}");
                // 將 data 轉為 json
                string json = JsonConvert.SerializeObject(_SapMiddleData);
                // 執行 post 動作
                var result = webClient.UploadString(_ConnectionString + "SAP_PriceList", json);
            }
        }
        private void BtnExcel_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            txtDate.Text = "";
            SapMiddleData _SapMiddleData = initSapMiddleData();
            //建立 xlxs 轉換物件
            XSLXHelper helper = new XSLXHelper();
            List<SapPriceListExcel> _SapPriceListExcelList = SetSapPriceListExcel(_SapMiddleData.PriceList);
            var xlsx = helper.Export(_SapPriceListExcelList);
            xlsx.SaveAs(filepathNoExt + "_SapPrice" + ".xlsx");
            sw.Stop();
            MessageBox.Show(sw.ElapsedMilliseconds.ToString() + "毫秒");
        }
        private SapMiddleData initSapMiddleData()
        {
            SapMiddleData _SapMiddleData = GetSapPriceList();
            IList<QuotationTax> _QuotationTaxList = GetQuotationTaxList();
            //extra process CNY PR00 & PR01 price 
            SapMiddleData _SapMiddleData_CNY = ProcessChinaPrice(_SapMiddleData, _QuotationTaxList);
            _SapMiddleData.PriceList.AddRange(_SapMiddleData_CNY.PriceList);
            return _SapMiddleData;
        }
        private List<SapPriceListExcel> SetSapPriceListExcel(List<SAP_PriceList> sAP_PriceList)
        {
            List<SapPriceListExcel> _SapPriceListExcelList = new List<SapPriceListExcel>();
            foreach (SAP_PriceList s in sAP_PriceList.Where(x => x.TaxType == "PR00"))
            {
                SapPriceListExcel _Sap_Price = new SapPriceListExcel();
                _Sap_Price.CustomerId = s.CustomerId;
                _Sap_Price.CustomerName = s.CustomerName;
                _Sap_Price.EndCustomerId = s.EndCustomerId;
                _Sap_Price.EndCustomerName = s.EndCustomerName;
                _Sap_Price.PartNumber = s.PartNumber;
                _Sap_Price.Currency = s.Currency;
                _Sap_Price.MOQ1 = s.MOQ1;
                _Sap_Price.Price1 = s.Price1;
                _Sap_Price.Tax1 = s.Currency != "CNY" ? s.TaxRate : getTax(s.TaxRate, s.MOQ1);
                _Sap_Price.MOQ2 = s.MOQ2;
                _Sap_Price.Price2 = s.Price2;
                _Sap_Price.Tax2 = s.Currency != "CNY" ? s.TaxRate : getTax(s.TaxRate, s.MOQ2);
                _Sap_Price.MOQ3 = s.MOQ3;
                _Sap_Price.Price3 = s.Price3;
                _Sap_Price.Tax3 = s.Currency != "CNY" ? s.TaxRate : getTax(s.TaxRate, s.MOQ3);
                _Sap_Price.MOQ4 = s.MOQ4;
                _Sap_Price.Price4 = s.Price4;
                _Sap_Price.Tax4 = s.Currency != "CNY" ? s.TaxRate : getTax(s.TaxRate, s.MOQ4);
                _Sap_Price.MOQ5 = s.MOQ5;
                _Sap_Price.Price5 = s.Price5;
                _Sap_Price.Tax5 = s.Currency != "CNY" ? s.TaxRate : getTax(s.TaxRate, s.MOQ5);
                _Sap_Price.MOQ6 = s.MOQ6;
                _Sap_Price.Price6 = s.Price6;
                _Sap_Price.Tax6 = s.Currency != "CNY" ? s.TaxRate : getTax(s.TaxRate, s.MOQ6);
                _Sap_Price.MOQ7 = s.MOQ7;
                _Sap_Price.Price7 = s.Price7;
                _Sap_Price.Tax7 = s.Currency != "CNY" ? s.TaxRate : getTax(s.TaxRate, s.MOQ7);
                _Sap_Price.MOQ8 = s.MOQ8;
                _Sap_Price.Price8 = s.Price8;
                _Sap_Price.Tax8 = s.Currency != "CNY" ? s.TaxRate : getTax(s.TaxRate, s.MOQ8);
                _Sap_Price.MOQ9 = s.MOQ9;
                _Sap_Price.Price9 = s.Price9;
                _Sap_Price.Tax9 = s.Currency != "CNY" ? s.TaxRate : getTax(s.TaxRate, s.MOQ9);
                _Sap_Price.MOQ10 = s.MOQ10;
                _Sap_Price.Price10 = s.Price10;
                _Sap_Price.Tax10 = s.Currency != "CNY" ? s.TaxRate : getTax(s.TaxRate, s.MOQ10);
                //_Sap_Price.TaxType = s.TaxType;
                _SapPriceListExcelList.Add(_Sap_Price);
            }
            return _SapPriceListExcelList;
        }
        private int? getTax(int? taxRate, int? moq)
        {
            int? _Tax = null;
            if (moq != null)
            {
                if (taxRate == null) _Tax = 13;
                else _Tax = taxRate;
            }
            return _Tax;
        }
        /// <summary>
        /// test ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button3_Click(object sender, EventArgs e)
        {
            //string _SqlTax = @"insert into sapabap1.A501(KUNNR,MATNR) values('0010000156','aa')  ";
            //DataTable dtTax = OdbcHelper.GetDataTableText(_SqlTax, null);
            //string _SqlTax = @"insert into  ZSDT045(ZMAPID,KUNNR,ENDKUNNR,SPART,NEWSPART,PERNR) //values('6','0010000156','','2','3','4')  ";
            //DataTable dtTax = OdbcHelper.GetDataTableText(_SqlTax, null);
           // updateZSDT046();
        }

        private void BtnOrder_Click(object sender, EventArgs e)
        {
            SapSalesOrder _SapSalesOrder = setSalesOrder();
            string json = JsonConvert.SerializeObject(_SapSalesOrder);
            string _Action = "Index";
            string _Uri = _ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebAPI.Post(json, _Uri);
        }
        /// <summary>
        /// when you want to call sap webapi you need get token first
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        private SapSalesOrder setSalesOrder()
        {
            SapSalesOrder _SapSalesOrder = new SapSalesOrder();
            List<SalesItem> _ItemList = new List<SalesItem>();
            List<SalesPartner> _PartnerList = new List<SalesPartner>();
            List<SalesSchedule> _ScheduleList = new List<SalesSchedule>();
            SalesHeader _Header = new SalesHeader();
            _Header.DOC_TYPE = "ZOR3";
            _Header.SALES_ORG = "1000";
            _Header.DISTR_CHAN = "10";
            _Header.DIVISION = "13";
            _Header.PURCH_NO_C = "C#";
            _SapSalesOrder.Header = _Header;
            SalesItem _Item = new SalesItem();
            _Item.ITM_NUMBER = "000010";
            _Item.MATERIAL = "12-04BFFB-SL7001";
            _Item.TARGET_QTY = "555";
            _ItemList.Add(_Item);
            SalesItem _Item2 = new SalesItem();
            _Item2.ITM_NUMBER = "000020";
            _Item2.MATERIAL = "ADC-07PMMS-LS7001";
            _Item2.TARGET_QTY = "100";
            _ItemList.Add(_Item2);
            _SapSalesOrder.ItemList = _ItemList;
            SalesPartner _Partner = new SalesPartner();
            _Partner.PARTN_ROLE = "AG";
            _Partner.PARTN_NUMB = "0010000242";
            _PartnerList.Add(_Partner);
            _SapSalesOrder.PartnerList = _PartnerList;
            SalesSchedule _Schedule = new SalesSchedule();
            _Schedule.REQ_QTY = "555";
            _Schedule.ITM_NUMBER = "000010";
            _Schedule.SCHED_LINE = "0001";
            _Schedule.REQ_DATE = "20190920";
            _ScheduleList.Add(_Schedule);
            SalesSchedule _Schedule2 = new SalesSchedule();
            _Schedule2.REQ_QTY = "50";
            _Schedule2.ITM_NUMBER = "000020";
            _Schedule2.SCHED_LINE = "0001";
            _Schedule2.REQ_DATE = "20190923";
            _ScheduleList.Add(_Schedule2);
            SalesSchedule _Schedule3 = new SalesSchedule();
            _Schedule3.REQ_QTY = "50";
            _Schedule3.ITM_NUMBER = "000020";
            _Schedule3.SCHED_LINE = "0002";
            _Schedule3.REQ_DATE = "20190928";
            _ScheduleList.Add(_Schedule3);
            _SapSalesOrder.ScheduleList = _ScheduleList;

            return _SapSalesOrder;

        }

        private void BtnGetBpEmail_Click(object sender, EventArgs e)
        {
            List<WebApi.DataModel.CustomModel.SAP.BpEmail> _BpEmailList = GetBpEmailList();
            XSLXHelper helper = new XSLXHelper();
            var xlsx = helper.Export(_BpEmailList);
            xlsx.SaveAs(filepath);
            MessageBox.Show("done");
        }
        private List<WebApi.DataModel.CustomModel.SAP.BpEmail> GetBpEmailList()
        {
            BLL.BpEmail _BpEmail = new BLL.BpEmail();
            List<WebApi.DataModel.CustomModel.SAP.BpEmail> _BpEmailList = new List<WebApi.DataModel.CustomModel.SAP.BpEmail>();
            //抓全部資料，由於不是參數化查詢，第二個參數傳null
            string _SqlBUT000 = @"select partner,name_org1 from  sapabap1.BUT000 ";
            DataTable dtBUT000 = OdbcHelper.GetDataTableText(_SqlBUT000, null);
            string _SqlBpEmail = @"  SELECT   partner1 , bu_sort1,name_first,  smtp_addr
                 FROM sapabap1.BUT051 a
                 join  sapabap1.BUT000 b on a.partner2=b.partner
                 join sapabap1.ADR6 c on b.persnumber=c.persnumber   ";
            DataTable dtBpEmail = OdbcHelper.GetDataTableText(_SqlBpEmail, null);
            _BpEmailList = _BpEmail.Parse(dtBUT000, dtBpEmail);
            return _BpEmailList;
        }

        private void BtnBpAddress_Click(object sender, EventArgs e)
        {
            List<WebApi.DataModel.CustomModel.SAP.BpAddress> _BpAddressList = GetBpAddressList();
            XSLXHelper helper = new XSLXHelper();
            var xlsx = helper.Export(_BpAddressList);
            xlsx.SaveAs(filepath);
            MessageBox.Show("done");
        }
        private List<WebApi.DataModel.CustomModel.SAP.BpAddress> GetBpAddressList()
        {
            BLL.BpAddress _BpAddress = new BLL.BpAddress();
            List<WebApi.DataModel.CustomModel.SAP.BpAddress> _BpAddressList = new List<WebApi.DataModel.CustomModel.SAP.BpAddress>();
            string _SqlKNA1 = @" SELECT   kunnr,a.name1,str_suppl1,str_suppl2  
                      from sapabap1.kna1 a 
                      join sapabap1.adrc b on a.adrnr=b.ADDRNUMBER ";
            DataTable dtKNA1 = OdbcHelper.GetDataTableText(_SqlKNA1, null);
            _BpAddressList = _BpAddress.Parse(dtKNA1);
            return _BpAddressList;
        }
        /// <summary>
        /// get customer address data no need content end cust(20001XXX)
        /// except customer be delete 
        /// --d.bu_group='1000'
        /// --and
        /// </summary>
        /// <returns></returns>
        private List<WebApi.DataModel.CustomModel.SAP.BpAddressAll> GetBpAddressAllList()
        {
            string _SqlKNA1 = @"   SELECT  a.land1, a.kunnr,a.name1,str_suppl1,str_suppl2 ,b.street
                       ,b.LOCATION
                       ,b.POST_CODE1,b.CITY1
                       ,b.COUNTRY,b.REGION,b.TIME_ZONE
                       ,  CITY2
                       ,str_suppl3 
                       ,HOME_CITY
                       ,c.VKORG
                      from sapabap1.kna1 a 
                      join sapabap1.adrc b on a.adrnr=b.ADDRNUMBER 
                      join sapabap1.knvv c on a.kunnr=c.kunnr
                      join sapabap1.but000 d on a.kunnr = d.partner
                  where  
                  d.bu_group='1000'
                  and 
                  c.loevm <> 'X'  
                  order by a.kunnr ";
            DataTable dtKNA1 = OdbcHelper.GetDataTableText(_SqlKNA1, null);
            IList<WebApi.DataModel.CustomModel.SAP.BpAddressAll> _BpAddressAllIList = dtKNA1.ToList<WebApi.DataModel.CustomModel.SAP.BpAddressAll>();
            List<WebApi.DataModel.CustomModel.SAP.BpAddressAll> _BpAddressAllList = _BpAddressAllIList as List<WebApi.DataModel.CustomModel.SAP.BpAddressAll>;
            return _BpAddressAllList;
        }

        private void BtnAddressAll_Click(object sender, EventArgs e)
        {
            List<WebApi.DataModel.CustomModel.SAP.BpAddressAll> _BpAddressAllList = GetBpAddressAllList();
            XSLXHelper helper = new XSLXHelper();
            var xlsx = helper.Export(_BpAddressAllList);
            xlsx.SaveAs(filepathNoExt + "_Address" + ".xlsx");
            MessageBox.Show("done");
        }
        private List<WebApi.DataModel.CustomModel.SAP.BpSales> GetBpSalesList()
        {
            string _SqlKNA1 = @"    SELECT   a.vkorg, a.kunnr ,c.name1,c.name2,SNAME 
             FROM sapabap1.KNVP a
             INNER JOIN sapabap1.PA0001 b  ON b.PERNR = a.PERNR 
             join sapabap1.kna1 c on a.kunnr=c.kunnr ";
            DataTable dtKNA1 = OdbcHelper.GetDataTableText(_SqlKNA1, null);
            IList<WebApi.DataModel.CustomModel.SAP.BpSales> _BpSalesIList = dtKNA1.ToList<WebApi.DataModel.CustomModel.SAP.BpSales>();
            List<WebApi.DataModel.CustomModel.SAP.BpSales> _BpSalesList = _BpSalesIList as List<WebApi.DataModel.CustomModel.SAP.BpSales>;
            return _BpSalesList;
        }

        private void BtnSales_Click(object sender, EventArgs e)
        {
            List<WebApi.DataModel.CustomModel.SAP.BpSales> _BpSalesList = GetBpSalesList();
            XSLXHelper helper = new XSLXHelper();
            var xlsx = helper.Export(_BpSalesList);
            xlsx.SaveAs(filepathNoExt + "_Sales" + ".xlsx");
            MessageBox.Show("done");
        }

        private void BtnMoqOrder_Click(object sender, EventArgs e)
        {
            string _Tag = ((Button)sender).Tag.ToString();
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            List<WebApi.DataModel.CustomModel.SAP.SapLessMoqOrder> _SapLessMoqOrderList = GetSapLessMoqOrderList(_Tag);
            XSLXHelper helper = new XSLXHelper();
            var xlsx = helper.Export(_SapLessMoqOrderList);
            xlsx.SaveAs(filepathNoExt + "_MoqOrder" + ".xlsx");
            MessageBox.Show("done");
            sw.Stop();
            MessageBox.Show(sw.ElapsedMilliseconds.ToString() + "毫秒");
        }
        /// <summary>
        /// get 20181001 to 20191031 all less mog order
        /// TWD/JPY need to multiply 100
        /// cost = sap price / per
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private List<WebApi.DataModel.CustomModel.SAP.SapLessMoqOrder> GetSapLessMoqOrderList(string tag)
        {
            List<WebApi.DataModel.CustomModel.SAP.SapLessMoqOrder> _SapLessMoqOrderList = new List<WebApi.DataModel.CustomModel.SAP.SapLessMoqOrder>();
            BLL.SapMOQ _SapMOQ = new BLL.SapMOQ();
            string _SqlOrder = @"   select   case a.waerk  
                               when 'TWD' then    b.netpr/b.kpein*100 
                               when 'JPY' then    b.netpr/b.kpein*100
                               else  b.netpr/b.kpein
                               end as cost,
                               e.name1,
                               a.AUART,
                               b.kpein,a.vbeln,b.KWMENG,b.ZMENG,b.NETPR, a.kunnr,a.erdat,a.waerk,a.spart,
                               d.sname,
                               b.matnr, 
                               b.werks from sapabap1.vbak a
                               join sapabap1.vbap b on a.vbeln=b.vbeln
                               join sapabap1.vbpa c on a.vbeln=c.vbeln
                               join sapabap1.pa0001 d on c.PERNR = d.PERNR
                               join sapabap1.kna1 e on a.kunnr=e.kunnr
                               where  a.AUART not in ('ZDE1','ZDE2','ZCR1' ,'ZCR2', 'ZCR3', 'ZOR8') and 
                               c.parvw='VE' 
                               and a.ERDAT >'20181001' and a.ERDAT <'20191031'
                               order by a.vbeln ";
            DataTable dtOrder = OdbcHelper.GetDataTableText(_SqlOrder, null);
            int _A = dtOrder.Rows.Count;
            string _SqlMOQ_MARC = @" select BSTMI,WERKS,MATNR from sapabap1.marc 
                                order by MATNR";
            string _SqlMOQ_EINE = @"select a.MINBM  ,a.NORBM,b.MATNR,WERKS from sapabap1.EINE a
                               join sapabap1.EINA b on a.INFNR = b.INFNR order by a.NORBM  desc ";
            string _SqlMOQ = tag == "MARC" ? _SqlMOQ_MARC : _SqlMOQ_EINE;
            DataTable dtMOQ = OdbcHelper.GetDataTableText(_SqlMOQ, null);
            int _B = dtMOQ.Rows.Count;
            string _SqlDepartment = @"SELECT   SPRAS,VTEXT  ,SPART 
                               FROM sapabap1.TSPAT
                               WHERE   SPRAS = 'M' ";
            DataTable dtDepartment = OdbcHelper.GetDataTableText(_SqlDepartment, null);
            int _C = dtDepartment.Rows.Count;
            _SapLessMoqOrderList = _SapMOQ.Parse(dtOrder, dtMOQ, dtDepartment, tag);
            return _SapLessMoqOrderList;
        }

        private void BtnPriceGroup_Click(object sender, EventArgs e)
        {
            List<WebApi.DataModel.CustomModel.SAP.BpPriceGroup> _BpPriceGroupList = GetBpPriceGroupList();
            XSLXHelper helper = new XSLXHelper();
            var xlsx = helper.Export(_BpPriceGroupList);
            xlsx.SaveAs(filepathNoExt + "_PriceGroup" + ".xlsx");
            MessageBox.Show("done");
        }
        private List<WebApi.DataModel.CustomModel.SAP.BpPriceGroup> GetBpPriceGroupList()
        {
            List<WebApi.DataModel.CustomModel.SAP.BpPriceGroup> _BpPriceGroupList = new List<WebApi.DataModel.CustomModel.SAP.BpPriceGroup>();
            BLL.BpPriceGroup _BpPriceGroup = new BLL.BpPriceGroup();
            string _Sql = @"  select   a.kunnr,c.BU_SORT1,c.BU_SORT2,vkorg,case KONDA   when '01' then 'DIS'
                    when '02' then 'ITP'
                    when '03' then 'ICAT'
                    when '04' then 'MSRP' 
                    end as PriceGroup ,b.BAHNE from sapabap1.KNVV a
                    join sapabap1.kna1 b on a.kunnr=b.kunnr
                    join  sapabap1.but000 c on a.kunnr = c.partner 
                    where  c.bu_group='1000'
                    and a.loevm <> 'X'   ";
            DataTable dt = OdbcHelper.GetDataTableText(_Sql, null);
            _BpPriceGroupList = _BpPriceGroup.Parse(dt);
            return _BpPriceGroupList;
        }
         

        private void BtnBpXY_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            List<WebApi.DataModel.CustomModel.SAP.BpXY> _BpXYList = GetBpXYList();
            XSLXHelper helper = new XSLXHelper();
            var xlsx = helper.Export(_BpXYList);
            xlsx.SaveAs(filepathNoExt + "_XY" + ".xlsx");
            MessageBox.Show("done");
            sw.Stop();
            MessageBox.Show(sw.ElapsedMilliseconds.ToString() + "毫秒");
        }
        /// <summary>
        /// only get buyer 1000 data
        /// country name in english
        /// US/CN need mapping region
        /// </summary>
        /// <returns></returns>
        private List<WebApi.DataModel.CustomModel.SAP.BpXY> GetBpXYList()
        {
            List<WebApi.DataModel.CustomModel.SAP.BpXY> _BpXYList = new List<WebApi.DataModel.CustomModel.SAP.BpXY>();
            BLL.BpXY _BpXY = new BLL.BpXY();
            string _Sql = @"   select   a.kunnr,c.BU_SORT1,c.BU_SORT2,vkorg,d.NAME_CO ,a.KDGRP ,b.land1 ,e.landx,d.REGION
                               from sapabap1.KNVV a
                               join sapabap1.kna1 b on a.kunnr=b.kunnr
                               join  sapabap1.but000 c on a.kunnr = c.partner 
                               JOIN sapabap1.ADRC d   ON d.ADDRNUMBER = b.ADRNR 
                               join sapabap1.t005t e on b.land1=e.land1
                    where  c.bu_group='1000' and e.spras='E' and e.mandt='888'
                    and a.loevm <> 'X'    ";
            DataTable dt = OdbcHelper.GetDataTableText(_Sql, null);
            string _SqlRegion = @"  select  bland,bezei from sapabap1.T005U where mandt='888'  and spras='E' and (land1='CN'
                      or land1='US')  ";
            DataTable dtRegion = OdbcHelper.GetDataTableText(_SqlRegion, null);
            _BpXYList = _BpXY.Parse(dt, dtRegion);
            string _SqlCE11000 = @" select  ERLOS,GJAHR,KNDNR from sapabap1.CE11000
                                   WHERE PALEDGER = '01'
                                   AND ( VRGAR = 'F' OR  VRGAR = 'Y' )   ";
            DataTable dtCE11000 = OdbcHelper.GetDataTableText(_SqlCE11000, null);
            _BpXYList = _BpXY.ParseRevenue(_BpXYList, dtCE11000);
            return _BpXYList;
        }

        private static System.Drawing.Image DrawText(String text, System.Drawing.Font font, Color textColor, Color backColor, double height, double width)
        {
            //create a bitmap image with specified width and height
            Image img = new Bitmap((int)width, (int)height);
            Graphics drawing = Graphics.FromImage(img);
            //get the size of text
            SizeF textSize = drawing.MeasureString(text, font);
            //set rotation point
            drawing.TranslateTransform(((int)width - textSize.Width) / 2, ((int)height - textSize.Height) / 2);
            //rotate text
            drawing.RotateTransform(-45);
            //reset translate transform
            drawing.TranslateTransform(-((int)width - textSize.Width) / 2, -((int)height - textSize.Height) / 2);
            //paint the background
            drawing.Clear(backColor);
            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);
            //draw text on the image at center position
            drawing.DrawString(text, font, textBrush, ((int)width - textSize.Width) / 2, ((int)height - textSize.Height) / 2);
            drawing.Save();
            return img;
        }

        private void BtnWaterMark_Click(object sender, EventArgs e)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(@"D:\Users\Jelly\ALTW\TestWebApi\bin\Debug\Success\20191120104818_MoqOrder_By採購單_標準採購數量_最大值為主.xlsx");
            Font font = new System.Drawing.Font("arial", 40);
            String watermark = "Confidential";
            foreach (Worksheet sheet in workbook.Worksheets)
            {
                //call DrawText() to create an image
                System.Drawing.Image imgWtrmrk = DrawText(watermark, font, System.Drawing.Color.LightCoral, System.Drawing.Color.White, sheet.PageSetup.PageHeight, sheet.PageSetup.PageWidth);
                //set image as left header image
                sheet.PageSetup.LeftHeaderImage = imgWtrmrk;
                sheet.PageSetup.LeftHeader = "&G";
                //the watermark will only appear in this mode, it will disappear if the mode is normal
                sheet.ViewMode = ViewMode.Layout;
            }
            workbook.SaveToFile(@"D:\Users\Jelly\ALTW\TestWebApi\bin\Debug\Success\resulte2.xlsx", ExcelVersion.Version2010);
            System.Diagnostics.Process.Start(@"D:\Users\Jelly\ALTW\TestWebApi\bin\Debug\Success\resulte2.xlsx");
        }

        private void BtnEdiTest_Click(object sender, EventArgs e)
        {
            bool _OK=readEdiTest();
            string edi =
                @"ISA*01*0000000000*01*0000000000*ZZ*ABCDEFGHIJKLMNO*ZZ*123456789012345*101127*1719*U*00400*000003438*0*P*>
                GS*OW*7705551212*3111350000*20000128*0557*3317*T*004010
                ST*940*0001
                W05*N*538686**001001*538686
                LX*1
                W01*12*CA*000100000010*VN*000100*UC*DEC0199******19991205
                G69*11.500 STRUD BLUBRY
                W76*56*500*LB*24*CF
                SE*7*0001
                GE*1*3317
                IEA*1*000003438";


            EdiDataReader r = new EdiDataReader();
            EdiBatch b = r.FromString(edi);

            //Serialize the whole batch to JSON
            JsonDataWriter w1 = new JsonDataWriter();
            string json = w1.WriteToString(b);

            //OR Serialize selected EDI message to Json
            string jsonTrans = JsonConvert.SerializeObject(b.Interchanges[0].Groups[0].Transactions[0]);

            //Serialize the whole batch to XML
            XmlDataWriter w2 = new XmlDataWriter();
            string xml = w2.WriteToString(b);
        } 
        private void BtnArea_Click(object sender, EventArgs e)
        {
            List<WebApi.DataModel.CustomModel.SAP.BpArea> _BpAreaList = GetBpAreaList();
            XSLXHelper helper = new XSLXHelper();
            var xlsx = helper.Export(_BpAreaList);
            xlsx.SaveAs(filepathNoExt + "_Area" + ".xlsx");
            MessageBox.Show("done");
        }
        private List<WebApi.DataModel.CustomModel.SAP.BpArea> GetBpAreaList()
        {
            string _Sql = @"select * from sapabap1.T005U where  land1='CN' and (SPRAS='1' or SPRAS='M' or SPRAS='E' )  and MANDT='888' ";
            DataTable dt = OdbcHelper.GetDataTableText(_Sql, null);
            string _SqlUS = @"select * from sapabap1.T005U where  land1='US' and (SPRAS='1' or SPRAS='M' or SPRAS='E')  and MANDT='888' ";
            DataTable dtUS = OdbcHelper.GetDataTableText(_SqlUS, null);
            dt.Merge(dtUS);
            IList<WebApi.DataModel.CustomModel.SAP.BpArea> _BpAreaIList = dt.ToList<WebApi.DataModel.CustomModel.SAP.BpArea>();
            List<WebApi.DataModel.CustomModel.SAP.BpArea> _BpAreaList = _BpAreaIList as List<WebApi.DataModel.CustomModel.SAP.BpArea>;
            return _BpAreaList;
        }

        private void BtnCountry_Click(object sender, EventArgs e)
        {
            List<WebApi.DataModel.CustomModel.SAP.BpCountry> _BpCountryList = GetCountryList();
            XSLXHelper helper = new XSLXHelper();
            var xlsx = helper.Export(_BpCountryList);
            xlsx.SaveAs(filepathNoExt + "_Country" + ".xlsx");
            MessageBox.Show("done");
        }
        private List<WebApi.DataModel.CustomModel.SAP.BpCountry> GetCountryList()
        {
            string _Sql = @"select SPRAS,LAND1,LANDX from sapabap1.T005T    
where  ( SPRAS='1' or SPRAS='M' or SPRAS='E'   ) and MANDT='888' ";
            DataTable dt = OdbcHelper.GetDataTableText(_Sql, null);
            IList<WebApi.DataModel.CustomModel.SAP.BpCountry> _BpCountryIList = dt.ToList<WebApi.DataModel.CustomModel.SAP.BpCountry>();
            List<WebApi.DataModel.CustomModel.SAP.BpCountry> _BpCountryList = _BpCountryIList as List<WebApi.DataModel.CustomModel.SAP.BpCountry>;
            return _BpCountryList;
        }  
       
        private bool readEdiTest( )
        {
            bool _OK = false;
            try
            {
                EdiDataReader r = new EdiDataReader();
                EdiBatch b = r.FromString(txtANSIX12.Text);
                _OK = true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message); 
            }
            return _OK;
        }
        /// <summary>
        /// test RSSBus API
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRSSAPI_Click(object sender, EventArgs e)
        {
            string _ConnectionString = ConfigurationManager.AppSettings["EdiApi"];
            using (WebClient webClient = new WebClient())
            {
                webClient.UseDefaultCredentials = true;
                webClient.Credentials = new NetworkCredential("Jelly", "8z0X8c6o6Z2p9f6X7n3m");
                // 從 url 讀取資訊至 stream
                using (Stream stream = webClient.OpenRead(_ConnectionString + "files"))
                // 使用 StreamReader 讀取 stream 內的字元
                using (StreamReader reader = new StreamReader(stream))
                {
                    // 將 StreamReader 所讀到的字元轉為 string
                    string request = reader.ReadToEnd();
                    MessageBox.Show(request);
                }
            }
        }
        /// <summary>
        /// upload test.edi to RSSBus -test ok but no output file 
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        private string Post(string jsonData)
        {
            string _ConnectionString = ConfigurationManager.AppSettings["EdiApi"];
            string result = "";
            using (WebClient webClient = new WebClient())
            {
                webClient.UseDefaultCredentials = true;
                webClient.Credentials = new NetworkCredential("Jelly", "8z0X8c6o6Z2p9f6X7n3m");
                // 指定 WebClient 編碼
                webClient.Encoding = Encoding.UTF8;
                // 指定 WebClient 的 Content-Type header
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                // 執行 post 動作
                result = webClient.UploadString(_ConnectionString + "files", jsonData);
            }
            return result;
        } 
        /// <summary>
        /// test RSSBus API upload file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button4_Click(object sender, EventArgs e)
        {
            Byte[] bytes = File.ReadAllBytes(@"D:\Users\Jelly\ALTW\TestWebApi\bin\Debug\Edi\AS2_Mouser\Send\test.edi");
            String content = Convert.ToBase64String(bytes);
            String json = "{ \"Folder\": \"Send\",\"Filename\": \"test.edi\",\"Content\": \"" + content + "\",\"PortId\": \"AS2_Mouser\",\"MessageId\": \"\"}";
            Post(json);
        } 
    }
}
