using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Configuration;
using SAP.Middleware.Connector;
using WebApi.ThirdParty.SAP;
using System.Configuration;
namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static string destinationConfigName = "QAS"; 
        protected void Application_Start()
        {
            var _Ip = WebApi.Common.Function.GetIpAddresses();
            //string destinationConfigName = "QAS";
            if (_Ip.Count() > 0)
            {
                if (_Ip[_Ip.Count() - 1].ToString() == ConfigurationManager.AppSettings["ServerIp"])
                {
                    destinationConfigName = ConfigurationManager.AppSettings["SapMode"];
                    // destinationConfigName = "PRD";
                } 
                else
                {
                    destinationConfigName = ConfigurationManager.AppSettings["SapModeTest"];
                   // destinationConfigName = "QAS";
                }
            }

            IDestinationConfiguration destinationConfig = null;
            bool destinationIsInialised = false;
            if (!destinationIsInialised)
            {
                destinationConfig = new SapDestinationConfig();
                destinationConfig.GetParameters(destinationConfigName);
                if (RfcDestinationManager.TryGetDestination(destinationConfigName) == null)
                {
                    RfcDestinationManager.RegisterDestinationConfiguration(destinationConfig);
                    destinationIsInialised = true;
                }
            }

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas(); 
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //強迫只回傳json格式
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

        }
        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    HttpRuntimeSection section = (HttpRuntimeSection)WebConfigurationManager.GetSection("system.web/httpRuntime");
        //    int maxFileSize = section.MaxRequestLength * 1024;
        //    if (Request.ContentLength > maxFileSize)
        //    {
            
        //    }
        //}
    }
}
