using System.ComponentModel.DataAnnotations;
using hrm_server.entities;

public class RefreshBody
{
  /// <summary>
  /// Refresh token
  /// </summary>
  /// <example></example>
  public string RefreshToken { get; set; }
}