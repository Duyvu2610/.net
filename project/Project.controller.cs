using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProjectModule;
[Route("api/project")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly ProjectService _projectService;
    public ProjectController(ProjectService projectService)
    {
        _projectService = projectService;
    }
    [HttpGet]
    public IActionResult GetProject([FromQuery] string id)
    {
        var project = _projectService.GetProject(id);
        if (project == null) return NotFound(new ExceptionModel(404, "NOT FOUND", new List<string> { }));
        return Ok(project);
    }
    [HttpPost("github")]
    public IActionResult CreateProject([FromBody] ProjectModel body)
    {

        if (body == null) return BadRequest(new ExceptionModel(400, "BAD REQUEST", new List<string> { }));
        ResponseModel response = _projectService.Create(body);
        if (response.statusCode != 200)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
}