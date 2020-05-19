using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Service.Interface; 
using System.Web.Http.Cors;
using WebApi.Service.Interface.Vote;
using WebApi.Service.Implement;
using WebApi.Models;
namespace WebApi.Controllers.Vote
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VoteItemCatalogController : ApiController
    {
        private IVoteService _voteService;
        public VoteItemCatalogController(IVoteService voteService)
        {
            this._voteService = voteService;
        }
        public List<Vote_ItemCatalog> Get(string className)
        {
            List<Vote_ItemCatalog> _Vote_ItemCatalogList = this._voteService.GetVote_ItemCatalogList(className);
            return _Vote_ItemCatalogList;
        }
        public bool Put(Vote_ItemCatalog vote_ItemCatalog)
        {
            bool _OK = false;
            this._voteService.UpdateVote_ItemCatalog(vote_ItemCatalog); 
            _OK = true;
            return _OK;
        }

    }
}