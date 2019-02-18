﻿using Microsoft.AspNetCore.Builder;
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
                await HandleExceptionAsync(httpContext, ivrte, "InvalidResourceTypeException");
            }
            catch (Exception ex)
            {
                Log.Error($"Exception Caught: {ex}");
                await HandleExceptionAsync(httpContext, ex, "Exception");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, string type)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new FailureResponse(type, exception.Message);

            return context.Response.WriteAsync(response.Json);
        }
    }
}
