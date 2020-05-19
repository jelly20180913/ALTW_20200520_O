using System.Web.Http.Controllers;

namespace WebApi
{
    public class JwtAuthActionForCustomer : BaseJwtAuthActionFilter
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // TODO: use class name as secret key 
            base.Secret = this.GetType().Name;
            base.OnActionExecuting(actionContext);
        }
    }
}