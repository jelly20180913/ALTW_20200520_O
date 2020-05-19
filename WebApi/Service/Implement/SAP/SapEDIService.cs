using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Service.Interface;
using WebApi.Models;
using WebApi.Service.Interface.Table;
using Sap.Data.Hana;
using WebApi.DataModel.CustomModel.SAP;
using WebApi.Common;
using System.Data;
using WebApi.DataModel.CustomModel.SAP.SalesOrder;
using System.Reflection;
namespace WebApi.Service.Implement
{
    public class SapEDIService : ISapEDIService
    {
        private ISAP_PriceListService _sap_PriceListService;
        public SapEDIService(ISAP_PriceListService sap_PriceListService)
        {
            this._sap_PriceListService = sap_PriceListService;
        }
        public IEnumerable<SAP_PriceList> GetSAP_PriceLists(string date)
        {
            //DateTime _DateTime = date != "" ? DateTime.Parse(date) : DateTime.Now; 
            int _DateTime = date != "" && date != null ? Convert.ToInt32(date) : Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            IEnumerable<SAP_PriceList> _Sap_PriceList = this._sap_PriceListService.GetAll().Where(x => Convert.ToInt32(x.Date) > _DateTime).ToList();
            return _Sap_PriceList;
        }
        /// <summary>
        /// test
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SAP_PriceList> GetSAP_PriceListsExcludeTax()
        {
            IEnumerable<SAP_PriceList> _Sap_PriceList = this._sap_PriceListService.GetAll().ToList();
            return _Sap_PriceList;
        }
        public bool BatchInsert(SapMiddleData sapMiddleData)
        {
            List<string> _ListError = new List<string>();
            _ListError = miltiCreate(sapMiddleData);
            return _ListError.Count > 1 ? false : true;
        }
        private List<string> miltiCreate(SapMiddleData sapMiddleData)
        {
            string type = "";
            List<string> _ListError = new List<string>();
            switch (type)
            {
                default:
                    _ListError = this._sap_PriceListService.MiltiCreate(sapMiddleData.PriceList);
                    break;
            }
            return _ListError;
        }
        public bool Email(string ordernumber, SapSalesOrder sapSalesOrder)
        {
            string[] _Email = sapSalesOrder.SalesEmail.Split('@');
            string _Body = "Hello " + _Email[0] + "<br>order:" + Function.SetFontColor(ordernumber, "blue") + " has created <br>";
            List<SalesHeader> _HeaderList = new List<SalesHeader>();
            _HeaderList.Add(sapSalesOrder.Header);
            DataTable dtHeader = Function.ListToDataTable(_HeaderList);
            DataTable dtItem = Function.ListToDataTable(sapSalesOrder.ItemList);
            DataTable dtPartner = Function.ListToDataTable(sapSalesOrder.PartnerList);
            DataTable dtSchedule = Function.ListToDataTable(sapSalesOrder.ScheduleList);
            string _BodyTable = Function.RenderDataTableToHtml(dtHeader) + Function.RenderDataTableToHtml(dtItem) + Function.RenderDataTableToHtml(dtPartner) + Function.RenderDataTableToHtml(dtSchedule);
            _Body = _Body + _BodyTable;
            bool _Success = Function.SendEmail("EDI data-" + sapSalesOrder.CustomerName, _Body, "IT001@ltw-tech.com", sapSalesOrder.SalesEmail);
            return _Success;
        }

    }
}