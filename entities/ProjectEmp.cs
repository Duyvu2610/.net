using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace hrm_server.entities;

[Table("projects_emps", Schema = "official")]
[PrimaryKey(nameof(ProjectId), nameof(UserId))]
public partial class ProjectEmp
{
    [Column("project_id")]
    public string? ProjectId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("clickup_id")]
    public int? ClickupId { get; set; }

    [Column("github_id")]
    public string? GithubId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("ended_at")]
    public DateTime? EndedAt { get; set; }

    [Column("role")]
    public string? Role { get; set; }
    public virtual User User { get; set; } = null!;
}
