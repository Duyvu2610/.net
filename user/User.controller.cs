using Common.Guard;
using Common.Models;
using hrm_server.entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace UserModule;
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
  private readonly UserService _userService;

  public UserController(UserService userService)
  {
    _userService = userService;
  }

  [TypeFilter(typeof(AuthGuard))]
  [HttpGet("profile")]
  public IActionResult GetProfile()
  {
    ResponseModel response = _userService.GetUser(int.Parse(HttpContext.Items["userId"].ToString()));
    if (response.statusCode != 200) return NotFound(response);
    return Ok(response);
  }

  [HttpGet("list")]
  public IActionResult GetUsers([FromQuery] GetUsersQuery query)
  {
    return Ok(_userService.GetUsers(query));
  }

  [TypeFilter(typeof(AuthGuard))]
  [HttpGet]
  public IActionResult GetUser([FromQuery] int id)
  {
    ResponseModel response = _userService.GetUser(id);
    if (response.statusCode != 200) return NotFound(response);
    return Ok(response);
  }

  [HttpPost]
  public IActionResult CreateUser([FromBody] RegisterBody body)
  {
    if (body == null) return BadRequest(new ExceptionModel(400, "BAD REQUEST", new List<string> { }));
    ResponseModel response = _userService.CreateUser(body);
    if (response.statusCode != 200)
    {
      return BadRequest(response);
    }
    return Ok(response);
  }

  [TypeFilter(typeof(AuthGuard))]
  [HttpPut]
  public IActionResult UpdateUser([FromBody] UpdateBody user)
  {
    ResponseModel response = _userService.Update(int.Parse(HttpContext.Items["userId"].ToString()), user);
    if (response.statusCode == 404) return NotFound(response);
    return Ok(response);
  }

  [TypeFilter(typeof(AuthGuard))]
  [HttpPut("github")]
  public IActionResult UpdateGithub([FromBody] UpdateGithubBody body)
  {
    ResponseModel response = _userService.UpdateGithub(int.Parse(HttpContext.Items["userId"].ToString()), body.github);
    if (response.statusCode != 200) return Unauthorized(response);
    return Ok(response);
  }

  [HttpDelete]
  public IActionResult DeleteUser([FromQuery] int id)
  {
    var currentUser = _userService.Delete(id);
    if (currentUser == null) return NotFound();
    return NoContent();
  }

  [TypeFilter(typeof(AuthGuard), Arguments = new object[] { nameof(RoleModel.ADMIN) })]
  [HttpPut("role")]
  public IActionResult UpdateRole([FromBody] UpdateRoleBody body)
  {
    int current = int.Parse(HttpContext.Items["userId"].ToString());
    if (body.Role == nameof(RoleModel.ADMIN) || current == body.UserId)
    {
      return Forbid();
    }
    ResponseModel response = _userService.UpdateRole(body);
    if (response.statusCode != 200)
    {
      return NotFound(response);
    }
    return Ok(response);
  }
}