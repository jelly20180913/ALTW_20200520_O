using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface
{
    public interface IButtonLogService
    {
        string Create(ButtonLog instance);

        void Update(ButtonLog instance);

        void Delete(int Id);

        bool IsExists(int Id);

        ButtonLog GetByID(int Id);

        IEnumerable<ButtonLog> GetAll(); 
    }
}
