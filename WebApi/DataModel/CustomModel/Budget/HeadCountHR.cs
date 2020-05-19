using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.DataModel.CustomModel.Budget
{
    public class HeadCountHR
    {
        public int Id { get; set; }
        public string DepartmentId { get; set; }
        public string Account { get; set; }
        public string AltwName { get; set; }
        public string DirectStaff { get; set; }
        public string  HR  { get; set; }
        public string PartTime { get; set; }
        public string  DirectType { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public string  JobFunction { get; set; }
        public string  Title { get; set; }
        public string DepartmentName { get; set; }
    }
}