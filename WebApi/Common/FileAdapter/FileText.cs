using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using WebApi.Models;
using WebApi.Service.Interface.Table;
using WebApi.Service.Interface.Common;
namespace WebApi.Common.FileAdapter
{
    public class FileText : FileBase
    {
        private IPosOrderMappingService _posOrderMappingService;
        private ICountryService _countryService;
        private ICommonFileService _commonFileService;
         public FileText(IPosOrderMappingService posOrderMappingService, ICountryService countryService,ICommonFileService commonFileService)
       // public FileText(IPosOrderMappingService posOrderMappingService,   ICommonFileService commonFileService)
        {
            this._posOrderMappingService = posOrderMappingService;
            this._countryService = countryService;
            this._commonFileService = commonFileService;
        }
        public override IQueryable<PosData> Parse(int _Model, string filePath)
        {
            IQueryable<PosData> _PosList;
            int startLine = 4;//test
            int lineCount = File.ReadAllLines(filePath).Skip((startLine)).Count();
            var fileLines = File.ReadAllLines(filePath).Skip((startLine)).Take(lineCount).ToList();
            List<PosData> _PosDataList = new List<PosData>();
            foreach (string line in fileLines)
            {
                //replace "" characters
                string[] temp = line.Replace((char)34, (char)32).Trim().Split(',');
                if (temp.Count() >= 0)
                {
                    PosData _PosData = new PosData();
                    _PosData.Year = temp[12].Trim();
                    _PosData.MonthYear = temp[12].Trim();
                    _PosData.Distributor = temp[5].Trim();
                    _PosData.Customer = temp[3].Trim();
                    _PosData.ISOCountryCode = temp[6].Trim();
                    _PosData.Country = temp[6].Trim();
                    _PosData.SalesArea = temp[0].Trim();
                    _PosData.SalesManager = temp[0].Trim();
                    _PosData.City = temp[18].Trim();
                    _PosData.PostCode = temp[1].Trim();
                    _PosData.PartNo = temp[8].Trim();
                    _PosData.BaseCurrency = temp[13].Trim();
                    _PosData.Qty = temp[13].Trim().ToString() != "" ? Convert.ToInt32(temp[13].Trim()) : 0;
                    _PosData.TotalSalesBaseCurreny = temp[13].Trim().ToString() != "" ? Convert.ToDecimal(temp[13].Trim()) : 0;
                    _PosData.TotalSalesEUR = temp[14].Trim().ToString() != "" ? Convert.ToDecimal(temp[14].Trim()) : 0;
                    _PosData.ProductSeries = temp[11].Trim();
                    _PosData.UpdateTime = DateTime.Now;
                    _PosDataList.Add(_PosData);
                }
            }
            _PosList = _PosDataList.AsQueryable();
            return _PosList;
        }
        /// <summary>
        /// parse txt format into pos entity
        /// 1. get pos mapping order number
        /// 2. start from startLine
        /// 3. replace "" characters
        /// 4. by SplitChar 
        /// 5. put error message in parent list
        /// </summary>
        /// <param name="_Model"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public override IQueryable<Pos> ParsePos(int _Model, string filePath)
        {
            IQueryable<Pos> _PosList;
            List<Pos> _PosDataList = new List<Pos>();  
            PosOrderMapping _PosOrderMapping = _posOrderMappingService.GetByID(_Model); 
            int startLine = _PosOrderMapping.Start;
            int lineCount = File.ReadAllLines(filePath).Skip((startLine)).Count();
            var fileLines = File.ReadAllLines(filePath).Skip((startLine)).Take(lineCount).ToList(); 
            foreach (string line in fileLines)
            {
                try
                {
                    startLine++;
                    //replace "" characters
                    string _Line = line.Replace(_PosOrderMapping.SplitChar, "▲"); 
                    string[] temp = _Line.Replace((char)34, (char)32).Trim().Split('▲');
                    if (temp.Count() > 32)
                    {
                        Pos _Pos = new Pos();
                        _Pos.Address = _PosOrderMapping.Address != null ? temp[Convert.ToInt32(_PosOrderMapping.Address)].Trim() : "";
                        _Pos.City = _PosOrderMapping.City != null ? temp[Convert.ToInt32(_PosOrderMapping.City)].Trim() : "";
                        _Pos.State = _PosOrderMapping.State != null ? temp[Convert.ToInt32(_PosOrderMapping.State)].Trim() : "";
                        _Pos.ZIP = _PosOrderMapping.ZIP != null ? temp[Convert.ToInt32(_PosOrderMapping.ZIP)].Trim() : "";
                        _Pos.Series = _PosOrderMapping.Series != null ? temp[Convert.ToInt32(_PosOrderMapping.Series)].Trim() : "";
                        _Pos.PartNo = _PosOrderMapping.PartNo != null ? temp[Convert.ToInt32(_PosOrderMapping.PartNo)].Trim() : "";
                        if (_PosOrderMapping.Quantity != null)
                        {
                            _Pos.Quantity = temp[Convert.ToInt32(_PosOrderMapping.Quantity)].Trim().ToString() != "" ? Convert.ToInt32(temp[Convert.ToInt32(_PosOrderMapping.Quantity)].Trim()) : 0;
                        }
                        _Pos.InvoiceNo = _PosOrderMapping.InvoiceNo != null ? temp[Convert.ToInt32(_PosOrderMapping.InvoiceNo)].Trim() : "";
                        _Pos.InvoiceDate = _PosOrderMapping.InvoiceDate != null ? temp[Convert.ToInt32(_PosOrderMapping.InvoiceDate)].Trim() : "";
                        if (_PosOrderMapping.Cost != null)
                        {
                            _Pos.Cost = temp[Convert.ToInt32(_PosOrderMapping.Cost)].Trim().ToString() != "" ? Convert.ToDecimal(temp[Convert.ToInt32(_PosOrderMapping.Cost)].Trim()) : 0;
                        }
                        if (_PosOrderMapping.Price != null)
                        {
                            _Pos.Price = temp[Convert.ToInt32(_PosOrderMapping.Price)].Trim().ToString() != "" ? Convert.ToDecimal(temp[Convert.ToInt32(_PosOrderMapping.Price)].Trim()) : 0;
                        }
                        if (_PosOrderMapping.ResellingExt != null)
                        {
                            _Pos.ResellingExt = temp[Convert.ToInt32(_PosOrderMapping.ResellingExt)].Trim().ToString() != "" ? Convert.ToDecimal(temp[Convert.ToInt32(_PosOrderMapping.ResellingExt)].Trim()) : 0;
                        }
                        _Pos.ACCT = _PosOrderMapping.ACCT != null ? temp[Convert.ToInt32(_PosOrderMapping.ACCT)].Trim() : "";
                        _Pos.CustomerName = _PosOrderMapping.CustomerName != null ? temp[Convert.ToInt32(_PosOrderMapping.CustomerName)].Trim() : "";
                        _Pos.MarketCode = _PosOrderMapping.MarketCode != null ? temp[Convert.ToInt32(_PosOrderMapping.MarketCode)].Trim() : "";
                        _Pos.Market = _PosOrderMapping.Market != null ? temp[Convert.ToInt32(_PosOrderMapping.Market)].Trim() : "";
                        _Pos.SubSegmentCode = _PosOrderMapping.SubSegmentCode != null ? temp[Convert.ToInt32(_PosOrderMapping.SubSegmentCode)].Trim() : "";
                        _Pos.SubSegment = _PosOrderMapping.SubSegment != null ? temp[Convert.ToInt32(_PosOrderMapping.SubSegment)].Trim() : "";
                        _Pos.Remarks = _PosOrderMapping.Remarks != null ? temp[Convert.ToInt32(_PosOrderMapping.Remarks)].Trim() : "";
                        _Pos.CustomerPO = _PosOrderMapping.CustomerPO != null ? temp[Convert.ToInt32(_PosOrderMapping.CustomerPO)].Trim() : "";
                        _Pos.ShipDate = _PosOrderMapping.ShipDate != null ? temp[Convert.ToInt32(_PosOrderMapping.ShipDate)].Trim() : ""; 
                        if (_Pos.ShipDate != "")
                        {
                            _Pos.ShipDate2 = _Pos.ShipDate;
                           DateTime  _ShipDate2 = DateTime.ParseExact(_Pos.ShipDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                              string _Day = Convert.ToDateTime(_ShipDate2).Day.ToString(); 
                            _Pos.ShipMonth = Convert.ToDateTime(_ShipDate2).ToString("MMMM", new System.Globalization.CultureInfo("en-us")).Substring(0, 3)+"," + _Day;
                            _Pos.ShipQuarter =this._commonFileService.GetQuarter( Convert.ToDateTime(_ShipDate2).Month);
                          
                        } 
                        _Pos.CountryCode = _PosOrderMapping.CountryCode != null ? temp[Convert.ToInt32(_PosOrderMapping.CountryCode)].Trim() : "";
                        _Pos.Country = this._countryService.GetByCode(_Pos.CountryCode).EnglishName;
                        _Pos.Region = this._countryService.GetByCode(_Pos.CountryCode).Region;
                        _Pos.Status = "T";
                        _PosDataList.Add(_Pos);
                    }
                }
                catch (Exception ex)
                {
                   this.ListError.Add(" row : "+ startLine.ToString() + " ,txt row data has error format:" + ex.Message+"\r\n data:" + line);
                }
            } 
            _PosList = _PosDataList.AsQueryable();
            return _PosList;
        }
    }
}