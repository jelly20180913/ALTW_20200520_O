using System.Web.Http;
using Unity;
using Unity.WebApi;
using WebApi.Service.Implement;
using WebApi.Service.Interface;
using WebApi.ThirdParty.SAP;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models.Repository.EDI.Implement;
using WebApi.Models;
using Unity.Injection;
using WebApi.Common.FileAdapter;
using WebApi.Service.Interface.Table;
using WebApi.Service.Implement.Table;
using WebApi.Service.Interface.Common;
using WebApi.Service.Implement.Common; 
using System.Linq;
using System;
using WebApi.Common.BudgetAdapter;
using WebApi.Common.SapAdapter;
using System.Configuration;
using WebApi.Service.Interface.Vote;
namespace WebApi
{
    public static class UnityConfig
    { 
        /// <summary>
        /// you can register your interface to  inject to controller or service
        /// </summary>
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            //var dbContext = new EDIEntities();  
            //server inner ip : 10.0.0.201
            var _Ip = WebApi.Common.Function.GetIpAddresses();
            string _ConnectionString = "name=EDIEntities";
            if (_Ip.Count() > 0)
            { 
                if (_Ip[_Ip.Count() - 1].ToString() == ConfigurationManager.AppSettings["ServerIp"])
                    _ConnectionString = "name=EDIEntities";
                else
                    _ConnectionString = "name=EDIEntities_TEST";
            }
            var dbContext = new DynamicEntity(_ConnectionString); 
            //service
            container.RegisterType<ISapService, SapService>();
            container.RegisterType<IFileService, FileService>();
            container.RegisterType<Service.Interface.IReportFileService, Service.Implement.ReportFileService>();
            container.RegisterType<ITokenService, TokenService>();
            container.RegisterType<IPosDataService, PosDataService>();
            container.RegisterType<IPosColumnMapService, PosColumnMapService>();
            container.RegisterType<IUploadLogService, UploadLogService>();
            container.RegisterType<ILoginService, LoginService>();
            container.RegisterType<ICommonFileService, CommonFileService>(); 
            container.RegisterType<IFileAdapterFactory, FileAdapterFactory>();
            container.RegisterType<IPosService, PosService>();
            container.RegisterType<IPosOrderMappingService, PosOrderMappingService>();
            container.RegisterType<ICountryService, CountryService>();
            container.RegisterType<IItemCatalogService, ItemCatalogService>();
            container.RegisterType<IBudget_CapexService, Budget_CapexService>();
            container.RegisterType<IBudget_DepartmentReportService, Budget_DepartmentReportService>();
            container.RegisterType<IBudget_DeptExpenseService, Budget_DeptExpenseService>();
            container.RegisterType<IBudget_DeptKPIService, Budget_DeptKPIService>();
            container.RegisterType<IBudget_FileVersionBudgetService, Budget_FileVersionBudgetService>();
            container.RegisterType<IBudget_HeadCountService, Budget_HeadCountService>();
            container.RegisterType<IBudget_HeadCountHRService, Budget_HeadCountHRService>(); 
            container.RegisterType<IBudget_ScrapService, Budget_ScrapService>();
            container.RegisterType<IBudget_TravelingService, Budget_TravelingService>();
            container.RegisterType<Service.Interface.Table.IReportFileService, Service.Implement.Table.ReportFileService>();
            container.RegisterType<IBudgetAdapterFactory, BudgetAdapterFactory>();
            container.RegisterType<IBudgetExcelUploadService, BudgetExcelUploadService>(); 
            container.RegisterType<IBudget_CostService, Budget_CostService>();
            container.RegisterType<IBudget_CostTravelingMappingService, Budget_CostTravelingMappingService>();
            container.RegisterType<IBudget_LoginUploadReportService, Budget_LoginUploadReportService>();
            container.RegisterType<IBudget_LoginUploadCommonCostService, Budget_LoginUploadCommonCostService>();
            container.RegisterType<IBudget_LoginReviewReportService, Budget_LoginReviewReportService>();
            container.RegisterType<IBudget_LoginReviewCommonCostService, Budget_LoginReviewCommonCostService>();
            container.RegisterType<IBudgetLoginReviewService, BudgetLoginReviewService>();
            container.RegisterType<IBudgetReviewReportService, BudgetReviewReportService>();            
	        container.RegisterType<ICommonService, CommonService>();
            container.RegisterType<ISapExcelUploadService,  SapExcelUploadService>();
            container.RegisterType<ISapAdapterFactory, SapAdapterFactory>();
            container.RegisterType<ISAP_PriceListService, SAP_PriceListService>();
            container.RegisterType<ISapEDIService, SapEDIService>();
            container.RegisterType<ISapConnectorInterface, SapConnectorInterface>();
            container.RegisterType<IErrorLogService, ErrorLogService>();
            container.RegisterType<IVote_ItemCatalogService, Vote_ItemCatalogService>();
            container.RegisterType<IVote_MappingService, Vote_MappingService>();
            container.RegisterType<IVoteService, VoteService>();
            container.RegisterType<IPosExcelUploadService, PosExcelUploadService>();
            container.RegisterType<IEdi_PosService, Edi_PosService>();
            container.RegisterType<IButtonLogService, ButtonLogService>();

            container.RegisterType<IEdi_CustomerService, Edi_CustomerService>();
            container.RegisterType<IEdi_SalesHeaderService, Edi_SalesHeaderService>();
            container.RegisterType<IEdi_SalesItemService, Edi_SalesItemService>();
            container.RegisterType<IEdi_SalesPartnerService, Edi_SalesPartnerService>();
            container.RegisterType<IEdi_SalesScheduleService, Edi_SalesScheduleService>();
            container.RegisterType<IEdi_SalesHeader_855Service, Edi_SalesHeader_855Service>();
            container.RegisterType<IEdi_SalesItem_855Service, Edi_SalesItem_855Service>();
            container.RegisterType<IEdi_SalesSchedule_855Service, Edi_SalesSchedule_855Service>();
            container.RegisterType<IEdiService, EdiService>();
            //repository p.s dont forget the table need injected 
            container.RegisterType<IRepository<PosColumnMap>, Repository<PosColumnMap>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<PosData>, Repository<PosData>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<UploadLog>, Repository<UploadLog>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Login>, Repository<Login>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Pos>, Repository<Pos>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<PosOrderMapping>, Repository<PosOrderMapping>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<ReportFile>, Repository<ReportFile>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Country>, Repository<Country>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<ItemCatalog>, Repository<ItemCatalog>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Budget_Capex>, Repository<Budget_Capex>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Budget_DepartmentReport>, Repository<Budget_DepartmentReport>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Budget_DeptExpense>, Repository<Budget_DeptExpense>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Budget_DeptKPI>, Repository<Budget_DeptKPI>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Budget_FileVersionBudget>, Repository<Budget_FileVersionBudget>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Budget_HeadCount>, Repository<Budget_HeadCount>>(new InjectionConstructor(dbContext));           
            container.RegisterType<IRepository<Budget_HeadCountHR>, Repository<Budget_HeadCountHR>>(new InjectionConstructor(dbContext));             
            container.RegisterType<IRepository<Budget_Scrap>, Repository<Budget_Scrap>>(new InjectionConstructor(dbContext));           
            container.RegisterType<IRepository<Budget_Traveling>, Repository<Budget_Traveling>>(new InjectionConstructor(dbContext));   
            container.RegisterType<IRepository<Budget_Cost>, Repository<Budget_Cost>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Budget_CostTravelingMapping>, Repository<Budget_CostTravelingMapping>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Budget_LoginUploadReport>, Repository<Budget_LoginUploadReport>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Budget_LoginUploadCommonCost>, Repository<Budget_LoginUploadCommonCost>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Budget_LoginReviewReport>, Repository<Budget_LoginReviewReport>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Budget_LoginReviewCommonCost>, Repository<Budget_LoginReviewCommonCost>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<SAP_PriceList>, Repository<SAP_PriceList>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<ErrorLog>, Repository<ErrorLog>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Vote_ItemCatalog>, Repository<Vote_ItemCatalog>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Vote_Mapping>, Repository<Vote_Mapping>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Edi_Pos>, Repository<Edi_Pos>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<ButtonLog>, Repository<ButtonLog>>(new InjectionConstructor(dbContext));

            container.RegisterType<IRepository<Edi_Customer>, Repository<Edi_Customer>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Edi_SalesHeader>, Repository<Edi_SalesHeader>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Edi_SalesItem>, Repository<Edi_SalesItem>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Edi_SalesPartner>, Repository<Edi_SalesPartner>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Edi_SalesSchedule>, Repository<Edi_SalesSchedule>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Edi_SalesHeader_855>, Repository<Edi_SalesHeader_855>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Edi_SalesItem_855>, Repository<Edi_SalesItem_855>>(new InjectionConstructor(dbContext));
            container.RegisterType<IRepository<Edi_SalesSchedule_855>, Repository<Edi_SalesSchedule_855>>(new InjectionConstructor(dbContext));
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
       
    }
}