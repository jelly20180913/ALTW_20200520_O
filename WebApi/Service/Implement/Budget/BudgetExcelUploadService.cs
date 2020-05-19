using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Service.Interface;
using WebApi.Models;
using System.Data;
using WebApi.Common.BudgetAdapter;
using WebApi.Service.Interface.Table;
using WebApi.Service.Interface.Common;
using WebApi.DataModel.CustomModel.Budget;
namespace WebApi.Service.Implement
{
    public class BudgetExcelUploadService : IBudgetExcelUploadService
    {
        private IUploadLogService _uploadLogService;
        private ILoginService _loginService;
        private IBudgetAdapterFactory _budgetAdapterFactory;
        private ICommonFileService _commonFileService;
        private IBudget_HeadCountHRService _budget_HeadCountHRService;
        private IBudget_FileVersionBudgetService _budget_FileVersionBudgetService;
        private IBudget_HeadCountService _budget_HeadCountService;
        private IBudget_CapexService _budget_CapexService;
        private IBudget_DeptExpenseService _budget_DeptExpenseService;
        private IBudget_DeptKPIService _budget_DeptKPIService;
        private IBudget_ScrapService _budget_ScrapService;
        private IBudget_TravelingService _budget_TravelingService;
        private IItemCatalogService _itemCatalogService;
        private IBudget_LoginUploadReportService _budget_LoginUploadReportService;
        private IBudget_DepartmentReportService _budget_DepartmentReportService;
        private IBudget_LoginUploadCommonCostService _budget_LoginUploadCommonCostService;
        /// <summary>
        /// Dependence Injection
        /// </summary>
        /// <param name="uploadLogSerivice"></param>
        /// <param name="loginService"></param>
        /// <param name="budgetAdapterFactory"></param>
        /// <param name="commonFileService"></param>
        /// <param name="budget_HeadCountHRService"></param>
        /// <param name="budget_FileVersionBudgetService"></param>
        /// <param name="budget_HeadCountService"></param>
        /// <param name="budget_CapexService"></param>
        /// <param name="budget_DeptExpenseService"></param>
        /// <param name="budget_DeptKPIService"></param>
        /// <param name="budget_ScrapService"></param>
        /// <param name="budget_TravelingService"></param>
        /// <param name="itemCatalogService"></param>
        /// <param name="budget_LoginUploadReportService"></param>
        /// <param name="budget_DepartmentReportService"></param>
        /// <param name="budget_LoginUploadCommonCostService"></param>
        public BudgetExcelUploadService(IUploadLogService uploadLogSerivice, ILoginService loginService, BudgetAdapterFactory budgetAdapterFactory, ICommonFileService commonFileService, IBudget_HeadCountHRService budget_HeadCountHRService, IBudget_FileVersionBudgetService budget_FileVersionBudgetService, IBudget_HeadCountService budget_HeadCountService, IBudget_CapexService budget_CapexService, IBudget_DeptExpenseService budget_DeptExpenseService, IBudget_DeptKPIService budget_DeptKPIService, IBudget_ScrapService budget_ScrapService, IBudget_TravelingService budget_TravelingService, IItemCatalogService itemCatalogService, IBudget_LoginUploadReportService budget_LoginUploadReportService, IBudget_DepartmentReportService budget_DepartmentReportService, IBudget_LoginUploadCommonCostService budget_LoginUploadCommonCostService)
        {
            this._uploadLogService = uploadLogSerivice;
            this._loginService = loginService;
            this._budgetAdapterFactory = budgetAdapterFactory;
            this._commonFileService = commonFileService;
            this._budget_HeadCountHRService = budget_HeadCountHRService;
            this._budget_FileVersionBudgetService = budget_FileVersionBudgetService;
            this._budget_HeadCountService = budget_HeadCountService;
            this._budget_CapexService = budget_CapexService;
            this._budget_DeptExpenseService = budget_DeptExpenseService;
            this._budget_DeptKPIService = budget_DeptKPIService;
            this._budget_ScrapService = budget_ScrapService;
            this._budget_TravelingService = budget_TravelingService;
            this._itemCatalogService = itemCatalogService;
            this._budget_LoginUploadReportService = budget_LoginUploadReportService;
            this._budget_DepartmentReportService = budget_DepartmentReportService;
            this._budget_LoginUploadCommonCostService = budget_LoginUploadCommonCostService;
        }
        /// <summary>
        /// 1. upload file (excel)
        /// 2. get file version data to judge you got approved
        /// 3. parse excel column value
        /// 4. batch insert  data 
        /// 5. copy file to success directory 
        /// 6. insert uploadlog
        /// 7. insert update FileVersionBudget
        /// </summary>
        /// <returns></returns>
        public List<string> UploadFile()
        {
            string _ItemId = HttpContext.Current.Request["ItemId"].ToString();
            string _ItemId_CostCommon = "";
            //  string _ItemId = "00000000";  
            string _DepartmentId = HttpContext.Current.Request["DepartmentId"].ToString();
            string _Factory = HttpContext.Current.Request["Factory"].ToString();
            string _Account = HttpContext.Current.Request["Account"].ToString();
            List<string> _ListError = new List<string>();
            string _FilePath = this._commonFileService.Upload();
            int _LoginID = HttpContext.Current.Request["LoginID"].ToString() != "" ? Convert.ToInt32(HttpContext.Current.Request["LoginID"].ToString()) : 0;
            string _TableName = "";
            Budget_FileVersionBudget _Budget_FileVersionBudget = new Budget_FileVersionBudget();
            if (this._itemCatalogService.GetByItemIdInClassName(_ItemId, "CostCommon") != null)
            {
                _Budget_FileVersionBudget = this._budget_FileVersionBudgetService.GetLastVersion(_ItemId, (DateTime.Now.Year + 1).ToString(), _Factory);
                _ItemId_CostCommon = _ItemId;
                _ItemId = "CostCommon";
            }
            else if (_ItemId == "PartTime") { _Budget_FileVersionBudget = this._budget_FileVersionBudgetService.GetLastVersion(_ItemId, (DateTime.Now.Year + 1).ToString(), _Factory); }
            else
            {
                _Budget_FileVersionBudget = this._budget_FileVersionBudgetService.GetLastVersion(_ItemId, _DepartmentId, _Factory, (DateTime.Now.Year + 1).ToString());
            }
            if ((_Budget_FileVersionBudget != null) && (!Convert.ToBoolean(_Budget_FileVersionBudget.Approve))) throw new Exception("please inform Iris(#6802) to approve you duplicate upload ");
            string _Version = _Budget_FileVersionBudget != null ? Convert.ToString(Convert.ToInt32(_Budget_FileVersionBudget.Version) + 1).PadLeft(4, '0') : "0001";
            Budget _Budget = parse(_FilePath, _ItemId, _DepartmentId, _Factory, _Version, _ItemId_CostCommon);

            _ListError = miltiCreate(_ItemId, _Budget, out _TableName);
            bool _Success = _ListError.Count > 1 ? false : true;
            string _ServerFileName = this._commonFileService.SaveToSuccess(_LoginID, _FilePath, _TableName);
            string _UploadLogError = this._commonFileService.InsertUploadLog(_LoginID, _FilePath, _TableName, _Success, _ServerFileName);
            UploadLog _UploadLog = this._uploadLogService.GetAll().Where(x => x.FK_LoginId == _LoginID).OrderByDescending(x => x.Id).First();
            Budget_FileVersionBudget _SetBudget_FileVersionBudget = setBudget_FileVersionBudget(_Budget_FileVersionBudget, _Account, _ItemId, _DepartmentId, _Factory, _UploadLog.Id, _ItemId_CostCommon);
            if (_Budget_FileVersionBudget != null) this._budget_FileVersionBudgetService.Update(_SetBudget_FileVersionBudget);
            else this._budget_FileVersionBudgetService.Create(_SetBudget_FileVersionBudget);
            if (_UploadLogError != "") _ListError.Add(_UploadLogError);
            if (_ListError.Count > 0) _ListError[0] = _ListError[0] + "ms";
            return _ListError;
        }
        /// <summary>
        /// according to budget type batch insert data
        /// </summary>
        /// <param name="type">budget type</param>
        /// <param name="budget"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private List<string> miltiCreate(string type, Budget budget, out string tableName)
        {
            tableName = "";
            List<string> _ListError = new List<string>();
            switch (type)
            {
                case "HeadCountHR":
                    _ListError = this._budget_HeadCountHRService.MiltiCreate(budget.HeadCountHR);
                    tableName = "Budget_HeadCountHR";
                    break;
                case "PartTime":
                case "Headcount":
                    _ListError = this._budget_HeadCountService.MiltiCreate(budget.HeadCount);
                    tableName = "Budget_HeadCount";
                    break;
                case "Scrap":
                    _ListError = this._budget_ScrapService.MiltiCreate(budget.Scrap);
                    tableName = "Budget_Scrap";
                    break;
                case "KPI":
                    _ListError = this._budget_DeptKPIService.MiltiCreate(budget.DeptKPI);
                    tableName = "Budget_DeptKPI";
                    break;
                case "DeptExpense":
                case "CostCommon":
                    _ListError = this._budget_DeptExpenseService.MiltiCreate(budget.DeptExpense);
                    tableName = "Budget_DeptExpense";
                    break;
                case "Capex":
                    _ListError = this._budget_CapexService.MiltiCreate(budget.Capex);
                    tableName = "Budget_Capex";
                    break;
                case "Travelling":
                    _ListError = this._budget_TravelingService.MiltiCreate(budget.Traveling);
                    _ListError = this._budget_DeptExpenseService.MiltiCreate(budget.DeptExpense);
                    tableName = "Budget_Travelling";
                    break;
                case "ItemCatalog":
                    _ListError = this._itemCatalogService.MiltiCreate(budget.ItemCatalog);
                    tableName = "ItemCatalog";
                    break;
            }
            return _ListError;
        }

        /// <summary>
        ///  use budget adatper factory to produce budget object
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="itemId"></param>
        /// <param name="departmentId"></param>
        /// <param name="factory"></param>
        /// <param name="version"></param>
        /// <param name="itemId_CostCommon"></param>
        private Budget parse(string filePath, string itemId, string departmentId, string factory, string version, string itemId_CostCommon)
        {
            BudgetBase _Base = _budgetAdapterFactory.CreateBudgetAdapter(itemId);
            _Base.DepartmentId = departmentId;
            _Base.Factory = factory;
            _Base.Version = version;
            if (itemId_CostCommon != "")
            {
                _Base.CommonBudget = true;
                _Base.ItemId_CostCommon = itemId_CostCommon;
            }
            if (itemId == "PartTime")
                _Base.PartTime = true;
            Budget _Budget = _Base.Parse(filePath);
            if (_Base.ListError.Count() != 0)
            {
                string _Error = string.Join("\r\n", _Base.ListError.ToArray());
                throw new Exception(_Error);
            }
            return _Budget;
        }
        /// <summary>
        /// insert Budget_FileVersionBudget data  
        /// </summary>
        /// <param name="_Budget_FileVersionBudget"></param>
        /// <param name="account">altw id</param>
        /// <param name="itemId_BudgetName">budget item id</param>
        /// <param name="departmentId">department id</param>
        /// <param name="factory">LT/KS</param>
        /// <param name="uploadLogId"></param>
        /// <param name="itemId_CostCommon"></param>
        /// <returns></returns>
        private Budget_FileVersionBudget setBudget_FileVersionBudget(Budget_FileVersionBudget _Budget_FileVersionBudget, string account, string itemId_BudgetName, string departmentId, string factory, int uploadLogId, string itemId_CostCommon)
        {
            string _Version = _Budget_FileVersionBudget != null ? Convert.ToString(Convert.ToInt32(_Budget_FileVersionBudget.Version) + 1).PadLeft(4, '0') : "0001";
            if (_Budget_FileVersionBudget == null) _Budget_FileVersionBudget = new Budget_FileVersionBudget();
            _Budget_FileVersionBudget.Date = (DateTime.Now.Year + 1).ToString();
            _Budget_FileVersionBudget.Version = _Version;
            _Budget_FileVersionBudget.Approve = false;
            _Budget_FileVersionBudget.Account = account;
            _Budget_FileVersionBudget.ItemId_BudgetName = itemId_CostCommon != "" ? itemId_CostCommon : itemId_BudgetName;
            _Budget_FileVersionBudget.DepartmentId = departmentId;
            _Budget_FileVersionBudget.Factory = factory;
            _Budget_FileVersionBudget.UploadLogId = uploadLogId;
            return _Budget_FileVersionBudget;
        }
        /// <summary>
        /// department dropdownlist value
        /// </summary>
        /// <param name="account"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public List<DropDownDepartment> GetDepartmentByAccount(string account, string factory)
        {
            List<DropDownDepartment> _DropDownDepartmentList = new List<DropDownDepartment>();
            var results = this._budget_LoginUploadReportService.GetAll().Join(this._budget_DepartmentReportService.GetAll(),
                 LoginUploadReport => new { LoginUploadReport.DepartmentId, LoginUploadReport.Factory },
                 DepartmentReport => new { DepartmentReport.DepartmentId, DepartmentReport.Factory },
                 (LoginUploadReport, DepartmentReport) => new
                 {
                     LoginUploadReport,
                     DepartmentReport
                 }).Where(x => x.LoginUploadReport.Factory == factory && x.LoginUploadReport.Account == account).ToList();
            foreach (var r in results)
            {
                DropDownDepartment _DropDownDepartment = new DropDownDepartment();
                _DropDownDepartment.DepartmentId = r.LoginUploadReport.DepartmentId;
                _DropDownDepartment.DepartmentName = r.DepartmentReport.DepartmentName;
                _DropDownDepartmentList.Add(_DropDownDepartment);
            }
            return _DropDownDepartmentList;
        }
        /// <summary>
        /// budget type dropdownlist value
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public List<Budget_DepartmentReport> GetBudgetTypeByDepartmentId(string factory, string departmentId)
        {
            List<Budget_DepartmentReport> _Budget_DepartmentReportList = new List<Budget_DepartmentReport>();
            _Budget_DepartmentReportList = this._budget_DepartmentReportService.GetAll().Where(x => x.Factory == factory && x.DepartmentId == departmentId).ToList();
            return _Budget_DepartmentReportList;
        }
        /// <summary>
        /// common cost dropdownlist value
        /// </summary>
        /// <param name="account"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public List<DropDownItemCatalog> GetCommonCostByAccount(string account, string factory)
        {

            List<DropDownItemCatalog> _DropDownItemCatalogList = new List<DropDownItemCatalog>();
            var results = this._budget_LoginUploadCommonCostService.GetAll().Join(this._itemCatalogService.GetAll(),
                LoginUploadCommonCost => LoginUploadCommonCost.ItemId_CostCommon,
                Item => Item.ItemId,
                (LoginReviewCommonCost, Item) => new
                {
                    LoginReviewCommonCost,
                    Item
                }).Where(x => x.LoginReviewCommonCost.Factory == factory && x.LoginReviewCommonCost.Account == account).ToList();

            foreach (var r in results)
            {
                DropDownItemCatalog _DropDownItemCatalog = new DropDownItemCatalog();
                _DropDownItemCatalog.ItemId = r.LoginReviewCommonCost.ItemId_CostCommon;
                _DropDownItemCatalog.Name = r.Item.Name;
                _DropDownItemCatalogList.Add(_DropDownItemCatalog);
            }
            return _DropDownItemCatalogList;
        }
        /// <summary>
        /// you need to approve the row data that the user can duplicate upload
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool UpdateFileVersionBudgetApprove(int Id)
        {
            bool _Success = false;
            Budget_FileVersionBudget _Budget_FileVersionBudget = this._budget_FileVersionBudgetService.GetByID(Id);
            _Budget_FileVersionBudget.Approve = true;
            this._budget_FileVersionBudgetService.Update(_Budget_FileVersionBudget);
            _Success = true;
            return _Success;
        }
        /// <summary>
        /// Iris can get FileVersionBudget table for approve duplicate upload
        /// </summary>
        /// <returns></returns>
        public List<FileVersionBudget> GetFileVersionBudgetList()
        {
            List<FileVersionBudget> _FileVersionBudgetList = new List<FileVersionBudget>();
            List<Budget_FileVersionBudget> _Budget_FileVersionBudgetList = this._budget_FileVersionBudgetService.GetAll().ToList();
            List<Budget_HeadCountHR> _Budget_HeadCountHRList = this._budget_HeadCountHRService.GetAll().ToList();
            List<ItemCatalog> _ItemCatalogList = this._itemCatalogService.GetAll().ToList();
            List<Budget_DepartmentReport> _Budget_DepartmentReport = this._budget_DepartmentReportService.GetAll().ToList();
            var results = _Budget_FileVersionBudgetList.Join(_Budget_HeadCountHRList,
               p => p.Account, q => q.Account, (p, q) => new { p, q }).Join(_ItemCatalogList,
               pp => pp.p.ItemId_BudgetName, r => r.ItemId, (pp, r) => new { pp, r }).Join(_Budget_DepartmentReport,
               ppp => ppp.pp.p.DepartmentId, s => s.DepartmentId, (ppp, s) => new { ppp, s }
                ).Select(x => new
                {
                    Id = x.ppp.pp.p.Id,
                    Date = x.ppp.pp.p.Date,
                    Version = x.ppp.pp.p.Version,
                    Approve = x.ppp.pp.p.Approve,
                    Account = x.ppp.pp.p.Account,
                    Factory = x.ppp.pp.p.Factory,
                    AltwName = x.ppp.pp.q.AltwName,
                    BudgetName = x.ppp.r.Name,
                    DepartmentName = x.s.DepartmentName
                }).OrderBy(x => x.AltwName).ToList();

            foreach (var r in results.Distinct())
            {
                FileVersionBudget _FileVersionBudget = new FileVersionBudget();
                _FileVersionBudget.Id = r.Id;
                _FileVersionBudget.Date = r.Date;
                _FileVersionBudget.Version = r.Version;
                _FileVersionBudget.Approve = r.Approve;
                _FileVersionBudget.Account = r.Account;
                _FileVersionBudget.Factory = r.Factory;
                _FileVersionBudget.AltwName = r.AltwName;
                _FileVersionBudget.BudgetName = r.BudgetName;
                _FileVersionBudget.DepartmentName = r.DepartmentName;
                _FileVersionBudgetList.Add(_FileVersionBudget);
            }
            return _FileVersionBudgetList;
        }
        /// <summary>
        /// peggy login get employment data 
        /// </summary>
        /// <returns></returns>
        public List<HeadCountHR> GetHeadCountHRList()
        {
            List<HeadCountHR> _HeadCountHRList = new List<HeadCountHR>();
            List<ItemCatalog> _ItemCatalog = this._itemCatalogService.GetAll().ToList();
            //var results = this._budget_HeadCountHRService.GetAll().Join(this._budget_DepartmentReportService.GetAll(),
            //    p => p.DepartmentId, q => q.DepartmentId, (p, q) => new { p, q }).Select(x => new
            //    {
            //        Id = x.p.Id,
            //        // DepartmentId = x.p.DepartmentId,
            //        Account = x.p.Account,
            //        AltwName = x.p.AltwName,
            //        //  DirectStaff = x.p.DirectStaff,
            //        ItemId_HR = x.p.ItemId_HR,
            //        //  PartTime = x.p.PartTime,
            //        ItemId_DirectType = x.p.ItemId_DirectType,
            //        IsDel = x.p.IsDel,
            //        ItemId_JobFunction = x.p.ItemId_JobFunction,
            //        DepartmentName = x.q.DepartmentName,
            //        ItemId_Title = x.p.ItemId_Title
            //    }).ToList();
            var results = this._budget_HeadCountHRService.GetAll().OrderByDescending(x => x.Id).ToList();
            foreach (var r in results.Distinct())
            {
                string _JobFunction = "", _HR = "", _Title = "", _DirectType = "", _JobFunctionItem = "", _TitleItem = "";
                HeadCountHR _HeadCountHR = new HeadCountHR();
                _HeadCountHR.Id = r.Id;
                //_HeadCountHR.DepartmentId = r.DepartmentId;
                _HeadCountHR.Account = r.Account;
                _JobFunctionItem = r.Account.StartsWith("L") ? "JobFunction" : "JobFunctionKSZ";
                _TitleItem = r.Account.StartsWith("L") ? "Title" : "TitleKSZ";
                _HeadCountHR.AltwName = r.AltwName;
                if (r.ItemId_DirectType != null)
                    _DirectType = _ItemCatalog.Where(x => x.ClassName == "DirectType" && x.ItemId.Trim() == r.ItemId_DirectType.Trim()).ToList().Count > 0 ? _ItemCatalog.Where(x => x.ClassName == "DirectType" && x.ItemId.Trim() == r.ItemId_DirectType.Trim()).First().Name : "";
                _HeadCountHR.DirectType = _DirectType;
                if (r.ItemId_JobFunction != null)
                    _JobFunction = _ItemCatalog.Where(x => x.ClassName == _JobFunctionItem && x.ItemId.Trim() == r.ItemId_JobFunction.Trim()).ToList().Count > 0 ? _ItemCatalog.Where(x => x.ClassName == _JobFunctionItem && x.ItemId.Trim() == r.ItemId_JobFunction.Trim()).First().Name : "";
                _HeadCountHR.JobFunction = _JobFunction;
                if (r.ItemId_HR != null)
                    _HR = _ItemCatalog.Where(x => x.ClassName == "HR" && x.ItemId.Trim() == r.ItemId_HR.Trim()).ToList().Count > 0 ? _ItemCatalog.Where(x => x.ClassName == "HR" && x.ItemId.Trim() == r.ItemId_HR.Trim()).First().Name : "";
                _HeadCountHR.HR = _HR;
                if (r.ItemId_Title != null)
                    _Title = _ItemCatalog.Where(x => x.ClassName == _TitleItem && x.ItemId.Trim() == r.ItemId_Title.Trim()).ToList().Count > 0 ? _ItemCatalog.Where(x => x.ClassName == _TitleItem && x.ItemId.Trim() == r.ItemId_Title.Trim()).First().Name : "";
                _HeadCountHR.Title = _Title;
                //_HeadCountHR.DepartmentName = r.DepartmentName;
                _HeadCountHRList.Add(_HeadCountHR);
            }
            return _HeadCountHRList;
        }
        /// <summary>
        /// use this function to modify employment data
        /// </summary>
        /// <param name="headCountHR"></param>
        /// <returns></returns>
        public bool UpdateHeadCountHR(HeadCountHR headCountHR)
        {
            bool _Success = false;
            string _JobFunctionItem = "", _TitleItem = "";
            //just get item list once can approve performance
            _JobFunctionItem = headCountHR.Account.StartsWith("L") ? "JobFunction" : "JobFunctionKSZ";
            _TitleItem = headCountHR.Account.StartsWith("L") ? "Title" : "TitleKSZ";
            List<ItemCatalog> _ItemCatalog = this._itemCatalogService.GetAll().ToList();
            List<Budget_DepartmentReport> _Budget_DepartmentReportList = this._budget_DepartmentReportService.GetAll().ToList();
            Budget_HeadCountHR _HeadCountHR = this._budget_HeadCountHRService.GetByID(headCountHR.Id);
            _HeadCountHR.Id = headCountHR.Id;
            _HeadCountHR.Account = headCountHR.Account;
            _HeadCountHR.AltwName = headCountHR.AltwName;
            _HeadCountHR.ItemId_HR = _ItemCatalog.Where(x => x.ClassName == "HR" && x.Name.Trim() == headCountHR.HR.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == "HR" && x.Name == headCountHR.HR).First().ItemId : "";
            _HeadCountHR.ItemId_DirectType = _ItemCatalog.Where(x => x.ClassName == "DirectType" && x.Name.Trim() == headCountHR.DirectType.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == "DirectType" && x.Name.Trim() == headCountHR.DirectType.Trim()).First().ItemId : "";
            _HeadCountHR.ItemId_JobFunction = _ItemCatalog.Where(x => x.ClassName == _JobFunctionItem && x.Name.Trim() == headCountHR.JobFunction.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == _JobFunctionItem && x.Name.Trim() == headCountHR.JobFunction.Trim()).First().ItemId : "";
            _HeadCountHR.ItemId_Title = _ItemCatalog.Where(x => x.ClassName == _TitleItem && x.Name.Trim() == headCountHR.Title.Trim()) != null ? _ItemCatalog.Where(x => x.ClassName == _TitleItem && x.Name.Trim() == headCountHR.Title.Trim()).First().ItemId : "";

            this._budget_HeadCountHRService.Update(_HeadCountHR);
            _Success = true;
            return _Success;
        }
        public List<Budget_DepartmentReport> GetDepartment()
        {
            List<Budget_DepartmentReport> _Budget_DepartmentReportList = this._budget_DepartmentReportService.GetAll().ToList();
            return _Budget_DepartmentReportList;
        }
        /// <summary>
        /// if you change employee account ,and  synchronize headcount data
        /// </summary>
        /// <param name="headCountHR"></param>
        /// <returns></returns>
        public bool UpdateHeadCount(HeadCountHR headCountHR)
        {
            bool _Success = false;
            try
            {
                Budget_HeadCountHR _HeadCountHR = this._budget_HeadCountHRService.GetByID(headCountHR.Id);
                if (headCountHR.Account != _HeadCountHR.Account)
                {
                    List<Budget_HeadCount> _HeadCountList = this._budget_HeadCountService.GetAll().Where(x => x.Account == _HeadCountHR.Account).ToList();
                    foreach (Budget_HeadCount h in _HeadCountList)
                    {
                        h.Account = headCountHR.Account;
                        this._budget_HeadCountService.Update(h);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            _Success = true;
            return _Success;
        }
    }
}