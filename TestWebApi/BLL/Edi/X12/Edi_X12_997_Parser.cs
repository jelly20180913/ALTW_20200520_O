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
    public class Edi_X12_997_Parser : EdiBase
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
            return "";
        }
        public override string Process(Edi_Customer c, string s, string _EdiBase)
        {
            string _Log = "";
            if (c.Mode == true) FtpFile.RemoveFile(c.RSSBus_PortId + "/Receive", _EdiBase, s);
            return _Log;
        } 
    }
}
