using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWebApi.BLL.Edi; 
using EdiEngine.Runtime;
using WebApi.Models;
namespace TestWebApi.BLL.Edi
{
    public class EdiBase
    {
        public List<string> ListError = new List<string>();
        public virtual string Parse(EdiBatch b, Edi_Customer c, string _FilePath)
        {
            string _Log = "";
            return _Log;
        }
        public virtual string Process(Edi_Customer c, string s, string _EdiBase)
        {
            string _Log = "";
            return _Log;
        }
    }
}
