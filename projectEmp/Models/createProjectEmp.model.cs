using System.ComponentModel.DataAnnotations;

public class CreateProjectEmpModel
{
  /// <summary>
  /// projectID
  /// </summary>
  /// <example>0987654</example>
  [Required(ErrorMessage = "Project id is not empty")]
  public string? ProjectId { get; set; }

  /// <summary>
  /// userId
  /// </summary>
  /// <example>1</example>
  [Required(ErrorMessage = "User id is not empty")]
  public int UserId { get; set; }

  /// <summary>
  /// role in project
  /// </summary>
  /// <example>BE DEV</example>
  [Required(ErrorMessage = "Role is not empty")]
  public string? Role { get; set; }
}