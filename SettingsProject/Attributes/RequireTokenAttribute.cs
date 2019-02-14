using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using SettingsContracts.DatabaseModels;

namespace SettingsProject.Attributes
{
    public class RequireTokenAttribute : ActionFilterAttribute
    {
        private readonly IMemoryCache _cache;

        public RequireTokenAttribute(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Require the addition of a bearer token for the tagged endpoint.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string bearerToken = context.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (bearerToken.ToLower().IndexOf("bearer ") < 0)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string token = bearerToken.Substring(7);

            if (!_cache.TryGetValue(token, out Permissions cachedItem))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            context.ActionArguments.Add("access", cachedItem);
            context.RouteData.Values.Add("access", cachedItem);
            base.OnActionExecuting(context);
        }
    }

}
