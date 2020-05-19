using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApi.BLL.Edi
{
    public interface IEdiAdapterFactory
    {
        EdiBase CreateEdiAdapter(string definition);
    }
}
