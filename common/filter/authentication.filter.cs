using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using Common.Models;
using Microsoft.AspNetCore.Authentication;

public class MySchemeHandler : IAuthenticationHandler
{
  private HttpContext? _context;

  public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
  {
    _context = context;
    return Task.CompletedTask;
  }

  public Task<AuthenticateResult> AuthenticateAsync()
      => Task.FromResult(AuthenticateResult.NoResult());

  public Task ChallengeAsync(AuthenticationProperties? properties)
  {
    return Task.FromResult(properties);
  }

  public Task ForbidAsync(AuthenticationProperties? properties)
  {
    if (_context != null)
    {
      _context.Response.StatusCode = 403;
    };
    return Task.CompletedTask;
  }
}