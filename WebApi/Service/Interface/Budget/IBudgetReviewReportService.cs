using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Service.Implement.Table;


namespace WebApi.Service.Interface
{
   public interface IBudgetReviewReportService
    {
        Object GetReviewReport(string JsonStr);
    }
}
