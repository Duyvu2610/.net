using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace hrm_server.entities;

[Table("projects", Schema = "official")]
[PrimaryKey(nameof(ProjectId), nameof(GithubId))]
public partial class Projects
{
    [Column("project_id")]
    public string? ProjectId { get; set; }

    [Column("github_id")]
    public string? GithubId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }
}
