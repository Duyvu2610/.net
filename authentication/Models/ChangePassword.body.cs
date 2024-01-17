
using System.ComponentModel.DataAnnotations;

public class ChangePasswordBody
{
  /// <summary>
  /// Old password
  /// </summary>
  /// <example>oldpass</example>
  [Required(ErrorMessage = "old password is required")]
  public string? OldPassword { get; set; }
  /// <summary>
  /// New password
  /// </summary>
  /// <example>newpass</example>
  [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "email or password is invalid")]
  [Required(ErrorMessage = "new password is required")]
  public string? NewPassword { get; set; }
}