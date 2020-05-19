using System;
using WebApi.Models.CustomModel.SAP;
using NLog;
//using SAP.Middleware.Connector;
using System.Configuration;
using WebApi.ThirdParty;
namespace WebApi.ThirdParty.SAP
{
    /// <summary>
    /// for test
    /// </summary>
    public class SapService : ISapService
    {
        static string Id, Kokrs;
        Budget _Budget = new Budget();
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public Budget budgetStart(string id, string kokrs)
        {
            Id = id;
            Kokrs = kokrs;
            messsap();
            return _Budget;
        }
        private void messsap()//預算
        {
            System.Threading.Thread s = new System.Threading.Thread(new System.Threading.ThreadStart(getBudget));
            //Set the run mode 'STA' 
            s.SetApartmentState(System.Threading.ApartmentState.STA);
            s.Start();
            s.Join();
        }
        private void getBudget()
        {
            try
            {
                //SAPLogonCtrl.SAPLogonControlClass login = new SAPLogonCtrl.SAPLogonControlClass();
                //login.ApplicationServer = "10.0.0.79";
                //login.Client = "800";
                //login.Language = "ZF";
                //login.User = "L180067";
                //login.Password = "2819Jelly";
                //login.SystemNumber = 00;
                SapConnection _SapConn = SapConnection.GetInstance;
                //     if (!_SapConn.SapLogin) throw new Exception("登入SAP失敗");
                //    SAPFunctionsOCX.SAPFunctionsClass func = SapConnection.GetInstance_SAPFunctionsClass;
                //  SAPLogonCtrl.Connection conn = (SAPLogonCtrl.Connection)login.NewConnection();
                if (_SapConn.SapLogin)
                {
                    SAPFunctionsOCX.SAPFunctionsClass func = SapConnection.GetInstance_SAPFunctionsClass;
                    // SAPFunctionsOCX.SAPFunctionsClass func = new SAPFunctionsOCX.SAPFunctionsClass();
                    // func.Connection = conn;
                    //功能模組名稱 
                    SAPFunctionsOCX.IFunction ifunc = (SAPFunctionsOCX.IFunction)func.Add("Z_GET_IO_BUDGET");
                    //查詢參數：成本控制範圍
                    SAPFunctionsOCX.IParameter parameter1 = (SAPFunctionsOCX.IParameter)ifunc.get_Exports("I_KOKRS");
                    parameter1.Value = Id; // BBB; 
                    //查詢參數：內部訂單
                    SAPFunctionsOCX.IParameter parameter2 = (SAPFunctionsOCX.IParameter)ifunc.get_Exports("I_AUFNR");
                    parameter2.Value = Kokrs; // AAA;// "P180001CLB";
                    ifunc.Call();
                    SAPTableFactoryCtrl.Tables tables = (SAPTableFactoryCtrl.Tables)ifunc.Tables;
                    SAPTableFactoryCtrl.Table ENQEZKNAS = (SAPTableFactoryCtrl.Table)tables.get_Item("T_BUDGET");
                    int n = ENQEZKNAS.RowCount;
                    for (int i = 1; i <= n; i++)
                    {
                        _Budget.MANDT = ENQEZKNAS.get_Cell(i, "MANDT").ToString();
                        _Budget.KOKRS = ENQEZKNAS.get_Cell(i, "KOKRS").ToString();
                        _Budget.AUFNR = ENQEZKNAS.get_Cell(i, "AUFNR").ToString();
                        _Budget.WAERS = ENQEZKNAS.get_Cell(i, "WAERS").ToString();
                        _Budget.START_AMT = ENQEZKNAS.get_Cell(i, "START_AMT").ToString();
                        _Budget.USED_AMT = ENQEZKNAS.get_Cell(i, "USED_AMT").ToString();
                        _Budget.END_AMT = ENQEZKNAS.get_Cell(i, "END_AMT").ToString();
                        _Budget.COMMIT_AMT = ENQEZKNAS.get_Cell(i, "COMMIT_AMT").ToString();
                        _Budget.START_AMT_TOL = ENQEZKNAS.get_Cell(i, "START_AMT_TOL").ToString();
                        _Budget.USED_AMT_TOL = ENQEZKNAS.get_Cell(i, "USED_AMT_TOL").ToString();
                        _Budget.END_AMT_TOL = ENQEZKNAS.get_Cell(i, "END_AMT_TOL").ToString();
                        _Budget.COMMIT_AMT_TOL = ENQEZKNAS.get_Cell(i, "COMMIT_AMT_TOL").ToString();
                    }
                }
                else
                {
                    throw new Exception("登入SAP失敗");
                }
                //conn.Logoff();
                _SapConn.ConnLogoff();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void Sap_CreateOrder()
        {
          //  SapConnectorInterface _Sap = new SapConnectorInterface();
            // _Sap.TestConnection("QAS");-ok
            // _Sap.GetBudget("QAS");-ok
           // _Sap.CreateOrder("QAS");
        }
        private void messsap_order()//預算
        {
            System.Threading.Thread s = new System.Threading.Thread(new System.Threading.ThreadStart(createOrder));
            //Set the run mode 'STA' 
            s.SetApartmentState(System.Threading.ApartmentState.STA);
            s.Start();
            s.Join();
        }
        /// <summary>
        /// test not success
        /// </summary>
        private void createOrder()
        {
            try
            {
                SapConnection _SapConn = SapConnection.GetInstance;
                if (_SapConn.SapLogin)
                {
                    SAPFunctionsOCX.SAPFunctionsClass func = SapConnection.GetInstance_SAPFunctionsClass;
                    //功能模組名稱 
                    SAPFunctionsOCX.IFunction ifunc = (SAPFunctionsOCX.IFunction)func.Add("ZSD_CREATE_ORDER");
                    SAPTableFactoryCtrl.Tables _I_CREATE_ORDER = (SAPTableFactoryCtrl.Tables)ifunc.Tables;
                    SAPTableFactoryCtrl.Table _I_ORDER_HEADER_IN = (SAPTableFactoryCtrl.Table)_I_CREATE_ORDER.get_Item("I_ORDER_HEADER_IN");
                    _I_ORDER_HEADER_IN.AppendGridData(1, 1, 1, "doc_type  = 'ZOR1'");
                    _I_ORDER_HEADER_IN.AppendGridData(2, 1, 1, "sales_org   = '2000'");
                    _I_ORDER_HEADER_IN.AppendGridData(3, 1, 1, "distr_chan   = '30'");
                    _I_ORDER_HEADER_IN.AppendGridData(4, 1, 1, "division   = '30'");
                    _I_ORDER_HEADER_IN.AppendGridData(5, 1, 1, "purch_date   = '20190916'");
                    _I_ORDER_HEADER_IN.AppendGridData(6, 1, 1, "purch_no_c   = 'C#'");
                    SAPTableFactoryCtrl.Table _I_ORDER_PARTNERS = (SAPTableFactoryCtrl.Table)_I_CREATE_ORDER.get_Item("I_ORDER_PARTNERS");
                    _I_ORDER_PARTNERS.AppendGridData(1, 1, 1, "partn_role = 'AG'");
                    _I_ORDER_PARTNERS.AppendGridData(2, 1, 1, "partn_numb = '0010000242'");

                    SAPTableFactoryCtrl.Table _I_ORDER_ITEMS_IN = (SAPTableFactoryCtrl.Table)_I_CREATE_ORDER.get_Item("I_ORDER_ITEMS_IN");

                    _I_ORDER_ITEMS_IN.AppendGridData(1, 1, 1, "MATERIAL = '12D-04PFFP-SF8001'");
                    _I_ORDER_ITEMS_IN.AppendGridData(2, 1, 1, "itm_number = '000010'");
                    _I_ORDER_ITEMS_IN.AppendGridData(3, 1, 1, "target_qty = '20.000'");
                    _I_ORDER_ITEMS_IN.AppendGridData(4, 1, 1, "arget_qu = 'ST'");
                    _I_ORDER_ITEMS_IN.AppendGridData(5, 1, 1, "sales_unit = 'ST'");
                    SAPTableFactoryCtrl.Table _I_ORDER_SCHEDULES_IN = (SAPTableFactoryCtrl.Table)_I_CREATE_ORDER.get_Item("I_ORDER_SCHEDULES_IN");
                    _I_ORDER_SCHEDULES_IN.AppendGridData(1, 1, 1, "req_qty = '20.000'");
                    _I_ORDER_SCHEDULES_IN.AppendGridData(2, 1, 1, "ITM_NUMBER = '000010'");
                    _I_ORDER_SCHEDULES_IN.AppendGridData(3, 1, 1, "SCHED_LINE = '1'");
                    _I_ORDER_SCHEDULES_IN.AppendGridData(4, 1, 1, "REQ_DATE = '20190917'");

                    ifunc.Call();
                    SAPTableFactoryCtrl.Table _Return = (SAPTableFactoryCtrl.Table)_I_CREATE_ORDER.get_Item("E_RETURN");
                    int n = _Return.RowCount;
                    for (int i = 1; i <= n; i++)
                    {
                        _Budget.MANDT = _Return.get_Cell(i, "TYPE").ToString();
                        _Budget.KOKRS = _Return.get_Cell(i, "ID").ToString();
                        _Budget.AUFNR = _Return.get_Cell(i, "NUMBER").ToString();
                        _Budget.WAERS = _Return.get_Cell(i, "MESSAGE").ToString();
                        _Budget.START_AMT = _Return.get_Cell(i, "LOG_NO").ToString();
                        _Budget.USED_AMT = _Return.get_Cell(i, "MESSAGE_V1").ToString();
                        _Budget.END_AMT = _Return.get_Cell(i, "PARAMETER").ToString();
                        _Budget.COMMIT_AMT = _Return.get_Cell(i, "ROW").ToString();
                    }
                }
                else
                {
                    throw new Exception("登入SAP失敗");
                }
                //conn.Logoff();
                _SapConn.ConnLogoff();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

    }

}