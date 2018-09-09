using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Product.Api.Contract;
using Product.Api.DomainCore;
using Product.Api.DomainCore.Exceptions;
using Product.Api.DomainCore.Exceptions.ClientErrors;

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

            if (exception is ClientError)
            {
                if (exception is ValidationException)
                {
                    ClientError clientError = exception as ClientError;
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    var responseContent = GetErrorsResponseContent(clientError.Faults);
                    return context.Response.WriteAsync(JsonConvert.SerializeObject(responseContent));
                }
                if (exception is NotFoundException)
                {
                    ClientError clientError = exception as ClientError;
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    var responseContent = GetErrorsResponseContent(clientError.Faults);
                    return context.Response.WriteAsync(JsonConvert.SerializeObject(responseContent));
                }
            }

            if (exception is ServerError)
            {
                ServerError clientError = exception as ServerError;
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                var responseContent = GetErrorsResponseContent(clientError.Faults);
                return context.Response.WriteAsync(JsonConvert.SerializeObject(responseContent));
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new Error()
            {
                Reason = "InternalServerError",
                Message = "Internal Server Error, unable to process request."
            }));
        }

        private static Response GetErrorsResponseContent(List<Fault> faults)
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
