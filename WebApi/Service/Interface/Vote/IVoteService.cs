using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataModel.CustomModel.Vote;
using WebApi.Models;
namespace WebApi.Service.Interface.Vote
{
  public  interface IVoteService
    {
        List<Vote_ItemCatalog> GetVote_ItemCatalogList(string className);
        bool InsertVote_Mapping(int Id, string className, List<int> fK_Vote_ItemCatalogId);
        bool IsHasVote(int Id, string className);
        List<VoteResult> GetVoteResult(string className);
        List<int> GetVote_ItemCatalogId(int Id, string className);
        bool UpdateVote_ItemCatalog(Vote_ItemCatalog vote_ItemCatalog);
    }
}
