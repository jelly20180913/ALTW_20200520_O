using WebApi.Service.Interface;
using WebApi.Service.Interface.Table;
using WebApi.Service.Interface.Common;
namespace WebApi.Common.FileAdapter
{
    public class FileAdapterFactory:IFileAdapterFactory
    {
        private IPosColumnMapService _posColumnMapService;
        private IPosOrderMappingService _posOrderMappingService;
        private ICountryService _countryService;
        private ICommonFileService _commonFileService;
            public FileAdapterFactory(  IPosColumnMapService posColumnMapService, IPosOrderMappingService posOrderMappingService,ICountryService countryService,ICommonFileService commonFileService) 
        { 
            this._posColumnMapService = posColumnMapService;
            this._posOrderMappingService = posOrderMappingService;
            this._countryService = countryService;
            this._commonFileService = commonFileService;
        }
        /// <summary>
        /// support txt/TXT,xlsx/XLSX other file format
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public   FileBase CreateFileAdapter(string ext )
        {
            if ((ext == ".xlsx") || (ext == ".XLSX"))
                return new FileExcel(this._posColumnMapService);
            else if  ((ext == ".txt") || (ext == ".TXT"))
                  return new FileText(this._posOrderMappingService,this._countryService,this._commonFileService); 
            else
                return new FileCSV(); 
        }

    }
}