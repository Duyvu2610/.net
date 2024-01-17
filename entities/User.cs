using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace hrm_server.entities;

[Table("users", Schema = "official")]
[PrimaryKey(nameof(Id))]
public class User
{
    [Column("id")]
    public int Id { get; set; }

    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("last_name")]
    public string? LastName { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    [Column("hash_password")]
    public string? HashPassword { get; set; }

    [Column("birth")]
    public DateOnly? Birth { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("role")]
    public string Role { get; set; } = null!;

    [Column("gender")]
    public string? Gender { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("github")]
    public string? Github { get; set; }
    public virtual ICollection<ProjectEmp> ProjectsEmps { get; set; } = new List<ProjectEmp>();
    public virtual ICollection<Tokens> Tokens { get; set; } = new List<Tokens>();
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public virtual ICollection<Leaves> Leaves { get; set; } = new List<Leaves>();
}
