using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Product.Api.Contract;
using Product.Api.DomainCore;
using Product.Api.DomainCore.Exceptions;

namespace Product.Api.LocalInfrastructure
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is ApiError clientError)
            {
                context.Response.StatusCode = (int)clientError.StatusCode;
                Response responseContent = GetErrorsResponseContent(clientError.GetErrors());
                return context.Response.WriteAsync(JsonConvert.SerializeObject(responseContent));
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new Error()
            {
                Reason = "InternalServerError",
                Message = "Internal Server Error, unable to process request."
            }));
        }

        private static Response GetErrorsResponseContent(IReadOnlyCollection<Fault> faults)
        {
            var response = new Response();
            foreach (var fault in faults)
            {
                response.Errors.Add(new Error(){Reason = fault.Reason, Message = fault.Message});
            }

            return response;
        }
    }
}
