using System;
using System.Collections.Generic;
using SAP.Middleware.Connector;
using System.Data;
using WebApi.Service.Interface.Table;
using WebApi.Models;
using WebApi.DataModel.CustomModel.SAP;
using WebApi.DataModel.CustomModel.SAP.SalesOrder;
namespace WebApi.ThirdParty.SAP
{
    public class SapConnectorInterface : ISapConnectorInterface
    {
        private RfcDestination rfcDestination;
        private IErrorLogService _errorLogService;
        public SapConnectorInterface(IErrorLogService errorLogService)
        {
            this._errorLogService = errorLogService;
        }
        /// <summary>
        /// test ok 
        /// </summary>
        /// <param name="destinationName"></param>
        /// <returns></returns>
        public bool TestConnection(string destinationName)
        {
            bool result = false;
            try
            {
                rfcDestination = RfcDestinationManager.GetDestination(destinationName);
                if (rfcDestination != null)
                {
                    rfcDestination.Ping();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;

        }
        /// <summary>
        /// you need  excute(se38) ABLM_MODIFY_ITEMS in sap system and set relavant parameter
        /// previous when you use BAPI_SALESORDER_CREATEFROMDAT2 this RFC module 
        /// </summary>
        /// <param name="destinationName"></param>
        /// <returns></returns>
        public string CreateOrder(SapSalesOrder salesOrder)
        {
            if (rfcDestination == null)
            {
                rfcDestination = RfcDestinationManager.GetDestination(WebApiApplication.destinationConfigName);
            }
            RfcRepository repo = rfcDestination.Repository;
            IRfcFunction _SalesDoc = repo.CreateFunction("BAPI_SALESORDER_CREATEFROMDAT2");
            IRfcFunction _SalesDocCommit = repo.CreateFunction("BAPI_TRANSACTION_COMMIT");
            IRfcStructure _SalesHeader = _SalesDoc.GetStructure("ORDER_HEADER_IN");
            IRfcTable _SalesItems = _SalesDoc.GetTable("ORDER_ITEMS_IN");
            IRfcTable _SalesPartners = _SalesDoc.GetTable("ORDER_PARTNERS");
            IRfcTable _SalesSchedule = _SalesDoc.GetTable("ORDER_SCHEDULES_IN");
            _SalesHeader.SetValue("DOC_TYPE", salesOrder.Header.DOC_TYPE);
            _SalesHeader.SetValue("SALES_ORG", salesOrder.Header.SALES_ORG);
            _SalesHeader.SetValue("DISTR_CHAN", salesOrder.Header.DISTR_CHAN);
            _SalesHeader.SetValue("DIVISION", salesOrder.Header.DIVISION);
            _SalesHeader.SetValue("PURCH_NO_C", salesOrder.Header.PURCH_NO_C);
            //test 採購日期
            _SalesHeader.SetValue("PURCH_DATE", salesOrder.Header.PURCH_DATE);
            foreach (SalesItem si in salesOrder.ItemList)
            {
                IRfcStructure _SalesItemsStruct = _SalesItems.Metadata.LineType.CreateStructure();
                _SalesItemsStruct.SetValue("ITM_NUMBER", si.ITM_NUMBER);
                //_SalesItemsStruct.SetValue("MATERIAL", si.MATERIAL);
                _SalesItemsStruct.SetValue("MATERIAL_LONG", si.MATERIAL); 
                _SalesItemsStruct.SetValue("TARGET_QTY", si.TARGET_QTY);
                //test 客戶物料號碼
                _SalesItemsStruct.SetValue("CUST_MAT35", si.CUST_MAT35);
                _SalesItems.Append(_SalesItemsStruct);
            }
            foreach (SalesPartner sp in salesOrder.PartnerList)
            {
                IRfcStructure _SalesPartnersStruct = _SalesPartners.Metadata.LineType.CreateStructure();
                _SalesPartnersStruct.SetValue("PARTN_ROLE", sp.PARTN_ROLE);
                _SalesPartnersStruct.SetValue("PARTN_NUMB", sp.PARTN_NUMB);
                _SalesPartners.Append(_SalesPartnersStruct);
            }
            foreach (SalesSchedule ss in salesOrder.ScheduleList)
            {
                IRfcStructure _SalesScheduleStruct = _SalesSchedule.Metadata.LineType.CreateStructure();
                _SalesScheduleStruct.SetValue("REQ_QTY", ss.REQ_QTY);
                _SalesScheduleStruct.SetValue("ITM_NUMBER", ss.ITM_NUMBER);
                _SalesScheduleStruct.SetValue("SCHED_LINE", ss.SCHED_LINE);
                _SalesScheduleStruct.SetValue("REQ_DATE", ss.REQ_DATE);
                _SalesSchedule.Append(_SalesScheduleStruct);
            }
            ////salesPartnersStruct.SetValue("PARTN_ROLE", "RE");
            ////salesPartnersStruct.SetValue("PARTN_NUMB", "0010000650");
            ////salesPartners.Append(salesPartnersStruct);   
            RfcSessionManager.BeginContext(rfcDestination);
            _SalesDoc.Invoke(rfcDestination);
            _SalesDocCommit.Invoke(rfcDestination);
            RfcSessionManager.EndContext(rfcDestination);
            string _SalesCocument = _SalesDoc.GetString("SALESDOCUMENT");
            DataTable dtReturn = ConvertToDotNetTable(_SalesDoc.GetTable("RETURN"));
            List<ErrorLog> _ErrorList = SetErrorLog(dtReturn, "BAPI_SALESORDER_CREATEFROMDAT2", _SalesCocument, salesOrder.Header.PURCH_NO_C);
            List<string> _Error = this._errorLogService.MiltiCreate(_ErrorList);
            return _SalesCocument;
        }
        /// <summary>
        /// test Z_GET_IO_BUDGET RFC ok 
        /// </summary>
        /// <param name="destinationName"></param>
        /// <returns></returns>
        public DataSet GetBudget(string destinationName)
        {
            DataSet dsBudget = new DataSet();
            try
            {
                if (rfcDestination == null)
                {
                    rfcDestination = RfcDestinationManager.GetDestination(destinationName);
                }
                RfcRepository repo = rfcDestination.Repository;
                IRfcFunction salesDoc = repo.CreateFunction("Z_GET_IO_BUDGET");
                salesDoc.SetValue("I_KOKRS", "2000");
                salesDoc.SetValue("I_AUFNR", "P170008CKF");
                salesDoc.Invoke(rfcDestination);
                dsBudget.Tables.Add(ConvertToDotNetTable(salesDoc.GetTable("T_BUDGET")));
            }
            catch (Exception ex)
            {

            }
            return dsBudget;
        }
        /// <summary>
        /// sap table data convert to datatable 
        /// </summary>
        /// <param name="rfcTable"></param>
        /// <returns></returns>
        private DataTable ConvertToDotNetTable(IRfcTable rfcTable)
        {
            DataTable dtTable = new DataTable();
            for (int i = 0; i < rfcTable.ElementCount; i++)
            {
                RfcElementMetadata metadta = rfcTable.GetElementMetadata(i);
                dtTable.Columns.Add(metadta.Name);
            }
            foreach (IRfcStructure row in rfcTable)
            {
                DataRow dr = dtTable.NewRow();
                for (int i = 0; i < rfcTable.ElementCount; i++)
                {
                    RfcElementMetadata metadata = rfcTable.GetElementMetadata(i);
                    if (metadata.DataType == RfcDataType.BCD && metadata.Name == "ABC")
                    {
                        dr[i] = row.GetInt(metadata.Name);
                    }
                    else
                    {
                        dr[i] = row.GetString(metadata.Name);
                    }
                }
                dtTable.Rows.Add(dr);
            }
            return dtTable;
        }
        /// <summary>
        /// insert error log in edi db
        /// </summary>
        /// <param name="dtErrorLog"></param>
        /// <param name="moduleName"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private List<ErrorLog> SetErrorLog(DataTable dtErrorLog, string moduleName, string number,string po)
        {
            List<ErrorLog> _ErrorLogList = new List<ErrorLog>();
            foreach (DataRow dr in dtErrorLog.Rows)
            {
                ErrorLog _ErrorLog = new ErrorLog();
                _ErrorLog.Type = dr["TYPE"].ToString();
                _ErrorLog.Message = dr["MESSAGE"].ToString();
                // _ErrorLog.Parameter = dr["PARAMETER"].ToString();
                _ErrorLog.Parameter = po;
                _ErrorLog.Module = moduleName;
                _ErrorLog.Class = "Sap";
                _ErrorLog.Date = DateTime.Now.ToString("yyyyMMdd");
                _ErrorLog.UpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                _ErrorLogList.Add(_ErrorLog);
            }
            if (number != "")
            {
                ErrorLog _ErrorLog = new ErrorLog();
                _ErrorLog.Type = "S";
                _ErrorLog.Message = "create data success ,return number is " + number;
                _ErrorLog.Parameter = number;
                _ErrorLog.Module = moduleName;
                _ErrorLog.Class = "Sap";
                _ErrorLog.Date = DateTime.Now.ToString("yyyyMMdd");
                _ErrorLog.UpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                _ErrorLogList.Add(_ErrorLog);
            }
            return _ErrorLogList;
        }
        /// <summary>
        /// create bp data by job table:BUT000
        /// no use
        /// </summary>
        /// <returns></returns>
        public string CreateBusinessPartner()
        {
            if (rfcDestination == null)
            {
                rfcDestination = RfcDestinationManager.GetDestination(WebApiApplication.destinationConfigName);
            }
            RfcRepository repo = rfcDestination.Repository;
            IRfcFunction _BusinessPartner = repo.CreateFunction("BAPI_BUPA_CREATE_FROM_DATA");
            IRfcFunction _Commit = repo.CreateFunction("BAPI_TRANSACTION_COMMIT");
            _BusinessPartner.SetValue("PARTNERCATEGORY", 2); //2:org
            _BusinessPartner.SetValue("PARTNERGROUP", 1000); //1000:buyer
            IRfcStructure _CentralData = _BusinessPartner.GetStructure("CENTRALDATA");
            _CentralData.SetValue("SEARCHTERM1", "測試國度");
            _CentralData.SetValue("SEARCHTERM2", "測試資料");
            // _CentralData.SetValue("PARTNERLANGUAGEISO", "EN");

            IRfcStructure _CentraldataOrganization = _BusinessPartner.GetStructure("CENTRALDATAORGANIZATION");
            _CentraldataOrganization.SetValue("NAME1", "LLLLL");
            IRfcStructure _Address = _BusinessPartner.GetStructure("ADDRESSDATA");
            _Address.SetValue("COUNTRY", "TW");
            //_Address.SetValue("LANGU", "EN");
            _Address.SetValue("REGION", "TPE");
            RfcSessionManager.BeginContext(rfcDestination);
            _BusinessPartner.Invoke(rfcDestination);
            _Commit.Invoke(rfcDestination);
            RfcSessionManager.EndContext(rfcDestination);

            string _BusinessPartnerNumber = _BusinessPartner.GetString("BUSINESSPARTNER");
            DataTable dtReturn = ConvertToDotNetTable(_BusinessPartner.GetTable("RETURN"));
            List<ErrorLog> _ErrorList = SetErrorLog(dtReturn, "BAPI_BUPA_CREATE_FROM_DATA", _BusinessPartnerNumber,"");
            List<string> _Error = this._errorLogService.MiltiCreate(_ErrorList);
            return _BusinessPartnerNumber;
        }
        /// <summary>
        /// no use
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string CreateCustomer(string number)
        {
            if (rfcDestination == null)
            {
                rfcDestination = RfcDestinationManager.GetDestination(WebApiApplication.destinationConfigName);
            }
            RfcRepository repo = rfcDestination.Repository;
            IRfcFunction _CUSTOMER = repo.CreateFunction("BAPI_CUSTOMER_CREATEFROMDATA1");
            IRfcFunction _Commit = repo.CreateFunction("BAPI_TRANSACTION_COMMIT");
            IRfcStructure _PI_COPYREFERENCE = _CUSTOMER.GetStructure("PI_COPYREFERENCE");
            _PI_COPYREFERENCE.SetValue("SALESORG", "1000");
            _PI_COPYREFERENCE.SetValue("DISTR_CHAN", "00");
            _PI_COPYREFERENCE.SetValue("DIVISION", "00");
            _PI_COPYREFERENCE.SetValue("REF_CUSTMR", "0010000003");

            IRfcStructure _PI_PERSONALDATA = _CUSTOMER.GetStructure("PI_PERSONALDATA");
            _PI_PERSONALDATA.SetValue("FIRSTNAME", "1234");
            _PI_PERSONALDATA.SetValue("LASTNAME", "DDDD");
            _PI_PERSONALDATA.SetValue("COUNTRY", "US");
            _PI_PERSONALDATA.SetValue("REGION", "AK");
            _PI_PERSONALDATA.SetValue("LANGU_P", "EN");
            _PI_PERSONALDATA.SetValue("CURRENCY", "USD");
            _PI_PERSONALDATA.SetValue("middlename", "john");
            _PI_PERSONALDATA.SetValue("date_birth", "19780101");
            _PI_PERSONALDATA.SetValue("langu_p", "EN");
            _PI_PERSONALDATA.SetValue("district", "Hyd");
            _PI_PERSONALDATA.SetValue("house_no", "11230");
            _PI_PERSONALDATA.SetValue("building", "super");
            _PI_PERSONALDATA.SetValue("room_no", "113");
            _PI_PERSONALDATA.SetValue("title_p", "Mr.");//Invalid form of address text error you need set Mr. reference table TSAD3T
            _PI_PERSONALDATA.SetValue("city", "LOS_ANGELES");
            //IRfcStructure _PI_OPT_PERSONALDATA = _CUSTOMER.GetStructure("PI_OPT_PERSONALDATA");
            //_PI_OPT_PERSONALDATA.SetValue("SHIP_COND", "01");
            //_PI_OPT_PERSONALDATA.SetValue("DELYG_PLNT", "1000");
            //IRfcFunction _SdCustomer = repo.CreateFunction("SD_CUSTOMER_MAINTAIN_ALL");
            //IRfcStructure _Knvv = _SdCustomer.GetStructure("I_KNVV");
            //_Knvv.SetValue("KUNNR", number);
            //_Knvv.SetValue("VKORG", "1000");
            //_Knvv.SetValue("VTWEG", "00");
            //_Knvv.SetValue("SPART", "00");
            //_Knvv.SetValue("KALKS", "1");
            //_Knvv.SetValue("BZIRK", "US");
            //_Knvv.SetValue("WAERS", "USD");
            //_Knvv.SetValue("KURST", "E");
            //_Knvv.SetValue("KONDA", "MSRP");
            //_Knvv.SetValue("VSBED", "01");
            //_Knvv.SetValue("INCO1", "DDP");
            //_Knvv.SetValue("INCO2", "US");
            RfcSessionManager.BeginContext(rfcDestination);
            _CUSTOMER.Invoke(rfcDestination);
            _Commit.Invoke(rfcDestination);
            RfcSessionManager.EndContext(rfcDestination);
            IRfcStructure structReturn = _CUSTOMER.GetStructure("RETURN");
            string _CUSTOMERNO = _CUSTOMER.GetString("CUSTOMERNO");
            string _TYPE = structReturn.GetString("TYPE");
            string _MESSAGE = structReturn.GetString("MESSAGE");
            DataTable dtReturn = setErrorTable(_TYPE, _MESSAGE, "PI_COPYREFERENCE");
            List<ErrorLog> _ErrorList = SetErrorLog(dtReturn, "BAPI_CUSTOMER_CREATEFROMDATA1", "","");
            List<string> _Error = this._errorLogService.MiltiCreate(_ErrorList);
            return "";
        }
        private DataTable setErrorTable(string type, string msg, string param)
        {
            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("TYPE");
            dtReturn.Columns.Add("PARAMETER");
            dtReturn.Columns.Add("MESSAGE");
            DataRow dr = dtReturn.NewRow();
            dr["TYPE"] = type;
            dr["PARAMETER"] = param;
            dr["MESSAGE"] = msg;
            dtReturn.Rows.Add(dr);
            return dtReturn;
        }
        /// <summary>
        /// 1. if no parameter :IV_DOCOMMIT then no effect
        /// 2. RUN_ID:if not same number then no effect 
        /// 3. IT_BP_GENERAL-OBJECT_TASK: insert mode  I 
        /// </summary> 
        /// <returns></returns>
        public string CreateBP()
        {
            if (rfcDestination == null)
            {
                rfcDestination = RfcDestinationManager.GetDestination(WebApiApplication.destinationConfigName);
            }
            RfcRepository repo = rfcDestination.Repository;
            IRfcFunction _CUSTOMER = repo.CreateFunction("RFC_CVI_EI_INBOUND_MAIN");
            _CUSTOMER.SetValue("IV_DOCOMMIT", "X"); //required
            //IRfcFunction _Commit = repo.CreateFunction("BAPI_TRANSACTION_COMMIT");
            IRfcTable _IT_BP_GENERAL = _CUSTOMER.GetTable("IT_BP_GENERAL");
            IRfcStructure _IT_BP_GENERALStruct = _IT_BP_GENERAL.Metadata.LineType.CreateStructure();
            _IT_BP_GENERALStruct.SetValue("RUN_ID", "1");
            _IT_BP_GENERALStruct.SetValue("OBJECT_TASK", "I");
            _IT_BP_GENERALStruct.SetValue("BPARTNER", "");
            _IT_BP_GENERALStruct.SetValue("NAME1", "LLLLL");
            _IT_BP_GENERALStruct.SetValue("CATEGORY", 2);//required
            _IT_BP_GENERALStruct.SetValue("GROUPING", "1000");
            _IT_BP_GENERALStruct.SetValue("SEARCHTERM1", "國度");
            _IT_BP_GENERALStruct.SetValue("SEARCHTERM2", "資料");
            _IT_BP_GENERALStruct.SetValue("FOUNDATIONDATE", DateTime.Now.ToString("yyyyMMdd"));
            _IT_BP_GENERALStruct.SetValue("TITLE_KEY", "");
            _IT_BP_GENERALStruct.SetValue("AUTHORIZATIONGROUP", "");
            _IT_BP_GENERALStruct.SetValue("PARTNEREXTERNAL", "");
            _IT_BP_GENERAL.Append(_IT_BP_GENERALStruct);
            IRfcTable _IT_BP_ROLE = _CUSTOMER.GetTable("IT_BP_ROLE");
            IRfcStructure _IT_BP_ROLEStruct = _IT_BP_ROLE.Metadata.LineType.CreateStructure();
            IRfcStructure _IT_BP_ROLEStruct2 = _IT_BP_ROLE.Metadata.LineType.CreateStructure();
            _IT_BP_ROLEStruct.SetValue("RUN_ID", "1"); //required
            _IT_BP_ROLEStruct.SetValue("DATA_KEY", "FLCU00");
            _IT_BP_ROLEStruct.SetValue("ROLECATEGORY", "2");
            _IT_BP_ROLE.Append(_IT_BP_ROLEStruct);
            _IT_BP_ROLEStruct2.SetValue("RUN_ID", "1");//required
            _IT_BP_ROLEStruct2.SetValue("DATA_KEY", "1000");
            _IT_BP_ROLEStruct2.SetValue("ROLECATEGORY", "2");
            _IT_BP_ROLE.Append(_IT_BP_ROLEStruct2);
            IRfcTable _IT_BP_ADDRESS = _CUSTOMER.GetTable("IT_BP_ADDRESS");
            IRfcStructure _IT_BP_ADDRESStruct = _IT_BP_ADDRESS.Metadata.LineType.CreateStructure();
            _IT_BP_ADDRESStruct.SetValue("RUN_ID", "1");//required
            _IT_BP_ADDRESStruct.SetValue("STANDARDADDRESS", "X");
            _IT_BP_ADDRESStruct.SetValue("COUNTRY", "US");
            _IT_BP_ADDRESStruct.SetValue("LANGUISO", "EN");
            _IT_BP_ADDRESStruct.SetValue("LANGU", "E");
            _IT_BP_ADDRESS.Append(_IT_BP_ADDRESStruct); 
            RfcSessionManager.BeginContext(rfcDestination);
            _CUSTOMER.Invoke(rfcDestination);
            //_Commit.Invoke(rfcDestination);
            RfcSessionManager.EndContext(rfcDestination);
            DataTable dtReturn = ConvertToDotNetTable(_CUSTOMER.GetTable("CT_RETURN"));
            List<ErrorLog> _ErrorList = new List<ErrorLog>();
            string _Number = "";
            if (dtReturn.Rows.Count > 0)
            {
                _Number = dtReturn.Rows[0]["OBJECT_KEY"].ToString() == "" ? "" : dtReturn.Rows[0]["OBJECT_KEY"].ToString();
                _ErrorList = SetErrorLog(dtReturn, "RFC_CVI_EI_INBOUND_MAIN", _Number,"");
            }
            List<string> _Error = this._errorLogService.MiltiCreate(_ErrorList);
            //test 
            //RfcRepository repo2 = rfcDestination.Repository;
            //IRfcFunction _CUSTOMER2 = repo2.CreateFunction("RFC_CVI_EI_INBOUND_MAIN");
            //_CUSTOMER2.SetValue("IV_DOCOMMIT", "X");  
            //IRfcFunction _Commit2 = repo.CreateFunction("BAPI_TRANSACTION_COMMIT");
            //IRfcTable _IT_BP_GENERAL2 = _CUSTOMER2.GetTable("IT_BP_GENERAL");
            //IRfcStructure _IT_BP_GENERAL2Struct = _IT_BP_GENERAL2.Metadata.LineType.CreateStructure();
            //_IT_BP_GENERAL2Struct.SetValue("RUN_ID", "1");
            //_IT_BP_GENERAL2Struct.SetValue("OBJECT_TASK", "U");
            //_IT_BP_GENERAL2Struct.SetValue("BPARTNER", "0010002183");
            //_IT_BP_GENERAL2Struct.SetValue("SEARCHTERM1", "國度1234");
            //_IT_BP_GENERAL2.Append(_IT_BP_GENERAL2Struct); 
            //RfcSessionManager.BeginContext(rfcDestination);
            //_CUSTOMER2.Invoke(rfcDestination);
            //_Commit2.Invoke(rfcDestination);
            //RfcSessionManager.EndContext(rfcDestination);
            //DataTable dtReturn2 = ConvertToDotNetTable(_CUSTOMER2.GetTable("CT_RETURN"));
            return _Number;
        }
        public string CreateSalesArea(string number)
        {
            if (rfcDestination == null)
            {
                rfcDestination = RfcDestinationManager.GetDestination(WebApiApplication.destinationConfigName);
            }
            RfcRepository repo = rfcDestination.Repository;
            IRfcFunction _CUSTOMER = repo.CreateFunction("RFC_CVI_EI_INBOUND_MAIN");
            _CUSTOMER.SetValue("IV_DOCOMMIT", "X");// required
           //IRfcFunction _Commit = repo.CreateFunction("BAPI_TRANSACTION_COMMIT");
            IRfcTable _IT_CUST_GENERAL = _CUSTOMER.GetTable("IT_CUST_GENERAL");
            IRfcStructure _IT_CUST_GENERALStruct = _IT_CUST_GENERAL.Metadata.LineType.CreateStructure();
            _IT_CUST_GENERALStruct.SetValue("RUN_ID", "2");
            _IT_CUST_GENERALStruct.SetValue("KUNNR", number);
            _IT_CUST_GENERALStruct.SetValue("OBJECT_TASK", "I");
            _IT_CUST_GENERALStruct.SetValue("BAHNE", "25");
            _IT_CUST_GENERAL.Append(_IT_CUST_GENERALStruct);

            IRfcTable _IT_CUST_SALES = _CUSTOMER.GetTable("IT_CUST_SALES");
            IRfcStructure _IT_CUST_SALESStruct = _IT_CUST_SALES.Metadata.LineType.CreateStructure();
            _IT_CUST_SALESStruct.SetValue("RUN_ID", "2");
            _IT_CUST_SALESStruct.SetValue("VKORG", "1000");
            _IT_CUST_SALESStruct.SetValue("VTWEG", "00");
            _IT_CUST_SALESStruct.SetValue("SPART", "00");
            _IT_CUST_SALESStruct.SetValue("KALKS", "1");
            _IT_CUST_SALESStruct.SetValue("VSBED", "01");
            _IT_CUST_SALESStruct.SetValue("WAERS", "USD");

            _IT_CUST_SALESStruct.SetValue("KDGRP", "02");
            _IT_CUST_SALESStruct.SetValue("KURST", "E");
            _IT_CUST_SALESStruct.SetValue("KONDA", "02");
            _IT_CUST_SALESStruct.SetValue("BZIRK", "US");
            _IT_CUST_SALESStruct.SetValue("INCO1", "EXW");
            _IT_CUST_SALESStruct.SetValue("INCO2_L", "EXW");
            _IT_CUST_SALES.Append(_IT_CUST_SALESStruct);

            IRfcTable _IT_CUST_TAX_INDICATOR = _CUSTOMER.GetTable("IT_CUST_TAX_INDICATOR");
            IRfcStructure _IT_CUST_TAX_INDICATORStruct = _IT_CUST_TAX_INDICATOR.Metadata.LineType.CreateStructure();
            _IT_CUST_TAX_INDICATORStruct.SetValue("RUN_ID", "2");
            _IT_CUST_TAX_INDICATORStruct.SetValue("ALAND", "CN");
            _IT_CUST_TAX_INDICATORStruct.SetValue("TATYP", "MWST");
            _IT_CUST_TAX_INDICATORStruct.SetValue("TAXKD", "A");
            _IT_CUST_TAX_INDICATOR.Append(_IT_CUST_TAX_INDICATORStruct);
            IRfcStructure _IT_CUST_TAX_INDICATORStruct2 = _IT_CUST_TAX_INDICATOR.Metadata.LineType.CreateStructure();
            _IT_CUST_TAX_INDICATORStruct2.SetValue("RUN_ID", "2");
            _IT_CUST_TAX_INDICATORStruct2.SetValue("ALAND", "TW");
            _IT_CUST_TAX_INDICATORStruct2.SetValue("TATYP", "MWST");
            _IT_CUST_TAX_INDICATORStruct2.SetValue("TAXKD", "6");
            _IT_CUST_TAX_INDICATOR.Append(_IT_CUST_TAX_INDICATORStruct2);

            IRfcTable _IT_CUST_SALES_FUNCTIONS = _CUSTOMER.GetTable("IT_CUST_SALES_FUNCTIONS");
            IRfcStructure _IT_CUST_SALES_FUNCTIONSStruct = _IT_CUST_SALES_FUNCTIONS.Metadata.LineType.CreateStructure();
            _IT_CUST_SALES_FUNCTIONSStruct.SetValue("RUN_ID", "2");
            _IT_CUST_SALES_FUNCTIONSStruct.SetValue("VKORG", "1000");
            _IT_CUST_SALES_FUNCTIONSStruct.SetValue("VTWEG", "00");
            _IT_CUST_SALES_FUNCTIONSStruct.SetValue("SPART", "00");
            _IT_CUST_SALES_FUNCTIONSStruct.SetValue("PARVW", "SE");
            _IT_CUST_SALES_FUNCTIONSStruct.SetValue("KNREF", "22");
            _IT_CUST_SALES_FUNCTIONS.Append(_IT_CUST_SALES_FUNCTIONSStruct);
            RfcSessionManager.BeginContext(rfcDestination);
            _CUSTOMER.Invoke(rfcDestination);
           // _Commit.Invoke(rfcDestination);
            RfcSessionManager.EndContext(rfcDestination);
            DataTable dtReturn = ConvertToDotNetTable(_CUSTOMER.GetTable("CT_RETURN"));
            List<ErrorLog> _ErrorList = new List<ErrorLog>();
            string _Number = "";
            if (dtReturn.Rows.Count > 0)
            {
                _Number = dtReturn.Rows[0]["OBJECT_KEY"].ToString() == "" ? "" : dtReturn.Rows[0]["OBJECT_KEY"].ToString();
                _ErrorList = SetErrorLog(dtReturn, "RFC_CVI_EI_INBOUND_MAIN", _Number,"");
            }
            return "";
        }
    }
}