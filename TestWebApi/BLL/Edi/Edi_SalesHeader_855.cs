using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.DataModel.CustomModel.Edi;
using EdiEngine.Standards.X12_004010.Maps;
using EdiEngine;
using EdiEngine.Runtime;
using EdiEngine.Common.Definitions;
using System.Configuration;
using NLog;
namespace TestWebApi.BLL.Edi
{
    public class Edi_SalesHeader_855
    {
        public List<string> ListError = new List<string>();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public Edi_SalesOrder_855  LineItemStatus(Edi_SalesHeader edi_SalesHeader, List<SOrder> sorderList, List<Edi_SalesItem> edi_SalesItemList, List<Edi_SalesSchedule> edi_SalesScheduleList)
        {
            string _Log = "";
            string _LineItemStatus = "IA";
            string _ItemNumber = "";
            Edi_SalesOrder_855  _Edi_SalesOrder_855 = new  Edi_SalesOrder_855();
            try
            {
                List<WebApi.Models.Edi_SalesItem_855> _Edi_SalesItem_855List = new List<WebApi.Models.Edi_SalesItem_855>();
                List<WebApi.Models.Edi_SalesSchedule_855> _Edi_SalesSchedule_855List = new List<WebApi.Models.Edi_SalesSchedule_855>();
                WebApi.Models.Edi_SalesHeader_855 _Edi_SalesHeader_855 = new WebApi.Models.Edi_SalesHeader_855();
                _Edi_SalesHeader_855.PURCH_NO_C = edi_SalesHeader.PURCH_NO_C;
                _Edi_SalesHeader_855.PURCH_DATE = edi_SalesHeader.PURCH_DATE; 
                _Edi_SalesHeader_855.DateTime = DateTime.Now.ToString("yyyyMMdd");
                _Edi_SalesHeader_855.OrderNumber = edi_SalesHeader.OrderNumber;
                foreach (Edi_SalesItem s in edi_SalesItemList)
                {
                    List<Edi_SalesSchedule> _Edi_SalesScheduleList = edi_SalesScheduleList.Where(x => x.ITM_NUMBER == s.ITM_NUMBER).ToList();
                    _ItemNumber = s.ITM_NUMBER;
                    WebApi.Models.Edi_SalesItem_855 _Edi_SalesItem_855 = new WebApi.Models.Edi_SalesItem_855();
                    _Edi_SalesItem_855.OrderNumber = s.OrderNumber;
                    _Edi_SalesItem_855.CUST_MAT35 = s.CUST_MAT35;
                    _Edi_SalesItem_855.ITM_NUMBER = s.ITM_NUMBER;
                    _Edi_SalesItem_855.MATERIAL = s.MATERIAL;
                    _Edi_SalesItem_855.CustomerUnit = s.CustomerUnit;
                    _Edi_SalesItem_855.CustomerItemNumber = s.CustomerItemNumber;
                    _Edi_SalesItem_855.DateTime = DateTime.Now.ToString("yyyyMMdd");
                    _Edi_SalesItem_855.CustomerUnitOfPrice = s.CustomerUnitOfPrice; 
                    List <SOrder> _SOrderList = sorderList.Where(x => x.POSNR == s.ITM_NUMBER).ToList();
                    if (_SOrderList.Count > 0)
                    { 
                        _Edi_SalesItem_855.TARGET_QTY = Convert.ToString(_SOrderList.First().KWMENG);
                        _Edi_SalesItem_855.Price = Convert.ToString(Decimal.Round(_SOrderList.First().PRICE,6));
                        if (_SOrderList.First().LIFSP != "") _LineItemStatus = "IH";
                        if (_SOrderList.First().PRICE != Convert.ToDecimal(s.CustomerPrice)) _LineItemStatus = "IP";
                        if (_SOrderList.First().ABGRU != "") _LineItemStatus = "IR"; 
                        if (_Edi_SalesScheduleList.Count > 0)
                        {
                            if (_SOrderList.First().EDATU != _Edi_SalesScheduleList.First().REQ_DATE)
                                _LineItemStatus = "DR";
                            if (_SOrderList.First().LFSTA == "C") _LineItemStatus = "AC";
                            if(_SOrderList.First().KWMENG!=Convert.ToDecimal( _Edi_SalesScheduleList.First().REQ_QTY)) _LineItemStatus = "IQ";
                        }
                    }
                    else
                    {
                        _Edi_SalesItem_855.Price = s.CustomerPrice;
                        if (_Edi_SalesScheduleList.Count > 0)
                        {
                            _Edi_SalesItem_855.TARGET_QTY = _Edi_SalesScheduleList.First().REQ_QTY;
                        }
                        _LineItemStatus = "ID";
                    }
                    _Edi_SalesItem_855List.Add(_Edi_SalesItem_855);
                    WebApi.Models.Edi_SalesSchedule_855 _Edi_SalesSchedule_855 = new WebApi.Models.Edi_SalesSchedule_855();
                    _Edi_SalesSchedule_855.OrderNumber = s.OrderNumber;
                    _Edi_SalesSchedule_855.ITM_NUMBER = s.ITM_NUMBER;
                    _Edi_SalesSchedule_855.CustomerUnit = s.CustomerUnit;
                    _Edi_SalesSchedule_855.Status = _LineItemStatus;
                    _Edi_SalesSchedule_855.DateTime= DateTime.Now.ToString("yyyyMMdd");
                    _Edi_SalesSchedule_855.DateTimeCode = "017";
                    _Edi_SalesSchedule_855.REQ_DATE = _SOrderList.Count > 0?_SOrderList.First().EDATU: _Edi_SalesScheduleList.First().REQ_DATE;
                    _Edi_SalesSchedule_855.REQ_QTY = _SOrderList.Count > 0? Convert.ToString(_SOrderList.First().KWMENG) : _Edi_SalesScheduleList.First().REQ_QTY;
                    _Edi_SalesSchedule_855List.Add(_Edi_SalesSchedule_855);
                }
                string _HeaderStatus = "AC";
                if (_LineItemStatus == "IR") _HeaderStatus = "RD";
                else if (_LineItemStatus == "IA") _HeaderStatus = "AD"; 
                _Edi_SalesHeader_855.Status = _HeaderStatus;
                _Edi_SalesOrder_855.Header =_Edi_SalesHeader_855;
                _Edi_SalesOrder_855.ItemList = _Edi_SalesItem_855List;
                _Edi_SalesOrder_855.ScheduleList = _Edi_SalesSchedule_855List;
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")+ " Edi_SalesHeader_855:LineItemStatus: row : order:" + edi_SalesHeader.OrderNumber + " item:" + _ItemNumber + " , row data has error format:\r\n" + ex.Message ;
               this.ListError.Add(_Log);
                logger.Error(_Log);
            }
            return _Edi_SalesOrder_855;
        }
        public string X12_855(Edi_SalesOrder_855 sapSalesOrder, string mode, string receiver)
        {
            M_855 map = new M_855();
            EdiTrans t = new EdiTrans(map);

            var sDef = (MapSegment)map.Content.First(s => s.Name == "BAK");

            var seg = new EdiSegment(sDef);

            seg.Content.AddRange(new[]
             {
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[0], "00"),
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[1], sapSalesOrder.Header.Status),//AD:No change AC:with change RD:rejected 
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[2], sapSalesOrder.Header.PURCH_NO_C),//set 850 order number
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[3], sapSalesOrder.Header.PURCH_DATE),
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[7], sapSalesOrder.Header.OrderNumber),
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[8],  sapSalesOrder.Header.DateTime)
            });
            t.Content.Add(seg);
            //create segment 
            var lDef = (MapLoop)map.Content.First(s => s.Name == "L_PO1");
            sDef = (MapSegment)lDef.Content.First(s => s.Name == "PO1");

            EdiLoop p01 = new EdiLoop(lDef, null);
            t.Content.Add(p01);
            //get sap order price?
            foreach (Edi_SalesItem_855 i in sapSalesOrder.ItemList)
            {
                seg = new EdiSegment(sDef);
                seg.Content.AddRange(new[]
                {
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[0],i.CustomerItemNumber),
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[1],i.TARGET_QTY),
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[2],i.CustomerUnit),//ok
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[3],i.Price),
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[4],i.CustomerUnitOfPrice),//ok
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[5],"BP"),
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[6],i.CUST_MAT35),
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[7],"VP"),
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[8],i.MATERIAL)
                });
                p01.Content.Add(seg);

                var lDef_L_ACK = (MapLoop)lDef.Content.First(s => s.Name == "L_ACK");
                var sDef_ACK = (MapSegment)lDef_L_ACK.Content.First(s => s.Name == "ACK");

                EdiLoop w = new EdiLoop(lDef_L_ACK, p01);
                p01.Content.Add(w);
                foreach (Edi_SalesSchedule_855 j in sapSalesOrder.ScheduleList)
                {
                    if (i.ITM_NUMBER == j.ITM_NUMBER)
                    {
                        seg = new EdiSegment(sDef_ACK);
                        seg.Content.AddRange(new[]
                        {
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[0], j.Status),//IA:accept IR:reject
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[1], j.REQ_QTY),
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[2], i.CustomerUnit),//ok
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[2], j.DateTimeCode),
                new EdiSimpleDataElement((MapSimpleDataElement) sDef.Content[2], j.DateTime)
                    });
                        w.Content.Add(seg);
                    }
                }
            }
            string data = writeEdiEnvelope(t, "PR", mode, receiver);
            //read produced results and check for errors.
            EdiDataReader r = new EdiDataReader();
            EdiBatch batch = r.FromString(data);
            EdiTrans trans = batch.Interchanges[0].Groups[0].Transactions[0];
            return data;
        }
        private string writeEdiEnvelope(EdiTrans t, string functionalCode,string mode,string receiver)
        {
            //string _Sender = "84352008TEST";
            //string _Receiver = "8174836888";
            string _Sender = ConfigurationManager.AppSettings["Sender"]; 
            string _Receiver = receiver;
            string _Version = "00401";
            //create batch
            EdiBatch b = new EdiBatch();
            b.Interchanges.Add(new EdiInterchange());
            b.Interchanges.First().Groups.Add(new EdiGroup(functionalCode));
            b.Interchanges.First().Groups.First().Transactions.Add(t);

            //Add all service segments
            EdiDataWriterSettings settings = new EdiDataWriterSettings(
                new EdiEngine.Standards.X12_004010.Segments.ISA(), new EdiEngine.Standards.X12_004010.Segments.IEA(),
                new EdiEngine.Standards.X12_004010.Segments.GS(), new EdiEngine.Standards.X12_004010.Segments.GE(),
                new EdiEngine.Standards.X12_004010.Segments.ST(), new EdiEngine.Standards.X12_004010.Segments.SE(),
                  "ZZ", _Sender, "ZZ", _Receiver, _Sender, _Receiver,
              _Version, "004010", mode, 100, 200, "\r\n", "*");

            EdiDataWriter w = new EdiDataWriter(settings);
            return w.WriteToString(b);
        }
    }
}
