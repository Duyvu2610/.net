using System.ComponentModel.DataAnnotations;

public class MembersParams
{
  public string? projectId { get; set; }
  public string? role { get; set; }

  public string? keyword { get; set; }
}