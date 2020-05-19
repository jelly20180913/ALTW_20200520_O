using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DataModel.CustomModel.Vote
{
    public class VoteResult
    {
        public string Id { get; set; }
        public string ClassName { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public string Number { get; set; }
        public int VoteCount { get; set; }
        public string Voter { get; set; }
        public bool Important { get; set; }
    }
}