using Common.Guard;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
namespace AuthenticationModule;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
  private readonly AuthenticationService _authService;

  public AuthenticationController(AuthenticationService authService)
  {
    _authService = authService;
  }

  [HttpPost]
  public IActionResult login([FromBody] LoginBody body)
  {
    ResponseModel response = _authService.Login(body);
    if (response.statusCode != 200)
    {
      return BadRequest(response);
    }
    return Ok(response);
  }

  [HttpPost("logout")]
  public IActionResult logout()
  {
    string[] bearer = Request.Headers.Authorization.ToString().Split(" ");
    if (bearer.Length < 2)
    {
      return Unauthorized(new ExceptionModel(401, "UNAUTHORIED", new List<string>()));
    }
    ResponseModel response = _authService.Logout(bearer[1]);
    return Ok(response);
  }

  [HttpPost("refresh")]
  public IActionResult refresh([FromBody] RefreshBody body) {
    ResponseModel response = _authService.refresh(body.RefreshToken);
    if (response.statusCode != 200)
    {
      return Unauthorized(response);
    }
    return Ok(response);
  }

  [TypeFilter(typeof(AuthGuard))]
  [HttpGet("expried")]
  public IActionResult expried() {
    return Ok(new ResponseModel(200, "SUCCESSFULLY"));
  }
  
  [TypeFilter(typeof(AuthGuard))]
  [HttpPut("password")]
  public IActionResult changePassword([FromBody] ChangePasswordBody body)
  {
    ResponseModel response = _authService.changePassword(int.Parse(HttpContext.Items["userId"].ToString()), body);
    if (response.statusCode == 404) return NotFound(response);
    if (response.statusCode == 400) return BadRequest(response);
    return Ok(response);
  }
}