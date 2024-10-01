using System.Net;
using System.Text.Json;

namespace api_rota_oeste.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro inesperado.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; // 500 por padrão
            var result = JsonSerializer.Serialize(new { error = "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde." });

            // Exceção personalizada
            if (exception is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest; // 400
                result = JsonSerializer.Serialize(new { error = exception.Message });
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound; // 404
                result = JsonSerializer.Serialize(new { error = "O recurso solicitado não foi encontrado." });
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(result);
        }
    }
}