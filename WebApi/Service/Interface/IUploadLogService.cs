using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface
{
    public interface IUploadLogService
    {
        string Create(UploadLog instance);

        void Update(UploadLog instance);

        void Delete(int Id);

        bool IsExists(int Id);

        UploadLog GetByID(int Id);

        IEnumerable<UploadLog> GetAll();
        UploadLog GetByName(string name); 
    }
}
