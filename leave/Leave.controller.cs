using Common.Guard;
using Common.Models;
using hrm_server.entities;
using Microsoft.AspNetCore.Mvc;

namespace LeaveModule;
[Route("api/leaves")]
[ApiController]
public class LeaveController : ControllerBase
{
  private readonly LeaveService _leaveService;
  public LeaveController(LeaveService leaveService)
  {
    _leaveService = leaveService;
  }

  [TypeFilter(typeof(AuthGuard))]
  [HttpGet("user")]
  public IActionResult PersonalLeaves([FromQuery] LeavePer query)
  {
    ResponseModel response = _leaveService.leaves(int.Parse(HttpContext.Items["userId"].ToString()), query.type, query.date);
    if (response.statusCode != 200)
    {
      return Unauthorized(response);
    }
    return Ok(response);
  }

  [TypeFilter(typeof(AuthGuard))]
  [HttpGet]
  public IActionResult Leaves([FromQuery] LeaveQuery query)
  {
    ResponseModel response = _leaveService.leavesAll(query.type, query.month, query.year);
    if (response.statusCode != 200)
    {
      return Unauthorized(response);
    }
    return Ok(response);
  }

  [TypeFilter(typeof(AuthGuard))]
  [HttpPost]
  public IActionResult CreateLeave([FromBody] LeaveRequest body)
  {
    if (body == null) return BadRequest(new ExceptionModel(400, "BAD REQUEST", new List<string> { }));
    ResponseModel response = _leaveService.LeaveRequest(int.Parse(HttpContext.Items["userId"].ToString()), body);
    if (response.statusCode != 200)
    {
      return BadRequest(response);
    }
    return Ok(response);
  }
  [TypeFilter(typeof(AuthGuard), Arguments = new object[] { nameof(RoleModel.HR) })]
  [HttpPost("approve")]
  public IActionResult ApproveRequest([FromQuery] int leaveId)
  {
    int acceptedBy = int.Parse(HttpContext.Items["userId"].ToString());
    ResponseModel response = _leaveService.ApproveRequest(leaveId, acceptedBy);

    if (response.statusCode == 400)
    {
      return BadRequest(response);
    }
    if (response.statusCode == 404)
    {
      return NotFound(response);
    }
    return Ok(response);
  }
  [TypeFilter(typeof(AuthGuard), Arguments = new object[] { nameof(RoleModel.HR) })]
  [HttpPost("reject")]
  public IActionResult RejectRequest([FromQuery] int leaveId)
  {
    int rejectedBy = int.Parse(HttpContext.Items["userId"].ToString());
    ResponseModel response = _leaveService.RejectRequest(leaveId, rejectedBy);

    if (response.statusCode == 400)
    {
      return BadRequest(response);
    }
    if (response.statusCode == 404)
    {
      return NotFound(response);
    }
    return Ok(response);
  }
}