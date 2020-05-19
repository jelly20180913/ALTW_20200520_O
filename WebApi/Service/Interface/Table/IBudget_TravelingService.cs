using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IBudget_TravelingService
    {
        string Create(Budget_Traveling instance);

        void Update(Budget_Traveling instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Budget_Traveling GetByID(int Id);

        IEnumerable<Budget_Traveling> GetAll();
        List<string> MiltiCreate(List<Budget_Traveling> instance);

    }
}
