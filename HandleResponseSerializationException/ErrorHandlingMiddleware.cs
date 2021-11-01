using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace HandleResponseSerializationException
{
    public sealed class ErrorHandlingMiddleware
    {

        private readonly RequestDelegate _next;

        /// <summary>Конструктор</summary>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                var errorResult = GetErrorResult(ex);
                var statusCode = GetStatusCode(ex);
                errorResult.StatusCode = statusCode;

                if (context.Response.HasStarted)
                {
                    // Here is no way to handle serialization exception and pass ErrorResult instance to client
                    throw;
                }

                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsJsonAsync(errorResult).ConfigureAwait(true);
            }
        }

        private static int GetStatusCode(Exception exception)
        {
            return (int)(exception switch
                            {
                                _ => HttpStatusCode.InternalServerError
                            });
        }


        private static ErrorResult GetErrorResult(Exception exception)
        {
            return new ErrorResult(exception.Message, exception.StackTrace, Guid.NewGuid().ToString(), default);
        }
    }

    internal record ErrorResult(string Message, string? StackTrace, string ErrorId, ErrorResult? OriginalError)
    {
        public int? StatusCode { get; set; } = 500;
    };
}