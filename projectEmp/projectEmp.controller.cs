using System.Runtime.CompilerServices;
using Common.Guard;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/projects")]
[ApiController]
public class ProjectEpmsController : ControllerBase
{
  private readonly ProjectEmpService _projectEmpService;
  public ProjectEpmsController(ProjectEmpService projectEmpService)
  {
    _projectEmpService = projectEmpService;
  }

  [HttpGet("members")]
  public IActionResult Members([FromQuery] MembersParams param)
  {
    if (param.projectId == null)
    {
      return Ok(new MembersResponse(new List<object>()));
    }
    return Ok(_projectEmpService.members(param));
  }

  [HttpPost("members")]
  public IActionResult Create([FromBody] CreateProjectEmpModel body)
  {
    ResponseModel response = _projectEmpService.create(body);
    return Ok(response);
  }

  [HttpPut("members/clickup")]
  public IActionResult MembersSync([FromBody] SyncMemberBody body)
  {
    if (body.data.Count() <= 0)
    {
      return Ok(new ResponseModel(200, "SUCCESSFULLY"));
    }
    ResponseModel response = _projectEmpService.syncClickup(body);
    return Ok(response);
  }

  [HttpGet("members/free")]
  public IActionResult MemberFree()
  {
    return Ok(_projectEmpService.MemberFree());
  }

  [TypeFilter(typeof(AuthGuard), Arguments = new object[] {nameof(RoleModel.PM)})]
  [HttpDelete("members")]
  public IActionResult DeleteFromProject([FromQuery] DeleteMemberQuery query) {
    ResponseModel response = _projectEmpService.DeleteFromProject(query);
    if(response.statusCode != 200) {
      return NotFound(response);
    }
    return Ok(response);
  }
}