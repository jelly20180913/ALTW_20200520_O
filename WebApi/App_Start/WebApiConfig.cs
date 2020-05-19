using System.Net.Http.Formatting;
using System.Web.Http;
namespace WebApi
{
    public static class WebApiConfig
    {
        /// <summary>
        /// you can add action filter to this block 
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.AddQueryStringMapping("$format", "json", "application/json");
            //config.Formatters.XmlFormatter.AddQueryStringMapping("$format", "xml", "application/xml");
            //指定action name
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional }
            //);
            //error action filter
            config.Filters.Add(new AllExceptionResponse());
            //result action filter
            //測試下載,暫時取消***
            config.Filters.Add(new ApiResultAttribute());
            //basic authentication
            //  config.Filters.Add(new CustomBasicAuthenticationFilter());
            //use Https (IIS需改,尚未測試)
            //config.Filters.Add(new ForceHttpsAttribute());
            //add LogHandle 
            config.MessageHandlers.Add(new LogHandler());
            // Web API route
            config.MapHttpAttributeRoutes();
            // cross domain need
            config.EnableCors();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //use DI framwork  "Unity" 
            UnityConfig.RegisterComponents();
            //for download
           //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
        }
    }
}
