using System.ComponentModel.DataAnnotations;

public class UpdateGithubBody
{
  /// <summary>
  /// Github username
  /// </summary>
  /// <example>quagntho908</example>
  [Required(ErrorMessage = "Github is not empty")]
  public string? github { get; set; }
}