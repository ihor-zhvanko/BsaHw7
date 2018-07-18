using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;
using Airport.Common.Exceptions;

namespace Airport.Api.Middleware
{
  // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
  public class ErrorHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly IHostingEnvironment _env;

    public ErrorHandlingMiddleware(RequestDelegate next, IHostingEnvironment env)
    {
      _next = next;
      _env = env;
    }

    public async Task Invoke(HttpContext context)
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

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      var code = HttpStatusCode.InternalServerError; // 500 if unexpected

      if (exception is NotFoundException)
        code = HttpStatusCode.NotFound;
      else if (exception is BadRequestException)
        code = HttpStatusCode.BadRequest;

      var resp = new
      {
        error = exception.Message,
        stack = _env.IsDevelopment() ? exception.StackTrace : ""
      };

      var result = JsonConvert.SerializeObject(resp);
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)code;
      return context.Response.WriteAsync(result);
    }
  }

  public static class ErrorHandlingMiddlewareExtensions
  {
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
  }
}