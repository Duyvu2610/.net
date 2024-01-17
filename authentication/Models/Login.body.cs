using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

public class LoginBody
{
  /// <summary>
  /// email login
  /// </summary>
  /// <example>quangtho23062002@gmail.com</example>
  [Required(ErrorMessage = "email or password is invalid")]
  [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "email or password is invalid")]
  public string? email { get; set; }

  /// <summary>
  /// password login
  /// </summary>
  /// <example>fT4mty1f</example>
  [Required(ErrorMessage = "email or password is invalid")]
  [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "email or password is invalid")]
  public string? password { get; set; }
}