using System;
using System.Collections.Generic;
using System.Web;
using WebApi.Service.Interface;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models.Repository.EDI.Implement;
using WebApi.Models;
using System.Data;
using WebApi.Service.Interface.Table;
using WebApi.Service.Implement.Table;
using WebApi.Service.Interface.Common;
using Newtonsoft.Json;
using WebApi.Controllers.Budget;
using System.Linq;


namespace WebApi.Service.Implement
{

    public class BudgetReviewReportService : IBudgetReviewReportService
    {
        private IRepository<Budget_DeptExpense> _repo_de;
        private IRepository<Budget_DepartmentReport> _repo_dr;
        private IRepository<Budget_FileVersionBudget> _repo_fv;
        private IRepository<Budget_Cost> _repo_cs;
        private IRepository<Budget_LoginReviewCommonCost> _repo_lc;
        private IRepository<Budget_Capex> _repo_cp;
        private IRepository<Budget_CostTravelingMapping> _repo_cm;
        private IRepository<Budget_Scrap> _repo_sp;
        private IRepository<ItemCatalog> _repo_ic;
        private IRepository<Budget_Traveling> _repo_tg;
        private IRepository<Budget_HeadCountHR> _repo_hr;
        private IRepository<Budget_DeptKPI> _repo_ki;
        private IRepository<Budget_HeadCount> _repo_hc;

        public BudgetReviewReportService(IRepository<Budget_DeptExpense> repo_de, IRepository<Budget_DepartmentReport> repo_dr, IRepository<Budget_FileVersionBudget> repo_fv, IRepository<Budget_Cost> repo_cs, IRepository<Budget_LoginReviewCommonCost> repo_lc, IRepository<Budget_Capex> repo_cp, IRepository<Budget_CostTravelingMapping> repo_cm, IRepository<Budget_Scrap> repo_sp, IRepository<ItemCatalog> repo_ic, IRepository<Budget_Traveling> repo_tg, IRepository<Budget_HeadCountHR> repo_hr, IRepository<Budget_DeptKPI> repo_ki, IRepository<Budget_HeadCount> repo_hc)
        {
            this._repo_de = repo_de;
            this._repo_dr = repo_dr;
            this._repo_fv = repo_fv;
            this._repo_cs = repo_cs;
            this._repo_lc = repo_lc;
            this._repo_cp = repo_cp;
            this._repo_cm = repo_cm;
            this._repo_sp = repo_sp;
            this._repo_ic = repo_ic;
            this._repo_tg = repo_tg;
            this._repo_hr = repo_hr;
            this._repo_ki = repo_ki;
            this._repo_hc = repo_hc;
        }

        public Object GetDeptExpenseReportData(string JsonStr)
        {
            MyQueryJson QueryJson = JsonConvert.DeserializeObject<MyQueryJson>(JsonStr);

            var rp = _repo_de.GetAll();
            var dr = _repo_dr.GetAll();
            var fv = _repo_fv.GetAll();
            var cs = _repo_cs.GetAll();
            var lc = _repo_lc.GetAll().Where(x => x.Account == QueryJson.Account);
            var cp = _repo_cp.GetAll();
            var cm = _repo_cm.GetAll();

            bool crossbranch = false;
            if (QueryJson.LTDept.Length > 0 && QueryJson.KSZDept.Length > 0)
            {
                crossbranch = true;
                rp = _repo_de.GetAll().Where(x => ((x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId)) || (x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId))) && (x.CostCode != "6136010000" || x.ItemId_CostCommon != "00120005"));
                cp = _repo_cp.GetAll().Where(x => (x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId)) || (x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId)));

            }
            else
            {
                if (QueryJson.LTDept.Length > 0)
                {
                    rp = _repo_de.GetAll().Where(x => (x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId)) && (x.ItemId_CostCommon != "00120005" || x.CostCode != "6136010000"));
                    cp = _repo_cp.GetAll().Where(x => x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId));
                }
                if (QueryJson.KSZDept.Length > 0)
                {
                    rp = _repo_de.GetAll().Where(x => (x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId)) && (x.CostCode != "6136010000" || x.ItemId_CostCommon != "00120005"));
                    cp = _repo_cp.GetAll().Where(x => x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId));
                }
            }

            fv = _repo_fv.GetAll().Where(x => (x.Date == QueryJson.BudgetYear && x.ItemId_BudgetName == "DeptExpense"));
            var fvcp = _repo_fv.GetAll().Where(x => (x.Date == QueryJson.BudgetYear && x.ItemId_BudgetName == "Capex"));
            var fvtr = _repo_fv.GetAll().Where(x => (x.Date == QueryJson.BudgetYear && x.ItemId_BudgetName == "Travelling"));
            var fvc = _repo_fv.GetAll().Where(x => (x.Date == QueryJson.BudgetYear));


            var cpv = (from cpm in cp
                       where cpm.Date == QueryJson.BudgetYear
                       join fvcpm in fvcp on new { cpm.Factory, cpm.DepartmentId, cpm.Version } equals new { fvcpm.Factory, fvcpm.DepartmentId, fvcpm.Version }
                       select cpm
                     );
            var trv = (from rpm in rp
                       where rpm.Date == QueryJson.BudgetYear
                       join fvtrm in fvtr on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvtrm.Factory, fvtrm.DepartmentId, fvtrm.Version }
                       select rpm
                     );

            var mg = (from rpnm in rp
                      where rpnm.ItemId_CostCommon == "00120000" && (rpnm.CostCode != "6107010000" && rpnm.CostCode != "6107020000" && rpnm.CostCode != "6107030000" && rpnm.CostCode != "6107040000")
                      join fvm in fv on new { rpnm.Factory, rpnm.DepartmentId, rpnm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                      select new
                      {
                          Date = rpnm.Date,
                          Factory = rpnm.Factory,
                          DepartmentId = rpnm.DepartmentId,
                          CostCode = rpnm.CostCode,
                          ItemId_CostCommon = rpnm.ItemId_CostCommon,
                          YTD = rpnm.YTD,
                          Jan = rpnm.Jan,
                          Feb = rpnm.Feb,
                          Mar = rpnm.Mar,
                          Apr = rpnm.Apr,
                          May = rpnm.May,
                          Jun = rpnm.Jun,
                          Jul = rpnm.Jul,
                          Aug = rpnm.Aug,
                          Sep = rpnm.Sep,
                          Oct = rpnm.Oct,
                          Nov = rpnm.Nov,
                          Dec = rpnm.Dec
                      })
                    .Union
                    (from rpnm in rp
                     where rpnm.ItemId_CostCommon == "00120000" && (rpnm.CostCode == "6107010000" || rpnm.CostCode == "6107020000" || rpnm.CostCode == "6107030000" || rpnm.CostCode == "6107040000")
                     join trvm in trv on new { rpnm.Factory, rpnm.DepartmentId, rpnm.Version } equals new { trvm.Factory, trvm.DepartmentId, trvm.Version }
                     select new
                     {
                         Date = rpnm.Date,
                         Factory = rpnm.Factory,
                         DepartmentId = rpnm.DepartmentId,
                         CostCode = rpnm.CostCode,
                         ItemId_CostCommon = rpnm.ItemId_CostCommon,
                         YTD = rpnm.YTD,
                         Jan = rpnm.Jan,
                         Feb = rpnm.Feb,
                         Mar = rpnm.Mar,
                         Apr = rpnm.Apr,
                         May = rpnm.May,
                         Jun = rpnm.Jun,
                         Jul = rpnm.Jul,
                         Aug = rpnm.Aug,
                         Sep = rpnm.Sep,
                         Oct = rpnm.Oct,
                         Nov = rpnm.Nov,
                         Dec = rpnm.Dec
                     })
                    .Union
                    (from rpcm in rp
                     where rpcm.ItemId_CostCommon != "00120000"
                     join fvcm in fvc on new { rpcm.Factory, rpcm.Version, cc = rpcm.ItemId_CostCommon } equals new { fvcm.Factory, fvcm.Version, cc = fvcm.ItemId_BudgetName }
                     join lcm in lc on new { rpcm.Factory, rpcm.ItemId_CostCommon } equals new { lcm.Factory, lcm.ItemId_CostCommon }
                     select new
                     {
                         Date = rpcm.Date,
                         Factory = rpcm.Factory,
                         DepartmentId = rpcm.DepartmentId,
                         CostCode = rpcm.CostCode,
                         ItemId_CostCommon = rpcm.ItemId_CostCommon,
                         YTD = rpcm.YTD,
                         Jan = rpcm.Jan,
                         Feb = rpcm.Feb,
                         Mar = rpcm.Mar,
                         Apr = rpcm.Apr,
                         May = rpcm.May,
                         Jun = rpcm.Jun,
                         Jul = rpcm.Jul,
                         Aug = rpcm.Aug,
                         Sep = rpcm.Sep,
                         Oct = rpcm.Oct,
                         Nov = rpcm.Nov,
                         Dec = rpcm.Dec
                     })
                    .Union
                    (from cpvm in cpv
                     where cpvm.ItemId_AssetExpType == "00040004" || cpvm.ItemId_AssetExpType == "00040010" || cpvm.ItemId_AssetExpType == "00040011" || cpvm.ItemId_AssetExpType == "00040012" || cpvm.ItemId_AssetExpType == "00040013" || cpvm.ItemId_AssetExpType == "00040014" || cpvm.ItemId_AssetExpType == "00040015"
                     join cmm in cm on cpvm.ItemId_AssetExpType equals cmm.ItemId_TravelingType
                     select new
                     {
                         Date = cpvm.Date,
                         Factory = cpvm.Factory,
                         DepartmentId = cpvm.DepartmentId,
                         CostCode = cmm.CostCode,
                         ItemId_CostCommon = "00120000",
                         YTD = cpvm.JulLast,
                         Jan = cpvm.Jan,
                         Feb = cpvm.Feb,
                         Mar = cpvm.Mar,
                         Apr = cpvm.Apr,
                         May = cpvm.May,
                         Jun = cpvm.Jun,
                         Jul = cpvm.Jul,
                         Aug = cpvm.Aug,
                         Sep = cpvm.Sep,
                         Oct = cpvm.Oct,
                         Nov = cpvm.Nov,
                         Dec = cpvm.Dec
                     });

            int mv = 5;

            var ReportDataAll = (from rpm in mg
                              where rpm.Date == QueryJson.BudgetYear
                              join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                              join csm in cs on new { rpm.CostCode, rpm.ItemId_CostCommon } equals new { csm.CostCode, csm.ItemId_CostCommon }
                              group new { rpm, urm, csm } by new { rpm.Factory, rpm.DepartmentId, urm.DepartmentName, rpm.CostCode, csm.CostName } into g
                              orderby new { g.Key.Factory, g.Key.DepartmentId, g.Key.CostCode }
                              select new
                              {
                                  Factory = g.Key.Factory ?? "",
                                  DeptCode = g.Key.DepartmentId ?? "",
                                  DeptName = g.Key.DepartmentName ?? "",
                                  CostCode = g.Key.CostCode ?? "",
                                  Desc = g.Key.CostName ?? "",
                                  YTDJuly = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(x => x.rpm.YTD ?? 0) * mv : g.Sum(x => x.rpm.YTD ?? 0),
                                  JanBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(x => x.rpm.Jan ?? 0) * mv : g.Sum(x => x.rpm.Jan ?? 0),
                                  FebBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(x => x.rpm.Feb ?? 0) * mv : g.Sum(x => x.rpm.Feb ?? 0),
                                  MarBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(x => x.rpm.Mar ?? 0) * mv : g.Sum(x => x.rpm.Mar ?? 0),
                                  AprBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(x => x.rpm.Apr ?? 0) * mv : g.Sum(x => x.rpm.Apr ?? 0),
                                  MayBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(x => x.rpm.May ?? 0) * mv : g.Sum(x => x.rpm.May ?? 0),
                                  JunBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(x => x.rpm.Jun ?? 0) * mv : g.Sum(x => x.rpm.Jun ?? 0),
                                  JulBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(x => x.rpm.Jul ?? 0) * mv : g.Sum(x => x.rpm.Jul ?? 0),
                                  AugBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(x => x.rpm.Aug ?? 0) * mv : g.Sum(x => x.rpm.Aug ?? 0),
                                  SepBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(x => x.rpm.Sep ?? 0) * mv : g.Sum(x => x.rpm.Sep ?? 0),
                                  OctBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(x => x.rpm.Oct ?? 0) * mv : g.Sum(x => x.rpm.Oct ?? 0),
                                  NovBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(x => x.rpm.Nov ?? 0) * mv : g.Sum(x => x.rpm.Nov ?? 0),
                                  DecBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(x => x.rpm.Dec ?? 0) * mv : g.Sum(x => x.rpm.Dec ?? 0)
                              });

            //var ReportData = (from rpm in ReportDataAll
            //                  where rpm.JanBud != 0 || rpm.FebBud != 0 || rpm.MarBud != 0 || rpm.AprBud != 0 || rpm.MayBud != 0 || rpm.JunBud != 0
            //                     || rpm.JulBud != 0 || rpm.AugBud != 0 || rpm.SepBud != 0 || rpm.OctBud != 0 || rpm.NovBud != 0 || rpm.DecBud != 0
            //                  select rpm);
            return ReportDataAll;

        }

        public Object GetScrapReportData(string JsonStr)
        {
            MyQueryJson QueryJson = JsonConvert.DeserializeObject<MyQueryJson>(JsonStr);

            var rp = _repo_sp.GetAll();
            var dr = _repo_dr.GetAll();
            var fv = _repo_fv.GetAll();
            var ic = _repo_ic.GetAll();

            bool crossbranch = false;
            if (QueryJson.LTDept.Length > 0 && QueryJson.KSZDept.Length > 0)
            {
                crossbranch = true;
                rp = _repo_sp.GetAll().Where(x => (x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId)) || (x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId)));
            }
            else
            {
                if (QueryJson.LTDept.Length > 0)
                    rp = _repo_sp.GetAll().Where(x => x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId));
                if (QueryJson.KSZDept.Length > 0)
                    rp = _repo_sp.GetAll().Where(x => x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId));
            }

            fv = _repo_fv.GetAll().Where(x => (x.Date == QueryJson.BudgetYear && x.ItemId_BudgetName == "Scrap"));
            int mv = 5;
            var ReportData = (from rpm in rp
                              join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                              join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                              join icm in ic on rpm.ItemId_ScrapType equals icm.ItemId
                              where rpm.Date == QueryJson.BudgetYear
                              orderby new { rpm.Factory, urm.DepartmentId }
                              select new
                              {
                                  Factory = rpm.Factory,
                                  DepartmentName = urm.DepartmentName,
                                  ScrapType = icm.Name ?? "",
                                  PN = rpm.PartNumber ?? "",
                                  Reason = rpm.Reason ?? "",
                                  Month = rpm.Month ?? "",
                                  QTY = rpm.Quantity ?? 0,
                                  OriginalCost = (crossbranch && rpm.Factory == "KSZ") ? (rpm.PurchasePrice ?? 0) * mv : (rpm.PurchasePrice ?? 0),
                                  SellingAmount = (crossbranch && rpm.Factory == "KSZ") ? (rpm.BookValue ?? 0) * mv : (rpm.BookValue ?? 0)
                              });
            return ReportData;

        }

        public Object GetTravelingReportData(string JsonStr)
        {
            MyQueryJson QueryJson = JsonConvert.DeserializeObject<MyQueryJson>(JsonStr);

            var rp = _repo_tg.GetAll();
            var dr = _repo_dr.GetAll();
            var fv = _repo_fv.GetAll();
            var hr = _repo_hr.GetAll().GroupBy(x => new { x.Account, x.AltwName });
            var ic = _repo_ic.GetAll().Where(x => x.ClassName == "TravelingPurpose");
            var ic1 = _repo_ic.GetAll().Where(x => x.ClassName == "TravelingType");

            bool crossbranch = false;
            if (QueryJson.LTDept.Length > 0 && QueryJson.KSZDept.Length > 0)
            {
                crossbranch = true;
                rp = _repo_tg.GetAll().Where(x => (x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId)) || (x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId)));
            }
            else
            {
                if (QueryJson.LTDept.Length > 0)
                    rp = _repo_tg.GetAll().Where(x => x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId));
                if (QueryJson.KSZDept.Length > 0)
                    rp = _repo_tg.GetAll().Where(x => x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId));
            }
            fv = _repo_fv.GetAll().Where(x => (x.Date == QueryJson.BudgetYear && x.ItemId_BudgetName == "Travelling"));

            Object ReportData = null;
            int mv = 5;

            switch (QueryJson.QueryGroupby)
            {
                case "0":
                    ReportData = (from rpm in rp
                                  where rpm.Date == QueryJson.BudgetYear
                                  join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  //join hrm in hr on g.Key.Account equals hrm.Key.Account
                                  join icm in ic on rpm.ItemId_TravelingPurpose equals icm.ItemId
                                  join icm1 in ic1 on rpm.ItemId_TravelingType equals icm1.ItemId
                                  orderby new { rpm.Factory, urm.DepartmentName, rpm.AltwName, rpm.Country, rpm.Days, Purpose = icm.Name, TravellingType = icm1.Name, rpm.Remark }
                                  select new
                                  {
                                      Factory = rpm.Factory,
                                      DepartmentName = urm.DepartmentName,
                                      Employee = rpm.AltwName ?? "",
                                      Country = rpm.Country ?? "",
                                      Days = rpm.Days ?? 0,
                                      Purpose = icm.Name,
                                      TravellingType = icm1.Name,
                                      Remark = rpm.Remark ?? "",
                                      JanBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Jan ?? 0) * mv : (rpm.Jan ?? 0),
                                      FebBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Feb ?? 0) * mv : (rpm.Feb ?? 0),
                                      MarBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Mar ?? 0) * mv : (rpm.Mar ?? 0),
                                      AprBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Apr ?? 0) * mv : (rpm.Apr ?? 0),
                                      MayBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.May ?? 0) * mv : (rpm.May ?? 0),
                                      JunBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Jun ?? 0) * mv : (rpm.Jun ?? 0),
                                      JulBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Jul ?? 0) * mv : (rpm.Jul ?? 0),
                                      AugBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Aug ?? 0) * mv : (rpm.Aug ?? 0),
                                      SepBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Sep ?? 0) * mv : (rpm.Sep ?? 0),
                                      OctBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Oct ?? 0) * mv : (rpm.Oct ?? 0),
                                      NovBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Nov ?? 0) * mv : (rpm.Nov ?? 0),
                                      DecBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Dec ?? 0) * mv : (rpm.Dec ?? 0)
                                  });
                    break;
                case "1":
                    ReportData = (from rpm in rp
                                  where rpm.Date == QueryJson.BudgetYear
                                  group rpm by new { rpm.Factory, rpm.DepartmentId, rpm.Version, rpm.AltwName } into g
                                  join urm in dr on new { g.Key.Factory, g.Key.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { g.Key.Factory, g.Key.DepartmentId, g.Key.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  //join hrm in hr on g.Key.Account equals hrm.Key.Account
                                  orderby new { g.Key.Factory, g.Key.DepartmentId, g.Key.AltwName }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = urm.DepartmentName,
                                      Employee = g.Key.AltwName ?? "",
                                      JanBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jan ?? 0) * mv : g.Sum(rpm => rpm.Jan ?? 0),
                                      FebBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Feb ?? 0) * mv : g.Sum(rpm => rpm.Feb ?? 0),
                                      MarBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Mar ?? 0) * mv : g.Sum(rpm => rpm.Mar ?? 0),
                                      AprBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Apr ?? 0) * mv : g.Sum(rpm => rpm.Apr ?? 0),
                                      MayBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.May ?? 0) * mv : g.Sum(rpm => rpm.May ?? 0),
                                      JunBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jun ?? 0) * mv : g.Sum(rpm => rpm.Jun ?? 0),
                                      JulBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jul ?? 0) * mv : g.Sum(rpm => rpm.Jul ?? 0),
                                      AugBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Aug ?? 0) * mv : g.Sum(rpm => rpm.Aug ?? 0),
                                      SepBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Sep ?? 0) * mv : g.Sum(rpm => rpm.Sep ?? 0),
                                      OctBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Oct ?? 0) * mv : g.Sum(rpm => rpm.Oct ?? 0),
                                      NovBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Nov ?? 0) * mv : g.Sum(rpm => rpm.Nov ?? 0),
                                      DecBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Dec ?? 0) * mv : g.Sum(rpm => rpm.Dec ?? 0)
                                  });
                    break;
                case "2":
                    ReportData = (from rpm in rp
                                  where rpm.Date == QueryJson.BudgetYear
                                  group rpm by new { rpm.Factory, rpm.DepartmentId, rpm.Version, rpm.AltwName, rpm.Country, rpm.ItemId_TravelingPurpose } into g
                                  join urm in dr on new { g.Key.Factory, g.Key.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { g.Key.Factory, g.Key.DepartmentId, g.Key.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  //join hrm in hr on g.Key.Account equals hrm.Key.Account
                                  join icm in ic on g.Key.ItemId_TravelingPurpose equals icm.ItemId
                                  orderby new { g.Key.Factory, g.Key.DepartmentId, g.Key.AltwName, g.Key.Country, g.Key.ItemId_TravelingPurpose }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = urm.DepartmentName,
                                      Employee = g.Key.AltwName ?? "",
                                      Country = g.Key.Country ?? "",
                                      Purpose = icm.Name,
                                      JanBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jan ?? 0) * mv : g.Sum(rpm => rpm.Jan ?? 0),
                                      FebBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Feb ?? 0) * mv : g.Sum(rpm => rpm.Feb ?? 0),
                                      MarBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Mar ?? 0) * mv : g.Sum(rpm => rpm.Mar ?? 0),
                                      AprBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Apr ?? 0) * mv : g.Sum(rpm => rpm.Apr ?? 0),
                                      MayBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.May ?? 0) * mv : g.Sum(rpm => rpm.May ?? 0),
                                      JunBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jun ?? 0) * mv : g.Sum(rpm => rpm.Jun ?? 0),
                                      JulBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jul ?? 0) * mv : g.Sum(rpm => rpm.Jul ?? 0),
                                      AugBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Aug ?? 0) * mv : g.Sum(rpm => rpm.Aug ?? 0),
                                      SepBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Sep ?? 0) * mv : g.Sum(rpm => rpm.Sep ?? 0),
                                      OctBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Oct ?? 0) * mv : g.Sum(rpm => rpm.Oct ?? 0),
                                      NovBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Nov ?? 0) * mv : g.Sum(rpm => rpm.Nov ?? 0),
                                      DecBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Dec ?? 0) * mv : g.Sum(rpm => rpm.Dec ?? 0)
                                  });
                    break;
                case "3":
                    ReportData = (from rpm in rp
                                  where rpm.Date == QueryJson.BudgetYear
                                  group rpm by new { rpm.Factory, rpm.DepartmentId, rpm.Version, rpm.AltwName, rpm.Country, rpm.Days, rpm.ItemId_TravelingPurpose } into g
                                  join urm in dr on new { g.Key.Factory, g.Key.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { g.Key.Factory, g.Key.DepartmentId, g.Key.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  //join hrm in hr on g.Key.Account equals hrm.Key.Account
                                  join icm in ic on g.Key.ItemId_TravelingPurpose equals icm.ItemId
                                  orderby new { g.Key.Factory, g.Key.DepartmentId, g.Key.AltwName, g.Key.Country, g.Key.Days, g.Key.ItemId_TravelingPurpose }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = urm.DepartmentName,
                                      Employee = g.Key.AltwName ?? "",
                                      Country = g.Key.Country ?? "",
                                      Days = g.Key.Days ?? 0,
                                      Purpose = icm.Name,
                                      JanBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jan ?? 0) * mv : g.Sum(rpm => rpm.Jan ?? 0),
                                      FebBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Feb ?? 0) * mv : g.Sum(rpm => rpm.Feb ?? 0),
                                      MarBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Mar ?? 0) * mv : g.Sum(rpm => rpm.Mar ?? 0),
                                      AprBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Apr ?? 0) * mv : g.Sum(rpm => rpm.Apr ?? 0),
                                      MayBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.May ?? 0) * mv : g.Sum(rpm => rpm.May ?? 0),
                                      JunBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jun ?? 0) * mv : g.Sum(rpm => rpm.Jun ?? 0),
                                      JulBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jul ?? 0) * mv : g.Sum(rpm => rpm.Jul ?? 0),
                                      AugBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Aug ?? 0) * mv : g.Sum(rpm => rpm.Aug ?? 0),
                                      SepBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Sep ?? 0) * mv : g.Sum(rpm => rpm.Sep ?? 0),
                                      OctBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Oct ?? 0) * mv : g.Sum(rpm => rpm.Oct ?? 0),
                                      NovBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Nov ?? 0) * mv : g.Sum(rpm => rpm.Nov ?? 0),
                                      DecBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Dec ?? 0) * mv : g.Sum(rpm => rpm.Dec ?? 0)
                                  });
                    break;
                case "4":
                    ReportData = (from rpm in rp
                                  where rpm.Date == QueryJson.BudgetYear
                                  group rpm by new { rpm.Factory, rpm.DepartmentId, rpm.Version, rpm.ItemId_TravelingType } into g
                                  join urm in dr on new { g.Key.Factory, g.Key.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { g.Key.Factory, g.Key.DepartmentId, g.Key.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm1 in ic1 on g.Key.ItemId_TravelingType equals icm1.ItemId
                                  orderby new { g.Key.Factory, g.Key.DepartmentId, g.Key.ItemId_TravelingType }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = urm.DepartmentName,
                                      TravellingType = icm1.Name,
                                      JanBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jan ?? 0) * mv : g.Sum(rpm => rpm.Jan ?? 0),
                                      FebBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Feb ?? 0) * mv : g.Sum(rpm => rpm.Feb ?? 0),
                                      MarBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Mar ?? 0) * mv : g.Sum(rpm => rpm.Mar ?? 0),
                                      AprBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Apr ?? 0) * mv : g.Sum(rpm => rpm.Apr ?? 0),
                                      MayBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.May ?? 0) * mv : g.Sum(rpm => rpm.May ?? 0),
                                      JunBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jun ?? 0) * mv : g.Sum(rpm => rpm.Jun ?? 0),
                                      JulBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jul ?? 0) * mv : g.Sum(rpm => rpm.Jul ?? 0),
                                      AugBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Aug ?? 0) * mv : g.Sum(rpm => rpm.Aug ?? 0),
                                      SepBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Sep ?? 0) * mv : g.Sum(rpm => rpm.Sep ?? 0),
                                      OctBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Oct ?? 0) * mv : g.Sum(rpm => rpm.Oct ?? 0),
                                      NovBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Nov ?? 0) * mv : g.Sum(rpm => rpm.Nov ?? 0),
                                      DecBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Dec ?? 0) * mv : g.Sum(rpm => rpm.Dec ?? 0)
                                  });
                    break;
            }
            return ReportData;
        }

        public Object GetKPIReportData(string JsonStr)
        {
            MyQueryJson QueryJson = JsonConvert.DeserializeObject<MyQueryJson>(JsonStr);

            var rp = _repo_ki.GetAll();
            var dr = _repo_dr.GetAll();
            var fv = _repo_fv.GetAll();

            if (QueryJson.LTDept.Length > 0 && QueryJson.KSZDept.Length > 0)
            {
                rp = _repo_ki.GetAll().Where(x => (x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId)) || (x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId)));
            }
            else
            {
                if (QueryJson.LTDept.Length > 0)
                    rp = _repo_ki.GetAll().Where(x => x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId));
                if (QueryJson.KSZDept.Length > 0)
                    rp = _repo_ki.GetAll().Where(x => x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId));
            }

            fv = _repo_fv.GetAll().Where(x => (x.Date == QueryJson.BudgetYear && x.ItemId_BudgetName == "KPI"));

            var ReportData = (from rpm in rp
                              join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                              join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                              where rpm.Date == QueryJson.BudgetYear
                              orderby rpm.Id
                              select new
                              {
                                  GoalsObjectives = rpm.GoalName ?? "",
                                  PIC = rpm.PIC_Name ?? "",
                                  LastYearActual = rpm.LastYear ?? "",
                                  ThisYearYTD = rpm.YTD ?? "",
                                  JanBud = rpm.Jan ?? "",
                                  FebBud = rpm.Feb ?? "",
                                  MarBud = rpm.Mar ?? "",
                                  AprBud = rpm.Apr ?? "",
                                  MayBud = rpm.May ?? "",
                                  JunBud = rpm.Jun ?? "",
                                  JulBud = rpm.Jul ?? "",
                                  AugBud = rpm.Aug ?? "",
                                  SepBud = rpm.Sep ?? "",
                                  OctBud = rpm.Oct ?? "",
                                  NovBud = rpm.Nov ?? "",
                                  DecBud = rpm.Dec ?? ""
                              });
            return ReportData;

        }

        public Object GetHeadCountReportData(string JsonStr)
        {
            MyQueryJson QueryJson = JsonConvert.DeserializeObject<MyQueryJson>(JsonStr);
            var hp = _repo_hc.GetAll();
            var dr = _repo_dr.GetAll();
            var fv = _repo_fv.GetAll();
            //var hr = _repo_hr.GetAll().GroupBy(x => new { x.Account, x.AltwName,x.ItemId_JobFunction,x.ItemId_Title,x.ItemId_DirectType,x.ItemId_HR });

            var hr = _repo_hr.GetAll().Where(x => (x.ItemId_DirectType != null && x.ItemId_JobFunction != null && x.ItemId_DirectType != null)).Distinct();

            var hrmg = (from hrm in hr
                        where hrm.ItemId_HR == "" || hrm.ItemId_HR == null
                        select new
                        {
                            hrm.Account,
                            hrm.AltwName,
                            hrm.ItemId_DirectType,
                            hrm.ItemId_JobFunction,
                            hrm.ItemId_Title,
                            ItemId_HR = "00010000"
                        }
                )
                .Union
                (from hrm in hr
                 where hrm.ItemId_HR != "" && hrm.ItemId_HR != null
                 select new
                 {
                     hrm.Account,
                     hrm.AltwName,
                     hrm.ItemId_DirectType,
                     hrm.ItemId_JobFunction,
                     hrm.ItemId_Title,
                     hrm.ItemId_HR
                 }
                );

            var ic = _repo_ic.GetAll().Where(x => (x.ClassName == "JobFunction" || x.ClassName == "JobFunctionKSZ"));
            var ic1 = _repo_ic.GetAll().Where(x => (x.ClassName == "Title" || x.ClassName == "TitleKSZ"));
            var ic2 = _repo_ic.GetAll().Where(x => x.ClassName == "DirectType");
            var ic3 = _repo_ic.GetAll().Where(x => x.ClassName == "HR");
            var ic4 = _repo_ic.GetAll().Where(x => x.ClassName == "Reason");

            if (QueryJson.LTDept.Length > 0 && QueryJson.KSZDept.Length > 0)
            {
                hp = _repo_hc.GetAll().Where(x => (x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId)) || (x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId)));
            }
            else
            {
                if (QueryJson.LTDept.Length > 0)
                    hp = _repo_hc.GetAll().Where(x => x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId));
                if (QueryJson.KSZDept.Length > 0)
                    hp = _repo_hc.GetAll().Where(x => x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId));
            }
            fv = _repo_fv.GetAll().Where(x => (x.Date == QueryJson.BudgetYear && x.ItemId_BudgetName == "HeadCount"));

            var rp = hp;
            if (QueryJson.TBH == "1")
            {
                rp = hp.Where(x => x.CurrentHC == true);
            }

            var rpmg = (from rpm in rp
                        where rpm.ItemId_Reason == "" || rpm.ItemId_Reason == null
                        select new
                        {
                            rpm.Account
                              ,
                            rpm.SepLast
                              ,
                            rpm.OctLast
                              ,
                            rpm.NovLast
                              ,
                            rpm.DecLast
                              ,
                            rpm.Jan
                              ,
                            rpm.Feb
                              ,
                            rpm.Mar
                              ,
                            rpm.Apr
                              ,
                            rpm.May
                              ,
                            rpm.Jun
                              ,
                            rpm.Jul
                              ,
                            rpm.Aug
                              ,
                            rpm.Sep
                              ,
                            rpm.Oct
                              ,
                            rpm.Nov
                              ,
                            rpm.Dec
                              ,
                            rpm.Date
                              ,
                            rpm.DepartmentId
                              ,
                            rpm.Version
                              ,
                            rpm.JulLast
                              ,
                            rpm.AugLast
                              ,
                            rpm.Factory
                              ,
                            rpm.PartTime
                              ,
                            rpm.CurrentHC
                              ,
                            ItemId_Reason = "00150000"
                              ,
                            rpm.Remark
                        }
                )
                .Union
                (from rpm in rp
                 where rpm.ItemId_Reason != "" && rpm.ItemId_Reason != null
                 select new
                 {
                     rpm.Account
                                   ,
                     rpm.SepLast
                                   ,
                     rpm.OctLast
                                   ,
                     rpm.NovLast
                                   ,
                     rpm.DecLast
                                   ,
                     rpm.Jan
                                   ,
                     rpm.Feb
                                   ,
                     rpm.Mar
                                   ,
                     rpm.Apr
                                   ,
                     rpm.May
                                   ,
                     rpm.Jun
                                   ,
                     rpm.Jul
                                   ,
                     rpm.Aug
                                   ,
                     rpm.Sep
                                   ,
                     rpm.Oct
                                   ,
                     rpm.Nov
                                   ,
                     rpm.Dec
                                   ,
                     rpm.Date
                                   ,
                     rpm.DepartmentId
                                   ,
                     rpm.Version
                                   ,
                     rpm.JulLast
                                   ,
                     rpm.AugLast
                                   ,
                     rpm.Factory
                                   ,
                     rpm.PartTime
                                   ,
                     rpm.CurrentHC
                                   ,
                     rpm.ItemId_Reason
                                   ,
                     rpm.Remark
                 });

            Object ReportData = null;

            switch (QueryJson.QueryGroupby)
            {
                case "0":
                    ReportData = (from rpm in rpmg
                                  where rpm.Date == QueryJson.BudgetYear
                                  join hrm in hrmg on rpm.Account equals hrm.Account
                                  join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm in ic on hrm.ItemId_JobFunction equals icm.ItemId
                                  join icm1 in ic1 on hrm.ItemId_Title equals icm1.ItemId
                                  join icm2 in ic2 on hrm.ItemId_DirectType equals icm2.ItemId
                                  join icm3 in ic3 on hrm.ItemId_HR equals icm3.ItemId
                                  join icm4 in ic4 on rpm.ItemId_Reason equals icm4.ItemId
                                  group new { rpm, hrm, urm, icm, icm1, icm2, icm3, icm4 } by new { rpm.Factory, urm.DepartmentName,urm.DepartmentId, hrm.AltwName, Name4 = icm4.Name, rpm.Remark, icm.Name, Name1 = icm1.Name, Name2 = icm3.Name, Name3 = icm2.Name,rpm.Account } into g
                                  orderby new { g.Key.Factory, g.Key.DepartmentName, g.Key.AltwName, g.Key.Name4, g.Key.Remark, g.Key.Name, g.Key.Name1, g.Key.Name2, g.Key.Name3 }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = g.Key.DepartmentName,
                                      DepartmentId = g.Key.DepartmentId,
                                      Account = g.Key.Account ,
                                      EmployeeName = g.Key.AltwName,
                                      Reason = g.Key.Name4,
                                      Remark = g.Key.Remark ?? "",
                                      FunctionDesc = g.Key.Name,
                                      Title = g.Key.Name1,
                                      FOH_ADM_Selling_RD = g.Key.Name2,
                                      DLIDL = g.Key.Name3,
                                      JulTY = g.Sum(x => x.rpm.JulLast ?? 0),
                                      AugTY = g.Sum(x => x.rpm.AugLast ?? 0),
                                      SepTY = g.Sum(x => x.rpm.SepLast ?? 0),
                                      OctTY = g.Sum(x => x.rpm.OctLast ?? 0),
                                      NovTY = g.Sum(x => x.rpm.NovLast ?? 0),
                                      DecTY = g.Sum(x => x.rpm.DecLast ?? 0),
                                      JanBud = g.Sum(x => x.rpm.Jan ?? 0),
                                      FebBud = g.Sum(x => x.rpm.Feb ?? 0),
                                      MarBud = g.Sum(x => x.rpm.Mar ?? 0),
                                      AprBud = g.Sum(x => x.rpm.Apr ?? 0),
                                      MayBud = g.Sum(x => x.rpm.May ?? 0),
                                      JunBud = g.Sum(x => x.rpm.Jun ?? 0),
                                      JulBud = g.Sum(x => x.rpm.Jul ?? 0),
                                      AugBud = g.Sum(x => x.rpm.Aug ?? 0),
                                      SepBud = g.Sum(x => x.rpm.Sep ?? 0),
                                      OctBud = g.Sum(x => x.rpm.Oct ?? 0),
                                      NovBud = g.Sum(x => x.rpm.Nov ?? 0),
                                      DecBud = g.Sum(x => x.rpm.Dec ?? 0)
                                  });
                    break;
                case "1":
                    ReportData = (from rpm in rpmg
                                  where rpm.Date == QueryJson.BudgetYear
                                  join hrm in hrmg on rpm.Account equals hrm.Account
                                  join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm2 in ic2 on hrm.ItemId_DirectType equals icm2.ItemId
                                  group new { rpm, icm2 } by new { rpm.Factory, icm2.Name } into g
                                  orderby new { g.Key.Factory, g.Key.Name }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DLIDL = g.Key.Name,
                                      JulTY = g.Sum(x => x.rpm.JulLast ?? 0),
                                      AugTY = g.Sum(x => x.rpm.AugLast ?? 0),
                                      SepTY = g.Sum(x => x.rpm.SepLast ?? 0),
                                      OctTY = g.Sum(x => x.rpm.OctLast ?? 0),
                                      NovTY = g.Sum(x => x.rpm.NovLast ?? 0),
                                      DecTY = g.Sum(x => x.rpm.DecLast ?? 0),
                                      JanBud = g.Sum(x => x.rpm.Jan ?? 0),
                                      FebBud = g.Sum(x => x.rpm.Feb ?? 0),
                                      MarBud = g.Sum(x => x.rpm.Mar ?? 0),
                                      AprBud = g.Sum(x => x.rpm.Apr ?? 0),
                                      MayBud = g.Sum(x => x.rpm.May ?? 0),
                                      JunBud = g.Sum(x => x.rpm.Jun ?? 0),
                                      JulBud = g.Sum(x => x.rpm.Jul ?? 0),
                                      AugBud = g.Sum(x => x.rpm.Aug ?? 0),
                                      SepBud = g.Sum(x => x.rpm.Sep ?? 0),
                                      OctBud = g.Sum(x => x.rpm.Oct ?? 0),
                                      NovBud = g.Sum(x => x.rpm.Nov ?? 0),
                                      DecBud = g.Sum(x => x.rpm.Dec ?? 0)
                                  });
                    break;
                case "2":
                    ReportData = (from rpm in rpmg
                                  where rpm.Date == QueryJson.BudgetYear
                                  join hrm in hrmg on rpm.Account equals hrm.Account
                                  join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm2 in ic2 on hrm.ItemId_DirectType equals icm2.ItemId
                                  group new { rpm, urm, icm2 } by new { rpm.Factory, urm.DepartmentName, icm2.Name } into g
                                  orderby new { g.Key.Factory, g.Key.DepartmentName, g.Key.Name }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = g.Key.DepartmentName,
                                      DLIDL = g.Key.Name,
                                      JulTY = g.Sum(x => x.rpm.JulLast ?? 0),
                                      AugTY = g.Sum(x => x.rpm.AugLast ?? 0),
                                      SepTY = g.Sum(x => x.rpm.SepLast ?? 0),
                                      OctTY = g.Sum(x => x.rpm.OctLast ?? 0),
                                      NovTY = g.Sum(x => x.rpm.NovLast ?? 0),
                                      DecTY = g.Sum(x => x.rpm.DecLast ?? 0),
                                      JanBud = g.Sum(x => x.rpm.Jan ?? 0),
                                      FebBud = g.Sum(x => x.rpm.Feb ?? 0),
                                      MarBud = g.Sum(x => x.rpm.Mar ?? 0),
                                      AprBud = g.Sum(x => x.rpm.Apr ?? 0),
                                      MayBud = g.Sum(x => x.rpm.May ?? 0),
                                      JunBud = g.Sum(x => x.rpm.Jun ?? 0),
                                      JulBud = g.Sum(x => x.rpm.Jul ?? 0),
                                      AugBud = g.Sum(x => x.rpm.Aug ?? 0),
                                      SepBud = g.Sum(x => x.rpm.Sep ?? 0),
                                      OctBud = g.Sum(x => x.rpm.Oct ?? 0),
                                      NovBud = g.Sum(x => x.rpm.Nov ?? 0),
                                      DecBud = g.Sum(x => x.rpm.Dec ?? 0)
                                  });
                    break;
                case "3":
                    ReportData = (from rpm in rpmg
                                  where rpm.Date == QueryJson.BudgetYear
                                  join hrm in hrmg on rpm.Account equals hrm.Account
                                  join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm in ic on hrm.ItemId_JobFunction equals icm.ItemId
                                  join icm1 in ic1 on hrm.ItemId_Title equals icm1.ItemId
                                  join icm2 in ic2 on hrm.ItemId_DirectType equals icm2.ItemId
                                  join icm3 in ic3 on hrm.ItemId_HR equals icm3.ItemId
                                  group new { rpm, hrm, urm, icm, icm1, icm2, icm3 } by new { rpm.Factory, urm.DepartmentName, hrm.AltwName, icm.Name, Name1 = icm1.Name, Name2 = icm3.Name, Name3 = icm2.Name } into g
                                  orderby new { g.Key.Factory, g.Key.DepartmentName, g.Key.AltwName, g.Key.Name, g.Key.Name1, g.Key.Name2, g.Key.Name3 }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = g.Key.DepartmentName,
                                      EmployeeName = g.Key.AltwName,
                                      FunctionDesc = g.Key.Name,
                                      Title = g.Key.Name1,
                                      FOH_ADM_Selling_RD = g.Key.Name2,
                                      DLIDL = g.Key.Name3,
                                      JulTY = g.Sum(x => x.rpm.JulLast ?? 0),
                                      AugTY = g.Sum(x => x.rpm.AugLast ?? 0),
                                      SepTY = g.Sum(x => x.rpm.SepLast ?? 0),
                                      OctTY = g.Sum(x => x.rpm.OctLast ?? 0),
                                      NovTY = g.Sum(x => x.rpm.NovLast ?? 0),
                                      DecTY = g.Sum(x => x.rpm.DecLast ?? 0),
                                      JanBud = g.Sum(x => x.rpm.Jan ?? 0),
                                      FebBud = g.Sum(x => x.rpm.Feb ?? 0),
                                      MarBud = g.Sum(x => x.rpm.Mar ?? 0),
                                      AprBud = g.Sum(x => x.rpm.Apr ?? 0),
                                      MayBud = g.Sum(x => x.rpm.May ?? 0),
                                      JunBud = g.Sum(x => x.rpm.Jun ?? 0),
                                      JulBud = g.Sum(x => x.rpm.Jul ?? 0),
                                      AugBud = g.Sum(x => x.rpm.Aug ?? 0),
                                      SepBud = g.Sum(x => x.rpm.Sep ?? 0),
                                      OctBud = g.Sum(x => x.rpm.Oct ?? 0),
                                      NovBud = g.Sum(x => x.rpm.Nov ?? 0),
                                      DecBud = g.Sum(x => x.rpm.Dec ?? 0)
                                  });
                    break;
                case "4":
                    ReportData = (from rpm in rpmg
                                  where rpm.Date == QueryJson.BudgetYear
                                  join hrm in hrmg on rpm.Account equals hrm.Account
                                  join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm in ic on hrm.ItemId_JobFunction equals icm.ItemId
                                  join icm1 in ic1 on hrm.ItemId_Title equals icm1.ItemId
                                  join icm2 in ic2 on hrm.ItemId_DirectType equals icm2.ItemId
                                  join icm3 in ic3 on hrm.ItemId_HR equals icm3.ItemId
                                  group new { rpm, urm, icm, icm1, icm2, icm3 } by new { rpm.Factory, urm.DepartmentName, icm.Name, Name1 = icm1.Name, Name2 = icm3.Name, Name3 = icm2.Name } into g
                                  orderby new { g.Key.Factory, g.Key.DepartmentName, g.Key.Name, g.Key.Name1, g.Key.Name2, g.Key.Name3 }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = g.Key.DepartmentName,
                                      FunctionDesc = g.Key.Name,
                                      Title = g.Key.Name1,
                                      FOH_ADM_Selling_RD = g.Key.Name2,
                                      DLIDL = g.Key.Name3,
                                      JulTY = g.Sum(x => x.rpm.JulLast ?? 0),
                                      AugTY = g.Sum(x => x.rpm.AugLast ?? 0),
                                      SepTY = g.Sum(x => x.rpm.SepLast ?? 0),
                                      OctTY = g.Sum(x => x.rpm.OctLast ?? 0),
                                      NovTY = g.Sum(x => x.rpm.NovLast ?? 0),
                                      DecTY = g.Sum(x => x.rpm.DecLast ?? 0),
                                      JanBud = g.Sum(x => x.rpm.Jan ?? 0),
                                      FebBud = g.Sum(x => x.rpm.Feb ?? 0),
                                      MarBud = g.Sum(x => x.rpm.Mar ?? 0),
                                      AprBud = g.Sum(x => x.rpm.Apr ?? 0),
                                      MayBud = g.Sum(x => x.rpm.May ?? 0),
                                      JunBud = g.Sum(x => x.rpm.Jun ?? 0),
                                      JulBud = g.Sum(x => x.rpm.Jul ?? 0),
                                      AugBud = g.Sum(x => x.rpm.Aug ?? 0),
                                      SepBud = g.Sum(x => x.rpm.Sep ?? 0),
                                      OctBud = g.Sum(x => x.rpm.Oct ?? 0),
                                      NovBud = g.Sum(x => x.rpm.Nov ?? 0),
                                      DecBud = g.Sum(x => x.rpm.Dec ?? 0)
                                  });
                    break;
                case "5":
                    ReportData = (from rpm in rpmg
                                  where rpm.Date == QueryJson.BudgetYear
                                  join hrm in hrmg on rpm.Account equals hrm.Account
                                  join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm3 in ic3 on hrm.ItemId_HR equals icm3.ItemId
                                  group new { rpm, icm3 } by new { rpm.Factory, icm3.Name } into g
                                  orderby new { g.Key.Factory, g.Key.Name }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      FOH_ADM_Selling_RD = g.Key.Name,
                                      JulTY = g.Sum(x => x.rpm.JulLast ?? 0),
                                      AugTY = g.Sum(x => x.rpm.AugLast ?? 0),
                                      SepTY = g.Sum(x => x.rpm.SepLast ?? 0),
                                      OctTY = g.Sum(x => x.rpm.OctLast ?? 0),
                                      NovTY = g.Sum(x => x.rpm.NovLast ?? 0),
                                      DecTY = g.Sum(x => x.rpm.DecLast ?? 0),
                                      JanBud = g.Sum(x => x.rpm.Jan ?? 0),
                                      FebBud = g.Sum(x => x.rpm.Feb ?? 0),
                                      MarBud = g.Sum(x => x.rpm.Mar ?? 0),
                                      AprBud = g.Sum(x => x.rpm.Apr ?? 0),
                                      MayBud = g.Sum(x => x.rpm.May ?? 0),
                                      JunBud = g.Sum(x => x.rpm.Jun ?? 0),
                                      JulBud = g.Sum(x => x.rpm.Jul ?? 0),
                                      AugBud = g.Sum(x => x.rpm.Aug ?? 0),
                                      SepBud = g.Sum(x => x.rpm.Sep ?? 0),
                                      OctBud = g.Sum(x => x.rpm.Oct ?? 0),
                                      NovBud = g.Sum(x => x.rpm.Nov ?? 0),
                                      DecBud = g.Sum(x => x.rpm.Dec ?? 0)
                                  });
                    break;
                case "6":
                    ReportData = (from rpm in rpmg
                                  where rpm.Date == QueryJson.BudgetYear
                                  join hrm in hrmg on rpm.Account equals hrm.Account
                                  join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm3 in ic3 on hrm.ItemId_HR equals icm3.ItemId
                                  group new { rpm, urm, icm3 } by new { rpm.Factory, urm.DepartmentName, icm3.Name } into g
                                  orderby new { g.Key.Factory, g.Key.DepartmentName, g.Key.Name }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = g.Key.DepartmentName,
                                      FOH_ADM_Selling_RD = g.Key.Name,
                                      JulTY = g.Sum(x => x.rpm.JulLast ?? 0),
                                      AugTY = g.Sum(x => x.rpm.AugLast ?? 0),
                                      SepTY = g.Sum(x => x.rpm.SepLast ?? 0),
                                      OctTY = g.Sum(x => x.rpm.OctLast ?? 0),
                                      NovTY = g.Sum(x => x.rpm.NovLast ?? 0),
                                      DecTY = g.Sum(x => x.rpm.DecLast ?? 0),
                                      JanBud = g.Sum(x => x.rpm.Jan ?? 0),
                                      FebBud = g.Sum(x => x.rpm.Feb ?? 0),
                                      MarBud = g.Sum(x => x.rpm.Mar ?? 0),
                                      AprBud = g.Sum(x => x.rpm.Apr ?? 0),
                                      MayBud = g.Sum(x => x.rpm.May ?? 0),
                                      JunBud = g.Sum(x => x.rpm.Jun ?? 0),
                                      JulBud = g.Sum(x => x.rpm.Jul ?? 0),
                                      AugBud = g.Sum(x => x.rpm.Aug ?? 0),
                                      SepBud = g.Sum(x => x.rpm.Sep ?? 0),
                                      OctBud = g.Sum(x => x.rpm.Oct ?? 0),
                                      NovBud = g.Sum(x => x.rpm.Nov ?? 0),
                                      DecBud = g.Sum(x => x.rpm.Dec ?? 0)
                                  });
                    break;
                case "7":
                    ReportData = (from rpm in rpmg
                                  where rpm.Date == QueryJson.BudgetYear
                                  join hrm in hrmg on rpm.Account equals hrm.Account
                                  join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm1 in ic1 on hrm.ItemId_Title equals icm1.ItemId
                                  group new { rpm, urm, hrm, icm1 } by new { rpm.Factory, urm.DepartmentName, hrm.AltwName, Name1 = icm1.Name } into g
                                  orderby new { g.Key.Factory, g.Key.DepartmentName, g.Key.AltwName, g.Key.Name1 }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = g.Key.DepartmentName,
                                      EmployeeName = g.Key.AltwName,
                                      Title = g.Key.Name1,
                                      JulTY = g.Sum(x => x.rpm.JulLast ?? 0),
                                      AugTY = g.Sum(x => x.rpm.AugLast ?? 0),
                                      SepTY = g.Sum(x => x.rpm.SepLast ?? 0),
                                      OctTY = g.Sum(x => x.rpm.OctLast ?? 0),
                                      NovTY = g.Sum(x => x.rpm.NovLast ?? 0),
                                      DecTY = g.Sum(x => x.rpm.DecLast ?? 0),
                                      JanBud = g.Sum(x => x.rpm.Jan ?? 0),
                                      FebBud = g.Sum(x => x.rpm.Feb ?? 0),
                                      MarBud = g.Sum(x => x.rpm.Mar ?? 0),
                                      AprBud = g.Sum(x => x.rpm.Apr ?? 0),
                                      MayBud = g.Sum(x => x.rpm.May ?? 0),
                                      JunBud = g.Sum(x => x.rpm.Jun ?? 0),
                                      JulBud = g.Sum(x => x.rpm.Jul ?? 0),
                                      AugBud = g.Sum(x => x.rpm.Aug ?? 0),
                                      SepBud = g.Sum(x => x.rpm.Sep ?? 0),
                                      OctBud = g.Sum(x => x.rpm.Oct ?? 0),
                                      NovBud = g.Sum(x => x.rpm.Nov ?? 0),
                                      DecBud = g.Sum(x => x.rpm.Dec ?? 0)
                                  });
                    break;
                case "8":
                    ReportData = (from rpm in rpmg
                                  where rpm.Date == QueryJson.BudgetYear
                                  join hrm in hrmg on rpm.Account equals hrm.Account
                                  join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm in ic on hrm.ItemId_JobFunction equals icm.ItemId
                                  join icm1 in ic1 on hrm.ItemId_Title equals icm1.ItemId
                                  group new { rpm, urm, hrm, icm, icm1 } by new { rpm.Factory, urm.DepartmentName, hrm.AltwName, icm.Name, Name1 = icm1.Name } into g
                                  orderby new { g.Key.Factory, g.Key.DepartmentName, g.Key.AltwName, g.Key.Name, g.Key.Name1 }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = g.Key.DepartmentName,
                                      EmployeeName = g.Key.AltwName,
                                      FunctionDesc = g.Key.Name,
                                      Title = g.Key.Name1,
                                      JulTY = g.Sum(x => x.rpm.JulLast ?? 0),
                                      AugTY = g.Sum(x => x.rpm.AugLast ?? 0),
                                      SepTY = g.Sum(x => x.rpm.SepLast ?? 0),
                                      OctTY = g.Sum(x => x.rpm.OctLast ?? 0),
                                      NovTY = g.Sum(x => x.rpm.NovLast ?? 0),
                                      DecTY = g.Sum(x => x.rpm.DecLast ?? 0),
                                      JanBud = g.Sum(x => x.rpm.Jan ?? 0),
                                      FebBud = g.Sum(x => x.rpm.Feb ?? 0),
                                      MarBud = g.Sum(x => x.rpm.Mar ?? 0),
                                      AprBud = g.Sum(x => x.rpm.Apr ?? 0),
                                      MayBud = g.Sum(x => x.rpm.May ?? 0),
                                      JunBud = g.Sum(x => x.rpm.Jun ?? 0),
                                      JulBud = g.Sum(x => x.rpm.Jul ?? 0),
                                      AugBud = g.Sum(x => x.rpm.Aug ?? 0),
                                      SepBud = g.Sum(x => x.rpm.Sep ?? 0),
                                      OctBud = g.Sum(x => x.rpm.Oct ?? 0),
                                      NovBud = g.Sum(x => x.rpm.Nov ?? 0),
                                      DecBud = g.Sum(x => x.rpm.Dec ?? 0)
                                  });
                    break;
                case "9":
                    ReportData = (from rpm in rpmg
                                  where rpm.Date == QueryJson.BudgetYear
                                  join hrm in hrmg on rpm.Account equals hrm.Account
                                  join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm1 in ic1 on hrm.ItemId_Title equals icm1.ItemId
                                  join icm2 in ic2 on hrm.ItemId_DirectType equals icm2.ItemId
                                  join icm3 in ic3 on hrm.ItemId_HR equals icm3.ItemId
                                  group new { rpm, icm1, icm2, icm3 } by new { rpm.Factory, Name1 = icm1.Name, Name2 = icm3.Name, Name3 = icm2.Name } into g
                                  orderby new { g.Key.Factory, g.Key.Name1, g.Key.Name2, g.Key.Name3 }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      Title = g.Key.Name1,
                                      FOH_ADM_Selling_RD = g.Key.Name2,
                                      DLIDL = g.Key.Name3,
                                      JulTY = g.Sum(x => x.rpm.JulLast ?? 0),
                                      AugTY = g.Sum(x => x.rpm.AugLast ?? 0),
                                      SepTY = g.Sum(x => x.rpm.SepLast ?? 0),
                                      OctTY = g.Sum(x => x.rpm.OctLast ?? 0),
                                      NovTY = g.Sum(x => x.rpm.NovLast ?? 0),
                                      DecTY = g.Sum(x => x.rpm.DecLast ?? 0),
                                      JanBud = g.Sum(x => x.rpm.Jan ?? 0),
                                      FebBud = g.Sum(x => x.rpm.Feb ?? 0),
                                      MarBud = g.Sum(x => x.rpm.Mar ?? 0),
                                      AprBud = g.Sum(x => x.rpm.Apr ?? 0),
                                      MayBud = g.Sum(x => x.rpm.May ?? 0),
                                      JunBud = g.Sum(x => x.rpm.Jun ?? 0),
                                      JulBud = g.Sum(x => x.rpm.Jul ?? 0),
                                      AugBud = g.Sum(x => x.rpm.Aug ?? 0),
                                      SepBud = g.Sum(x => x.rpm.Sep ?? 0),
                                      OctBud = g.Sum(x => x.rpm.Oct ?? 0),
                                      NovBud = g.Sum(x => x.rpm.Nov ?? 0),
                                      DecBud = g.Sum(x => x.rpm.Dec ?? 0)
                                  });
                    break;
                case "A":
                    ReportData = (from rpm in rpmg
                                  where rpm.Date == QueryJson.BudgetYear
                                  join hrm in hrmg on rpm.Account equals hrm.Account
                                  join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm1 in ic1 on hrm.ItemId_Title equals icm1.ItemId
                                  join icm2 in ic2 on hrm.ItemId_DirectType equals icm2.ItemId
                                  join icm3 in ic3 on hrm.ItemId_HR equals icm3.ItemId
                                  group new { rpm, hrm, icm1, icm2, icm3 } by new { rpm.Factory, hrm.AltwName, Name1 = icm1.Name, Name2 = icm3.Name, Name3 = icm2.Name } into g
                                  orderby new { g.Key.Factory, g.Key.AltwName, g.Key.Name1, g.Key.Name2, g.Key.Name3 }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      EmployeeName = g.Key.AltwName,
                                      Title = g.Key.Name1,
                                      FOH_ADM_Selling_RD = g.Key.Name2,
                                      DLIDL = g.Key.Name3,
                                      JulTY = g.Sum(x => x.rpm.JulLast ?? 0),
                                      AugTY = g.Sum(x => x.rpm.AugLast ?? 0),
                                      SepTY = g.Sum(x => x.rpm.SepLast ?? 0),
                                      OctTY = g.Sum(x => x.rpm.OctLast ?? 0),
                                      NovTY = g.Sum(x => x.rpm.NovLast ?? 0),
                                      DecTY = g.Sum(x => x.rpm.DecLast ?? 0),
                                      JanBud = g.Sum(x => x.rpm.Jan ?? 0),
                                      FebBud = g.Sum(x => x.rpm.Feb ?? 0),
                                      MarBud = g.Sum(x => x.rpm.Mar ?? 0),
                                      AprBud = g.Sum(x => x.rpm.Apr ?? 0),
                                      MayBud = g.Sum(x => x.rpm.May ?? 0),
                                      JunBud = g.Sum(x => x.rpm.Jun ?? 0),
                                      JulBud = g.Sum(x => x.rpm.Jul ?? 0),
                                      AugBud = g.Sum(x => x.rpm.Aug ?? 0),
                                      SepBud = g.Sum(x => x.rpm.Sep ?? 0),
                                      OctBud = g.Sum(x => x.rpm.Oct ?? 0),
                                      NovBud = g.Sum(x => x.rpm.Nov ?? 0),
                                      DecBud = g.Sum(x => x.rpm.Dec ?? 0)
                                  });
                    break;
            }
            return ReportData;
        }

        public Object GetCapexReportData(string JsonStr)
        {
            MyQueryJson QueryJson = JsonConvert.DeserializeObject<MyQueryJson>(JsonStr);

            var rp = _repo_cp.GetAll();
            var dr = _repo_dr.GetAll();
            var fv = _repo_fv.GetAll();
            var ic = _repo_ic.GetAll().Where(x => x.ClassName == "AssetExp");
            var ic1 = _repo_ic.GetAll().Where(x => x.ClassName == "AssetExpType");
            var ic2 = _repo_ic.GetAll().Where(x => x.ClassName == "AssetExpPurpose");

            bool crossbranch = false;
            if (QueryJson.LTDept.Length > 0 && QueryJson.KSZDept.Length > 0)
            {
                crossbranch = true;
                rp = _repo_cp.GetAll().Where(x => (x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId)) || (x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId)));
            }
            else
            {
                if (QueryJson.LTDept.Length > 0)
                    rp = _repo_cp.GetAll().Where(x => x.Factory == "LT" && QueryJson.LTDept.Contains(x.DepartmentId));
                if (QueryJson.KSZDept.Length > 0)
                    rp = _repo_cp.GetAll().Where(x => x.Factory == "KSZ" && QueryJson.KSZDept.Contains(x.DepartmentId));
            }
            fv = _repo_fv.GetAll().Where(x => (x.Date == QueryJson.BudgetYear && x.ItemId_BudgetName == "Capex"));

            Object ReportData = null;
            int mv = 5;

            switch (QueryJson.QueryGroupby)
            {
                case "0":
                    ReportData = (from rpm in rp
                                  where rpm.Date == QueryJson.BudgetYear
                                  join urm in dr on new { rpm.Factory, rpm.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { rpm.Factory, rpm.DepartmentId, rpm.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm in ic on rpm.ItemId_AssetExp equals icm.ItemId
                                  join icm1 in ic1 on rpm.ItemId_AssetExpType equals icm1.ItemId
                                  join icm2 in ic2 on rpm.ItemId_Purpose equals icm2.ItemId
                                  orderby new { rpm.Factory, urm.DepartmentName, rpm.ItemId_AssetExp, rpm.InternalOrder, rpm.ProjectName, rpm.ProjectRemark, rpm.ItemId_AssetExpType, rpm.ItemId_Purpose }
                                  select new
                                  {
                                      Factory = rpm.Factory,
                                      DepartmentName = urm.DepartmentName,
                                      AssetExp = icm.Name,
                                      InternalOrder = rpm.InternalOrder ?? "",
                                      ProjectName = rpm.ProjectName ?? "",
                                      Remark = rpm.ProjectRemark ?? "",
                                      AssetExpType = icm1.Name,
                                      Purpose = icm2.Name,
                                      ProjectBudget = (crossbranch && rpm.Factory == "KSZ") ? (rpm.ProjectAmount ?? 0) * mv : (rpm.ProjectAmount ?? 0),
                                      YTDJuly = (crossbranch && rpm.Factory == "KSZ") ? (rpm.JulLast ?? 0) * mv : (rpm.JulLast ?? 0),
                                      AugTY = (crossbranch && rpm.Factory == "KSZ") ? (rpm.AugLast ?? 0) * mv : (rpm.AugLast ?? 0),
                                      SepTY = (crossbranch && rpm.Factory == "KSZ") ? (rpm.SepLast ?? 0) * mv : (rpm.SepLast ?? 0),
                                      OctTY = (crossbranch && rpm.Factory == "KSZ") ? (rpm.OctLast ?? 0) * mv : (rpm.OctLast ?? 0),
                                      NovTY = (crossbranch && rpm.Factory == "KSZ") ? (rpm.NovLast ?? 0) * mv : (rpm.NovLast ?? 0),
                                      DecTY = (crossbranch && rpm.Factory == "KSZ") ? (rpm.DecLast ?? 0) * mv : (rpm.DecLast ?? 0),
                                      JanBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Jan ?? 0) * mv : (rpm.Jan ?? 0),
                                      FebBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Feb ?? 0) * mv : (rpm.Feb ?? 0),
                                      MarBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Mar ?? 0) * mv : (rpm.Mar ?? 0),
                                      AprBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Apr ?? 0) * mv : (rpm.Apr ?? 0),
                                      MayBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.May ?? 0) * mv : (rpm.May ?? 0),
                                      JunBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Jun ?? 0) * mv : (rpm.Jun ?? 0),
                                      JulBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Jul ?? 0) * mv : (rpm.Jul ?? 0),
                                      AugBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Aug ?? 0) * mv : (rpm.Aug ?? 0),
                                      SepBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Sep ?? 0) * mv : (rpm.Sep ?? 0),
                                      OctBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Oct ?? 0) * mv : (rpm.Oct ?? 0),
                                      NovBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Nov ?? 0) * mv : (rpm.Nov ?? 0),
                                      DecBud = (crossbranch && rpm.Factory == "KSZ") ? (rpm.Dec ?? 0) * mv : (rpm.Dec ?? 0)
                                  });
                    break;
                case "1":
                    ReportData = (from rpm in rp
                                  where rpm.Date == QueryJson.BudgetYear
                                  group rpm by new { rpm.Factory, rpm.DepartmentId, rpm.Version, rpm.ItemId_AssetExpType } into g
                                  join urm in dr on new { g.Key.Factory, g.Key.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { g.Key.Factory, g.Key.DepartmentId, g.Key.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm1 in ic1 on g.Key.ItemId_AssetExpType equals icm1.ItemId
                                  orderby new { g.Key.Factory, g.Key.DepartmentId, g.Key.ItemId_AssetExpType }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = urm.DepartmentName,
                                      AssetExpType = icm1.Name,
                                      ProjectBudget = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.ProjectAmount ?? 0) * mv : g.Sum(rpm => rpm.ProjectAmount ?? 0),
                                      YTDJuly = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.JulLast ?? 0) * mv : g.Sum(rpm => rpm.JulLast ?? 0),
                                      AugTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.AugLast ?? 0) * mv : g.Sum(rpm => rpm.AugLast ?? 0),
                                      SepTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.SepLast ?? 0) * mv : g.Sum(rpm => rpm.SepLast ?? 0),
                                      OctTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.OctLast ?? 0) * mv : g.Sum(rpm => rpm.OctLast ?? 0),
                                      NovTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.NovLast ?? 0) * mv : g.Sum(rpm => rpm.NovLast ?? 0),
                                      DecTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.DecLast ?? 0) * mv : g.Sum(rpm => rpm.DecLast ?? 0),
                                      JanBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jan ?? 0) * mv : g.Sum(rpm => rpm.Jan ?? 0),
                                      FebBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Feb ?? 0) * mv : g.Sum(rpm => rpm.Feb ?? 0),
                                      MarBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Mar ?? 0) * mv : g.Sum(rpm => rpm.Mar ?? 0),
                                      AprBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Apr ?? 0) * mv : g.Sum(rpm => rpm.Apr ?? 0),
                                      MayBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.May ?? 0) * mv : g.Sum(rpm => rpm.May ?? 0),
                                      JunBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jun ?? 0) * mv : g.Sum(rpm => rpm.Jun ?? 0),
                                      JulBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jul ?? 0) * mv : g.Sum(rpm => rpm.Jul ?? 0),
                                      AugBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Aug ?? 0) * mv : g.Sum(rpm => rpm.Aug ?? 0),
                                      SepBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Sep ?? 0) * mv : g.Sum(rpm => rpm.Sep ?? 0),
                                      OctBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Oct ?? 0) * mv : g.Sum(rpm => rpm.Oct ?? 0),
                                      NovBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Nov ?? 0) * mv : g.Sum(rpm => rpm.Nov ?? 0),
                                      DecBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Dec ?? 0) * mv : g.Sum(rpm => rpm.Dec ?? 0)
                                  });
                    break;
                case "2":
                    ReportData = (from rpm in rp
                                  where rpm.Date == QueryJson.BudgetYear
                                  group rpm by new { rpm.Factory, rpm.DepartmentId, rpm.Version, rpm.ItemId_AssetExp, rpm.ItemId_AssetExpType, rpm.ItemId_Purpose } into g
                                  join urm in dr on new { g.Key.Factory, g.Key.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { g.Key.Factory, g.Key.DepartmentId, g.Key.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm in ic on g.Key.ItemId_AssetExp equals icm.ItemId
                                  join icm1 in ic1 on g.Key.ItemId_AssetExpType equals icm1.ItemId
                                  join icm2 in ic2 on g.Key.ItemId_Purpose equals icm2.ItemId
                                  orderby new { g.Key.Factory, g.Key.DepartmentId, g.Key.ItemId_AssetExp, g.Key.ItemId_AssetExpType, g.Key.ItemId_Purpose }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = urm.DepartmentName,
                                      AssetExp = icm.Name,
                                      AssetExpType = icm1.Name,
                                      Purpose = icm2.Name,
                                      ProjectBudget = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.ProjectAmount ?? 0) * mv : g.Sum(rpm => rpm.ProjectAmount ?? 0),
                                      YTDJuly = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.JulLast ?? 0) * mv : g.Sum(rpm => rpm.JulLast ?? 0),
                                      AugTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.AugLast ?? 0) * mv : g.Sum(rpm => rpm.AugLast ?? 0),
                                      SepTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.SepLast ?? 0) * mv : g.Sum(rpm => rpm.SepLast ?? 0),
                                      OctTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.OctLast ?? 0) * mv : g.Sum(rpm => rpm.OctLast ?? 0),
                                      NovTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.NovLast ?? 0) * mv : g.Sum(rpm => rpm.NovLast ?? 0),
                                      DecTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.DecLast ?? 0) * mv : g.Sum(rpm => rpm.DecLast ?? 0),
                                      JanBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jan ?? 0) * mv : g.Sum(rpm => rpm.Jan ?? 0),
                                      FebBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Feb ?? 0) * mv : g.Sum(rpm => rpm.Feb ?? 0),
                                      MarBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Mar ?? 0) * mv : g.Sum(rpm => rpm.Mar ?? 0),
                                      AprBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Apr ?? 0) * mv : g.Sum(rpm => rpm.Apr ?? 0),
                                      MayBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.May ?? 0) * mv : g.Sum(rpm => rpm.May ?? 0),
                                      JunBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jun ?? 0) * mv : g.Sum(rpm => rpm.Jun ?? 0),
                                      JulBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jul ?? 0) * mv : g.Sum(rpm => rpm.Jul ?? 0),
                                      AugBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Aug ?? 0) * mv : g.Sum(rpm => rpm.Aug ?? 0),
                                      SepBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Sep ?? 0) * mv : g.Sum(rpm => rpm.Sep ?? 0),
                                      OctBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Oct ?? 0) * mv : g.Sum(rpm => rpm.Oct ?? 0),
                                      NovBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Nov ?? 0) * mv : g.Sum(rpm => rpm.Nov ?? 0),
                                      DecBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Dec ?? 0) * mv : g.Sum(rpm => rpm.Dec ?? 0)
                                  });
                    break;
                case "3":
                    ReportData = (from rpm in rp
                                  where rpm.Date == QueryJson.BudgetYear
                                  group rpm by new { rpm.Factory, rpm.DepartmentId, rpm.Version, rpm.ItemId_AssetExp, rpm.ProjectName } into g
                                  join urm in dr on new { g.Key.Factory, g.Key.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { g.Key.Factory, g.Key.DepartmentId, g.Key.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm in ic on g.Key.ItemId_AssetExp equals icm.ItemId
                                  orderby new { g.Key.Factory, g.Key.DepartmentId, g.Key.ItemId_AssetExp, g.Key.ProjectName }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = urm.DepartmentName,
                                      AssetExp = icm.Name,
                                      ProjectName = g.Key.ProjectName ?? "",
                                      ProjectBudget = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.ProjectAmount ?? 0) * mv : g.Sum(rpm => rpm.ProjectAmount ?? 0),
                                      YTDJuly = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.JulLast ?? 0) * mv : g.Sum(rpm => rpm.JulLast ?? 0),
                                      AugTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.AugLast ?? 0) * mv : g.Sum(rpm => rpm.AugLast ?? 0),
                                      SepTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.SepLast ?? 0) * mv : g.Sum(rpm => rpm.SepLast ?? 0),
                                      OctTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.OctLast ?? 0) * mv : g.Sum(rpm => rpm.OctLast ?? 0),
                                      NovTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.NovLast ?? 0) * mv : g.Sum(rpm => rpm.NovLast ?? 0),
                                      DecTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.DecLast ?? 0) * mv : g.Sum(rpm => rpm.DecLast ?? 0),
                                      JanBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jan ?? 0) * mv : g.Sum(rpm => rpm.Jan ?? 0),
                                      FebBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Feb ?? 0) * mv : g.Sum(rpm => rpm.Feb ?? 0),
                                      MarBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Mar ?? 0) * mv : g.Sum(rpm => rpm.Mar ?? 0),
                                      AprBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Apr ?? 0) * mv : g.Sum(rpm => rpm.Apr ?? 0),
                                      MayBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.May ?? 0) * mv : g.Sum(rpm => rpm.May ?? 0),
                                      JunBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jun ?? 0) * mv : g.Sum(rpm => rpm.Jun ?? 0),
                                      JulBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jul ?? 0) * mv : g.Sum(rpm => rpm.Jul ?? 0),
                                      AugBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Aug ?? 0) * mv : g.Sum(rpm => rpm.Aug ?? 0),
                                      SepBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Sep ?? 0) * mv : g.Sum(rpm => rpm.Sep ?? 0),
                                      OctBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Oct ?? 0) * mv : g.Sum(rpm => rpm.Oct ?? 0),
                                      NovBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Nov ?? 0) * mv : g.Sum(rpm => rpm.Nov ?? 0),
                                      DecBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Dec ?? 0) * mv : g.Sum(rpm => rpm.Dec ?? 0)
                                  });
                    break;

                case "4":
                    ReportData = (from rpm in rp
                                  where rpm.Date == QueryJson.BudgetYear
                                  group rpm by new { rpm.Factory, rpm.DepartmentId, rpm.Version, rpm.ItemId_AssetExp, rpm.ProjectName, rpm.ItemId_AssetExpType, rpm.ItemId_Purpose } into g
                                  join urm in dr on new { g.Key.Factory, g.Key.DepartmentId } equals new { urm.Factory, urm.DepartmentId }
                                  join fvm in fv on new { g.Key.Factory, g.Key.DepartmentId, g.Key.Version } equals new { fvm.Factory, fvm.DepartmentId, fvm.Version }
                                  join icm in ic on g.Key.ItemId_AssetExp equals icm.ItemId
                                  join icm1 in ic1 on g.Key.ItemId_AssetExpType equals icm1.ItemId
                                  join icm2 in ic2 on g.Key.ItemId_Purpose equals icm2.ItemId
                                  orderby new { g.Key.Factory, g.Key.DepartmentId, g.Key.ItemId_AssetExp, g.Key.ProjectName, g.Key.ItemId_AssetExpType, g.Key.ItemId_Purpose }
                                  select new
                                  {
                                      Factory = g.Key.Factory,
                                      DepartmentName = urm.DepartmentName,
                                      AssetExp = icm.Name,
                                      ProjectName = g.Key.ProjectName ?? "",
                                      AssetExpType = icm1.Name,
                                      Purpose = icm2.Name,
                                      ProjectBudget = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.ProjectAmount ?? 0) * mv : g.Sum(rpm => rpm.ProjectAmount ?? 0),
                                      YTDJuly = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.JulLast ?? 0) * mv : g.Sum(rpm => rpm.JulLast ?? 0),
                                      AugTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.AugLast ?? 0) * mv : g.Sum(rpm => rpm.AugLast ?? 0),
                                      SepTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.SepLast ?? 0) * mv : g.Sum(rpm => rpm.SepLast ?? 0),
                                      OctTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.OctLast ?? 0) * mv : g.Sum(rpm => rpm.OctLast ?? 0),
                                      NovTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.NovLast ?? 0) * mv : g.Sum(rpm => rpm.NovLast ?? 0),
                                      DecTY = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.DecLast ?? 0) * mv : g.Sum(rpm => rpm.DecLast ?? 0),
                                      JanBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jan ?? 0) * mv : g.Sum(rpm => rpm.Jan ?? 0),
                                      FebBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Feb ?? 0) * mv : g.Sum(rpm => rpm.Feb ?? 0),
                                      MarBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Mar ?? 0) * mv : g.Sum(rpm => rpm.Mar ?? 0),
                                      AprBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Apr ?? 0) * mv : g.Sum(rpm => rpm.Apr ?? 0),
                                      MayBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.May ?? 0) * mv : g.Sum(rpm => rpm.May ?? 0),
                                      JunBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jun ?? 0) * mv : g.Sum(rpm => rpm.Jun ?? 0),
                                      JulBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Jul ?? 0) * mv : g.Sum(rpm => rpm.Jul ?? 0),
                                      AugBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Aug ?? 0) * mv : g.Sum(rpm => rpm.Aug ?? 0),
                                      SepBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Sep ?? 0) * mv : g.Sum(rpm => rpm.Sep ?? 0),
                                      OctBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Oct ?? 0) * mv : g.Sum(rpm => rpm.Oct ?? 0),
                                      NovBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Nov ?? 0) * mv : g.Sum(rpm => rpm.Nov ?? 0),
                                      DecBud = (crossbranch && g.Key.Factory == "KSZ") ? g.Sum(rpm => rpm.Dec ?? 0) * mv : g.Sum(rpm => rpm.Dec ?? 0)
                                  });
                    break;
            }
            return ReportData;

        }

        public Object GetReviewReport(string JsonStr)
        {
            MyQueryJson QueryJson = JsonConvert.DeserializeObject<MyQueryJson>(JsonStr);
            Object ObjReportData = null;
            switch (QueryJson.Report)
            {
                case "1":
                    ObjReportData = GetDeptExpenseReportData(JsonStr);
                    break;
                case "2":
                    ObjReportData = GetScrapReportData(JsonStr);
                    break;
                case "3":
                    ObjReportData = GetTravelingReportData(JsonStr);
                    break;
                case "4":
                    ObjReportData = GetKPIReportData(JsonStr);
                    break;
                case "5":
                    ObjReportData = GetHeadCountReportData(JsonStr);
                    break;
                case "6":
                    ObjReportData = GetCapexReportData(JsonStr);
                    break;
            }
            return ObjReportData;
        }

    }
}