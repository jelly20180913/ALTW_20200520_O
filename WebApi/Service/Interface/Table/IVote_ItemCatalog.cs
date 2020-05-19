using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IVote_ItemCatalogService
    {
        string Create(Vote_ItemCatalog instance);

        void Update(Vote_ItemCatalog instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Vote_ItemCatalog GetByID(int Id);

        IEnumerable<Vote_ItemCatalog> GetAll();
        List<string> MiltiCreate(List<Vote_ItemCatalog> instance);

    }
}
