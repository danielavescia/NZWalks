using System.Net;


namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> ilogger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware( ILogger<ExceptionHandlerMiddleware> ilogger, RequestDelegate next)
        {
            this.ilogger = ilogger;
            this.next = next;
        }

        public async Task InvokeAsync( HttpContext httpContext ) 
        {
            try 
            {
                await next( httpContext );
            }catch ( Exception ex ) 
            {
                var errorId = Guid.NewGuid().ToString();
                ilogger.LogError(ex, errorId," : ", ex.Message);

                httpContext.Response.StatusCode = ( int )HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong..."
                };

                await httpContext.Response.WriteAsJsonAsync( error );
            }
        }
    }
}
