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
using WebApi.DataModel.CustomModel.Vote;
namespace WebApi.Controllers.Vote
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VoteMappingController : ApiController
    {
        private IVoteService _voteService;
        public VoteMappingController(IVoteService voteService)
        {
            this._voteService = voteService;
        }
        /// <summary>
        /// insert vote data
        /// </summary>
        /// <param name="Id">login Id</param>
        /// <param name="className">which vote class</param>
        /// <param name="fK_Vote_ItemCatalogId">vote item</param>
        /// <returns></returns>
        public bool Post([FromBody] VoteList voteList)
        {
            return this._voteService.InsertVote_Mapping(voteList.Id, voteList.ClassName, voteList.FK_Vote_ItemCatalogIdList);
        }
        /// <summary>
        /// if you has voted return true and you cant vote any more
        /// </summary>
        /// <param name="Id">login id</param>
        /// <param name="className">which vote class</param>
        /// <returns></returns>
        public bool Get(int Id, string className)
        {
            return this._voteService.IsHasVote(Id, className);
        }
        //public List<int> Get(int Id, string className)
        //{
        //    return this._voteService.GetVote_ItemCatalogId(Id, className);
        //}
    }
}
