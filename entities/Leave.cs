using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace hrm_server.entities;

[Table("leaves", Schema = "official")]
[PrimaryKey(nameof(LeaveId))]
public partial class Leaves
{
    [Column("leave_id")]
    public int? LeaveId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [Column("accepted_by")]
    public int? AcceptedBy { get; set; }

    [Column("canceled_by")]
    public int? CanceledBy { get; set; }

    [Column("accepted_at")]
    public DateTime? AcceptedAt { get; set; }

    [Column("canceled_at")]
    public DateTime? CanceledAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }
    public virtual User User { get; set; } = null!;

}
