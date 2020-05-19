using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface IErrorLogService
    {
        void Create(ErrorLog instance);

        void Update(ErrorLog instance);

        void Delete(int Id);

        bool IsExists(int Id);

        ErrorLog GetByID(int Id);

        IEnumerable<ErrorLog> GetAll();
        List<string> MiltiCreate(List<ErrorLog> instance); 
    }
}
