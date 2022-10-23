using Newtonsoft.Json;
using System.Net;

namespace ResponseCompression.Middleware
{
    public class ResponsePattern
    {
        private readonly RequestDelegate _next;

        public ResponsePattern(RequestDelegate next)
            => _next = next;

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code = HttpStatusCode.InternalServerError;

            if (exception is Exception || context.Response.StatusCode == 500)
            {
                // Aqui salvar o logger...
                code = HttpStatusCode.InternalServerError;
            } 
            // else if (exception is MyException)
            //      code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
