using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Service.Interface;
using WebApi.Models;
using System.Data;
using WebApi.Service.Interface.Table;
using WebApi.Service.Interface.Vote;
using WebApi.DataModel.CustomModel.Vote;
namespace WebApi.Service.Implement
{
    public class VoteService : IVoteService
    {
        private ILoginService _loginService;
        private IVote_ItemCatalogService _vote_ItemCatalogService;
        private IVote_MappingService _vote_MappingService;
        /// <summary>
        /// Dependence Injection
        /// </summary>

        public VoteService(ILoginService loginService, IVote_ItemCatalogService vote_ItemCatalogService, IVote_MappingService vote_MappingService)
        {
            this._loginService = loginService;
            this._vote_ItemCatalogService = vote_ItemCatalogService;
            this._vote_MappingService = vote_MappingService;
        }
        /// <summary>
        /// get vote item list
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public List<Vote_ItemCatalog> GetVote_ItemCatalogList(string className)
        {
            List<Vote_ItemCatalog> _Vote_ItemCatalogList = this._vote_ItemCatalogService.GetAll().Where(x => x.ClassName == className).OrderBy(x=>x.Name).ToList();
            return _Vote_ItemCatalogList;
        }
        /// <summary>
        /// insert vote data
        /// </summary>
        /// <param name="Id">login Id</param>
        /// <param name="className">which vote class</param>
        /// <param name="fK_Vote_ItemCatalogId">vote item</param>
        /// <returns></returns>
        public bool InsertVote_Mapping(int Id, string className, List<int> fK_Vote_ItemCatalogId)
        {
            bool _Success = false;
            foreach (int v in fK_Vote_ItemCatalogId)
            {
                Vote_Mapping _Vote_Mapping = new Vote_Mapping();
                _Vote_Mapping.FK_Vote_ItemCatalogId = v;
                _Vote_Mapping.FK_LoginId = Id;
                _Vote_Mapping.ClassName = className;
                this._vote_MappingService.Create(_Vote_Mapping);
            }
            _Success = true;
            return _Success;
        }
        /// <summary>
        /// if you has voted return true and you cant vote any more
        /// </summary>
        /// <param name="Id">login id</param>
        /// <param name="className">which vote class</param>
        /// <returns></returns>
        public bool IsHasVote(int Id, string className)
        {
            bool _IsHas = this._vote_MappingService.GetAll().Where(x => x.FK_LoginId == Id && x.ClassName == className).Count() > 0 ? true : false;
            return _IsHas;
        }
        /// <summary>
        /// display vote result 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public List<VoteResult> GetVoteResult(string className)
        {
            List<VoteResult> _VoteResultList = new List<VoteResult>();
            List<Vote_ItemCatalog> _Vote_ItemCatalogList = className!=""?this._vote_ItemCatalogService.GetAll().Where(x => x.ClassName == className).OrderBy(x => x.Name).ToList(): this._vote_ItemCatalogService.GetAll().OrderBy(x=>x.Name).ToList();
            List<Vote_Mapping> _Vote_MappingList = className != "" ? this._vote_MappingService.GetAll().Where(x => x.ClassName == className).ToList() : this._vote_MappingService.GetAll().ToList(); ;
            List<Login> _LoginList = this._loginService.GetAll().ToList();
            foreach (var r in _Vote_ItemCatalogList)
            {
                VoteResult _VoteResult = new VoteResult();
                _VoteResult.Id = r.Id.ToString();
                _VoteResult.ClassName = r.ClassName;
                _VoteResult.Name = r.Name;
                _VoteResult.Number = r.Number.ToString();
                _VoteResult.Provider = r.Provider;
                if (_Vote_MappingList.Where(x => x.FK_Vote_ItemCatalogId == r.Id) != null)
                {
                    _VoteResult.VoteCount = _Vote_MappingList.Where(x => x.FK_Vote_ItemCatalogId == r.Id).Count();
                    foreach (Vote_Mapping v in _Vote_MappingList.Where(x => x.FK_Vote_ItemCatalogId == r.Id).ToList())
                    {
                        string _Name = _LoginList.Where(x => x.Id == v.FK_LoginId).First().CustomerName;
                        _VoteResult.Voter = _VoteResult.Voter + _Name + ",";
                    }
                    if (_VoteResult.Voter != null) _VoteResult.Voter = _VoteResult.Voter.Remove(_VoteResult.Voter.LastIndexOf(","), 1);
                }
                else
                {
                    _VoteResult.VoteCount = 0;
                    _VoteResult.Voter = "";
                }
                _VoteResultList.Add(_VoteResult);
            }
            //high vote show yellow backcolor and same vote count show yellow too
            if (_VoteResultList.Where(x => Convert.ToInt32(x.VoteCount) > 0).Count() > 0)
            {
                _VoteResultList.OrderByDescending(x => x.VoteCount).First().Important = true;
                int _ImportantVote = _VoteResultList.OrderByDescending(x => x.VoteCount).First().VoteCount;
                foreach (VoteResult v in _VoteResultList.Where(x => x.VoteCount == _ImportantVote).ToList())
                {
                    v.Important = true;
                }
            }
            return _VoteResultList;
        }
        /// <summary>
        /// no use
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public List<int> GetVote_ItemCatalogId(int Id, string className)
        {
            List<int> _IdList = new List<int>();
            List<Vote_Mapping> _Vote_MappingList = this._vote_MappingService.GetAll().Where(x => x.FK_LoginId == Id && x.ClassName == className) != null ? this._vote_MappingService.GetAll().Where(x => x.FK_LoginId == Id && x.ClassName == className).ToList() : null;
            foreach (Vote_Mapping v in _Vote_MappingList)
            {
                _IdList.Add(Convert.ToInt32(v.FK_Vote_ItemCatalogId));
            }
            return _IdList;
        }
        public bool UpdateVote_ItemCatalog(Vote_ItemCatalog vote_ItemCatalog)
        {
            bool _Success = false;
            Vote_ItemCatalog _Vote_ItemCatalog = this._vote_ItemCatalogService.GetByID(vote_ItemCatalog.Id);
            _Vote_ItemCatalog.Name = vote_ItemCatalog.Name;
            this._vote_ItemCatalogService.Update(_Vote_ItemCatalog);
            _Success = true;
            return _Success;
        }
    }
}