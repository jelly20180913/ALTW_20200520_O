using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApi.BLL.Edi
{
   public class EdiAdapterFactory:IEdiAdapterFactory
    {
        public EdiBase CreateEdiAdapter(string definition)
        {
            if (definition == "M_850"  )
                return new Edi_X12_850_Parser( );
            else if (definition == "M_997")
                return new Edi_X12_997_Parser();
            return new Edi_X12_850_Parser();
        }
    }
}
