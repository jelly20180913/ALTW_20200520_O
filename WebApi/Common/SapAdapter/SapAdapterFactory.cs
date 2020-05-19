using WebApi.Service.Interface.Table; 
namespace WebApi.Common.SapAdapter
{
    public class SapAdapterFactory : ISapAdapterFactory
    { 
        public SapAdapterFactory( )
        { 
        }
        /// <summary> 
        /// </summary>
        /// <param name="type"></param> 
        /// <returns></returns>
        public SapBase CreateSapAdapter(string type)
        { 
            return new SapExcelPriceList();
        }

    }
}