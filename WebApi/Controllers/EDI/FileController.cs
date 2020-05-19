using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Service.Interface;
namespace WebApi.Controllers
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FileController : ApiController
    {
        private IFileService _fileService;

        public FileController(IFileService fileService)
        {
            this._fileService = fileService;
        }
        /// <summary>
        /// upload file 
        /// </summary>
        /// <returns></returns>
        public List<string> Post()
        { 
            return  this._fileService.UploadFile();
        }
    }
}
