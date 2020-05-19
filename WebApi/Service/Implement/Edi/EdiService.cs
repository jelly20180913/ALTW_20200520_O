using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Service.Interface;
using WebApi.Models;
using WebApi.Service.Interface.Table;
using WebApi.DataModel.CustomModel.SAP;
using WebApi.DataModel.CustomModel.SAP.SalesOrder;
using WebApi.DataModel.CustomModel.Edi;
using System.Configuration;
namespace WebApi.Service.Implement
{
    public class EdiService : IEdiService
    {
        private IEdi_CustomerService _edi_CustomerService;
        private IEdi_SalesHeaderService _edi_SalesHeaderService;
        private IEdi_SalesItemService _edi_SalesItemService;
        private IEdi_SalesScheduleService _edi_SalesScheduleService;
        private IEdi_SalesPartnerService _edi_SalesPartnerService;
        private IEdi_SalesHeader_855Service _edi_SalesHeader_855Service;
        private IEdi_SalesItem_855Service _edi_SalesItem_855Service;
        private IEdi_SalesSchedule_855Service _edi_SalesSchedule_855Service;
        public EdiService(IEdi_CustomerService edi_CustomerSerivice, IEdi_SalesHeaderService edi_SalesHeaderService, IEdi_SalesItemService edi_SalesItemService, IEdi_SalesScheduleService edi_SalesScheduleService, IEdi_SalesPartnerService _edi_SalesPartnerService, IEdi_SalesHeader_855Service _edi_SalesHeader_855Service, IEdi_SalesItem_855Service _edi_SalesItem_855Service, IEdi_SalesSchedule_855Service _edi_SalesSchedule_855Service)
        {
            this._edi_CustomerService = edi_CustomerSerivice;
            this._edi_SalesHeaderService = edi_SalesHeaderService;
            this._edi_SalesItemService = edi_SalesItemService;
            this._edi_SalesScheduleService = edi_SalesScheduleService;
            this._edi_SalesPartnerService = _edi_SalesPartnerService;
            this._edi_SalesHeader_855Service = _edi_SalesHeader_855Service;
            this._edi_SalesItem_855Service = _edi_SalesItem_855Service;
            this._edi_SalesSchedule_855Service = _edi_SalesSchedule_855Service;
        }
        public List<Edi_Customer> GetEdi_CustomerList(bool mode)
        {
            //string _EdiMode = ConfigurationManager.AppSettings["EdiMode"];
            //bool _Mode = Convert.ToBoolean(_EdiMode);
            List<Edi_Customer> _Edi_CustomerList = this._edi_CustomerService.GetAll().Where(x => x.Mode == mode).ToList();
            return _Edi_CustomerList;
        }
        /// <summary>
        /// insert 850 table 
        /// Edi_SalesHeader/Edi_SalesItem/Edi_SalesPartner/Edi_SalesSchedule
        /// Is_Proc:V success F fail
        /// CreateBy:Edi/Web
        /// </summary>
        /// <param name="sapSalesOrder"></param>
        /// <param name="orderNumber"></param>
        public void InsertSapSalesOrder(SapSalesOrder sapSalesOrder, string orderNumber)
        {
            Edi_SalesHeader _Edi_SalesHeader = new Edi_SalesHeader();
            _Edi_SalesHeader.DOC_TYPE = sapSalesOrder.Header.DOC_TYPE;
            _Edi_SalesHeader.SALES_ORG = sapSalesOrder.Header.SALES_ORG;
            _Edi_SalesHeader.DISTR_CHAN = sapSalesOrder.Header.DISTR_CHAN;
            _Edi_SalesHeader.DIVISION = sapSalesOrder.Header.DIVISION;
            _Edi_SalesHeader.PURCH_NO_C = sapSalesOrder.Header.PURCH_NO_C;
            _Edi_SalesHeader.PURCH_DATE = sapSalesOrder.Header.PURCH_DATE;
            _Edi_SalesHeader.Is_Proc = orderNumber != "" ? "V" : "F";
            _Edi_SalesHeader.DateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss:");
            _Edi_SalesHeader.OrderNumber = orderNumber;
            _Edi_SalesHeader.CreateBy = sapSalesOrder.CreateBy;
            this._edi_SalesHeaderService.Create(_Edi_SalesHeader);
            foreach (SalesItem si in sapSalesOrder.ItemList)
            {
                Edi_SalesItem _Edi_SalesItem = new Edi_SalesItem();
                _Edi_SalesItem.OrderNumber = orderNumber;
                _Edi_SalesItem.ITM_NUMBER = si.ITM_NUMBER;
                _Edi_SalesItem.MATERIAL = si.MATERIAL;
                _Edi_SalesItem.CUST_MAT35 = si.CUST_MAT35;
                _Edi_SalesItem.CustomerItemNumber = si.CustomerItemNumber;
                _Edi_SalesItem.CustomerUnit = si.CustomerUnit;
                _Edi_SalesItem.CustomerPrice = si.CustomerPrice;
                _Edi_SalesItem.DateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                _Edi_SalesItem.CustomerUnitOfPrice = si.CustomerUnitOfPrice;
                this._edi_SalesItemService.Create(_Edi_SalesItem);
            }
            foreach (SalesSchedule ss in sapSalesOrder.ScheduleList)
            {
                Edi_SalesSchedule _Edi_SalesSchedule = new Edi_SalesSchedule();
                _Edi_SalesSchedule.OrderNumber = orderNumber;
                _Edi_SalesSchedule.ITM_NUMBER = ss.ITM_NUMBER;
                _Edi_SalesSchedule.REQ_DATE = ss.REQ_DATE;
                _Edi_SalesSchedule.SCHED_LINE = ss.SCHED_LINE;
                _Edi_SalesSchedule.DateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                _Edi_SalesSchedule.REQ_QTY = ss.REQ_QTY;
                this._edi_SalesScheduleService.Create(_Edi_SalesSchedule);
            }
            foreach (SalesPartner sp in sapSalesOrder.PartnerList)
            {
                Edi_SalesPartner _Edi_SalesPartner = new Edi_SalesPartner();
                _Edi_SalesPartner.OrderNumber = orderNumber;
                _Edi_SalesPartner.PARTN_ROLE = sp.PARTN_ROLE;
                _Edi_SalesPartner.PARTN_NUMB = sp.PARTN_NUMB;
                _Edi_SalesPartner.STREET = sp.STREET;
                _Edi_SalesPartner.CITY = sp.CITY;
                _Edi_SalesPartner.REGION = sp.REGION;
                _Edi_SalesPartner.POSTL_CODE = sp.POSTL_CODE;
                _Edi_SalesPartner.COUNTRY = sp.COUNTRY;
                _Edi_SalesPartner.NAME = sp.NAME;
                _Edi_SalesPartner.DateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                _Edi_SalesPartner.TELEPHONE = sp.TELEPHONE;
                this._edi_SalesPartnerService.Create(_Edi_SalesPartner);
            }
        }
        public List<Edi_SalesHeader> GetEdi_SalesHeaderList()
        {
            List<Edi_SalesHeader> _Edi_SalesHeaderList = this._edi_SalesHeaderService.GetAll().Where(x => x.Is_Proc == "V" && x.CreateBy == "Edi").ToList();
            return _Edi_SalesHeaderList;
        }
        public List<Edi_SalesItem> GetEdi_SalesItemList(string orderNumber)
        {
            List<Edi_SalesItem> _Edi_SalesItemList = this._edi_SalesItemService.GetAll().Where(x => x.OrderNumber == orderNumber).ToList();
            return _Edi_SalesItemList;
        }
        public List<Edi_SalesSchedule> GetEdi_SalesScheduleList(string orderNumber)
        {
            List<Edi_SalesSchedule> _Edi_SalesScheduleList = this._edi_SalesScheduleService.GetAll().Where(x => x.OrderNumber == orderNumber).ToList();
            return _Edi_SalesScheduleList;
        }
        public void InsertEdi_SalesOrder(Edi_SalesOrder_855 edi_SalesOrder_855)
        {
            this._edi_SalesHeader_855Service.Create(edi_SalesOrder_855.Header);
            foreach (Edi_SalesItem_855 si in edi_SalesOrder_855.ItemList)
            {
                this._edi_SalesItem_855Service.Create(si);
            }
            foreach (Edi_SalesSchedule_855 ss in edi_SalesOrder_855.ScheduleList)
            {
                this._edi_SalesSchedule_855Service.Create(ss);
            }
        }
    }
}