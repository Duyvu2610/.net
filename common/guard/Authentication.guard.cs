using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.Guard;

class AuthGuard : Attribute, IAuthorizationFilter
{
  public JWTService _jwtService { get; set; }
  public string _role;

  public AuthGuard(JWTService jwtService, string role = nameof(RoleModel.EMP))
  {
    _jwtService = jwtService;
    _role = role;
  }

  public void OnAuthorization(AuthorizationFilterContext context)
  {
    string[] bearer = context.HttpContext.Request.Headers.Authorization.ToString().Split(" ");
    if (bearer.Length < 2)
    {
      context.Result = new UnauthorizedObjectResult(
        new ExceptionModel(401, "UNAUTHORIZED", new List<string>())
      );
      return;
    }
    try
    {
      JWTPayload payload = _jwtService.verifyToken(bearer[1]);
      int required = Array.IndexOf(Enum.GetNames(typeof(RoleModel)), _role);
      int currentRole = Array.IndexOf(Enum.GetNames(typeof(RoleModel)), payload.role);
      if (currentRole < required)
      {
        context.Result = new ForbidResult();
        return;
      }

      bool isExpried = _jwtService.isExpried(payload.id, bearer[1]);
      if (isExpried)
      {
        context.Result = new UnauthorizedObjectResult(
          new ExceptionModel(401, "UNAUTHORIZED", new List<string>())
        );
        return;
      }

      context.HttpContext.Items.Add("userId", payload.id);
    }
    catch
    {
      context.Result = new UnauthorizedObjectResult(
        new ExceptionModel(401, "UNAUTHORIZED", new List<string>())
      );
    }
  }
}