using System.ComponentModel.DataAnnotations;

public class RegisterBody
{
  /// <summary>
  /// first name
  /// </summary>
  /// <example>Thu</example>
  [Required(ErrorMessage = "Frist name is not empty")]
  public string? FirstName { get; set; }
  /// <summary>
  /// last name
  /// </summary>
  /// <example>Tran</example>
  [Required(ErrorMessage = "Last name is not empty")]
  public string? LastName { get; set; }
  /// <summary>
  /// email login
  /// </summary>
  /// <example>minhthu@gmail.com</example>
  [Required(ErrorMessage = "Email is not empty")]
  [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "email or password is invalid")]
  public string? Email { get; set; }
  /// <summary>
  /// phone
  /// </summary>
  /// <example>01234567</example>
  [Required(ErrorMessage = "Phone number is not empty")]
  [MaxLength(20, ErrorMessage = "Phone number is invalid")]
  [MinLength(9, ErrorMessage = "Phone number is invalid")]
  [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Phone number is invalid")]
  public string? PhoneNumber { get; set; }
  /// <summary>
  /// birth
  /// </summary>
  /// <example>2003-10-26</example>
  [Required(ErrorMessage = "Birth is not empty")]
  public DateOnly? Birth { get; set; }

  /// <summary>
  /// Gender "M" or "FM"
  /// </summary>
  /// <example>FM</example>
  [Required(ErrorMessage = "Gender is not empty")]
  public string? Gender { get; set; }

  /// <summary>
  /// project id clickup
  /// </summary>
  /// <example>4567880</example>
  [Required(ErrorMessage = "Project is not empty")]
  public string? ProjectId { get; set; }

  /// <summary>
  /// role in project
  /// </summary>
  /// <example>BE DEV</example>
  [Required(ErrorMessage = "Role is not empty")]
  public string? Role { get; set; }
}