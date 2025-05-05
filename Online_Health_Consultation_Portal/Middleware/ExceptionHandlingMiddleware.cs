using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Services.Interfaces.Logging;
using Serilog.Core;

namespace Online_Health_Consultation_Portal.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DbUpdateException ex)
            {
                var logger = context.RequestServices.GetService<IApplogger<ExceptionHandlingMiddleware>>();
                context.Response.ContentType = "application/json";
                if (ex.InnerException is SqlException innerException)
                {

                    switch (innerException.Number)
                    {
                        case 2627: //Unique constraint violation 
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                            await context.Response.WriteAsync("Unique constrain violation");
                            break;
                        case 515: //cannot insert null
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync("Cannot insert null");
                            break;
                        case 547://foreign key constrain violation
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                            await context.Response.WriteAsync("foregin key constrain violation");
                            break;
                        default:
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync("An error occrurred while processing your request");
                            break;
                    }
                }
                else
                {
                    logger.LogError(ex, "Relates EFCore Exception");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("An Error occurred while saving the entity changes.");

                }

            }
            catch (Exception ex)
            {
                var logger = context.RequestServices.GetRequiredService<IApplogger<ExceptionHandlingMiddleware>>();
                logger.LogError(ex, "Unknown Exception");
                context.Response.ContentType = "Application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An Error occurred: " + ex.Message);
            }
        }

    }
}
