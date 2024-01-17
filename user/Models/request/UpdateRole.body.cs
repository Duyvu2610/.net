using System.ComponentModel.DataAnnotations;

public class UpdateRoleBody
{
  /// <summary>
  /// user id
  /// </summary>
  /// <example>1</example>
  [Required(ErrorMessage = "user id is not exits")]
  public int UserId { get; set; }

  /// <summary>
  /// role
  /// </summary>
  /// <example>BE DEV</example>
  [Required(ErrorMessage = "role is not empty")]
  public string? Role { get; set; }
}