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
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var response = new ErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = GetErrorMessage(exception)
            };

            _logger.LogError(exception, "Erro capturado pelo middleware. Status code: {StatusCode}, Mensagem: {Message}", response.StatusCode, response.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private HttpStatusCode GetStatusCode(Exception exception) => exception switch
        {
            ArgumentException => HttpStatusCode.BadRequest,
            KeyNotFoundException => HttpStatusCode.NotFound,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };

        private string GetErrorMessage(Exception exception) => exception switch
        {
            ArgumentException argEx => argEx.Message,
            KeyNotFoundException => "O recurso solicitado não foi encontrado.",
            UnauthorizedAccessException => "Você não tem permissão para acessar este recurso.",
            _ => "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."
        };

        private class ErrorResponse
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
        }
    }
}
