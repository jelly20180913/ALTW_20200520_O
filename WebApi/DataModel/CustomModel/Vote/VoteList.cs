using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DataModel.CustomModel.Vote
{
    public class VoteList
    {
        public string ClassName { get; set; }
        public int Id { get; set; }
        public List<int> FK_Vote_ItemCatalogIdList { get; set; }
    }
}