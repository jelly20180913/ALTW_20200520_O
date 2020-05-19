using System.Web.Http.Controllers;

namespace WebApi
{
    public class JwtAuthActionFilter : BaseJwtAuthActionFilter
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.Secret = this.GetType().Name;
            base.OnActionExecuting(actionContext);
        }

         
    }
}