using System.ComponentModel.DataAnnotations;

public class ProjectModel
{
    /// <summary>
    /// project id clickup
    /// </summary>
    /// <example>4567880</example>
    [Required(ErrorMessage = "Project id is not empty")]
    public string? ProjectId { get; set; }
    /// <summary>
    /// github Id
    /// </summary>
    /// <example>123123</example>
    [Required(ErrorMessage = "Github id is not empty")]
    public string? GithubId { get; set; }
}