using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.DataModel.CustomModel.Vote;
using WebApi.Service.Interface.Vote;
namespace WebApi.Controllers.Vote
{
    [JwtAuthActionFilter]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VoteResultController : ApiController
    {
        private IVoteService _voteService;
        public VoteResultController(IVoteService voteService)
        {
            this._voteService = voteService;
        }
        public List<VoteResult> Get(string className)
        {
            List<VoteResult> _VoteResultList = this._voteService.GetVoteResult(className);
            return _VoteResultList;
        }
        public List<VoteResult> Get( )
        {
            List<VoteResult> _VoteResultList = this._voteService.GetVoteResult("");
            return _VoteResultList;
        }
    }
}
