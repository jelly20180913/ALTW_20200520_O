using System.Collections.Generic;
using WebApi.Models;
namespace WebApi.Service.Interface.Table
{
    public interface ICountryService
    {
        void Create(Country instance);

        void Update(Country instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Country GetByID(int Id);

        IEnumerable<Country> GetAll();
        Country GetByCode(string code);
    }
}
