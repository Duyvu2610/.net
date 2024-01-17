using Common.Config;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class HttpResponseExceptionFilter : IActionFilter
{

  private PostgresConfig _pg;

  public HttpResponseExceptionFilter(PostgresFactory pg) {
    _pg = pg.CreateDbContext();
  }

  public void OnActionExecuting(ActionExecutingContext context)
  {
  }

  public void OnActionExecuted(ActionExecutedContext context)
  {
    if (context.Exception != null)
    {
      Console.WriteLine(context.Exception);
      _pg.ChangeTracker.Clear();
      context.Result = new ObjectResult(new ExceptionModel(500, "INTERNAL SERVER ERROR", new List<string>()));
      context.ExceptionHandled = true;
    }
  }
}