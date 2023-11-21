using FabricAir.Files.Common.Exceptions;

namespace FabricAir.Files.Api.Middleware
{
    /// <summary>
    /// Middleware to handle exceptions that occur while processing http requests.
    /// </summary>
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public ExceptionHandlingMiddleware()
        {
        }

        /// <summary>
        /// Is invoked by the previous middleware in the pipeline.
        /// </summary>
        /// <param name="context">The http context.</param>
        /// <param name="next">RequestDelegate to access the next middleware in the pipeline.</param>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                HandleException(context, e);
            }
        }

        private void HandleException<T>(HttpContext context, T exception) where T : Exception
        {
            string message;
            int statusCode;

            switch (exception)
            {
                case NotImplementedException notImplementedException:
                    message = notImplementedException.Message;
                    statusCode = StatusCodes.Status501NotImplemented;
                    break;
                case EntityAlreadyExistsException entityAlreadyExistsException:
                    message = entityAlreadyExistsException.Message;
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                default:
                    message = "An unexpected error occurred.";
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            context.Response.StatusCode = statusCode;
            context.Response
                .WriteAsJsonAsync(message)
                .GetAwaiter()
                .GetResult();
        }
    }
}
