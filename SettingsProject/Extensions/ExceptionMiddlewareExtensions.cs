using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using SettingsContracts.ApiTransaction.ResponseModels;
using SettingsContracts.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SettingsProject.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (InvalidResourceTypeException ivrte)
            {
                Log.Error($"InvalidResourceTypeException Caught: {ivrte}");
                await HandleExceptionAsync(httpContext, ivrte);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception Caught: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new FailureResponse { Error = exception.Message };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response, Formatting.Indented));
        }
    }
}
