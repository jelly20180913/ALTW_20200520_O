using System.Collections.Generic;
using WebApi.Models;
using System;

namespace WebApi.Service.Interface.Table
{
    public interface IVote_MappingService
    {
        string Create(Vote_Mapping instance);

        void Update(Vote_Mapping instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Vote_Mapping GetByID(int Id);

        IEnumerable<Vote_Mapping> GetAll();
        List<string> MiltiCreate(List<Vote_Mapping> instance);

    }
}
