using System.Collections.Generic;
using WebApi.DataModel.CustomModel.SAP;
namespace WebApi.Common.SapAdapter
{
    public class SapBase
    {
        public List<string> ListError = new List<string>();
        public string PriceType { get; set; }
        public virtual SapMiddleData Parse(string _FilePath)
        {
            SapMiddleData _SapMiddleData = new SapMiddleData();
            return _SapMiddleData;
        } 
    }
}