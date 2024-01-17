using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace hrm_server.entities;

[Table("tokens", Schema = "official")]
[PrimaryKey(nameof(Id))]
public partial class Tokens
{
    [Column("id")]
    public int Id { get; set; }

    [Column("token")]
    public string Token { get; set; } = null!;

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
