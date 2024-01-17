using Common.Models;
public class LoginResponse : ResponseModel
{
  /// <summary>
  /// Access Token
  /// </summary>
  /// <example>dfghjERTYUI678</example>
  public string accessToken { get; set; }
  public string refreshToken { get; set; }

  public LoginResponse(
    string accessToken,
    string refreshToken
  ) : base(200, "SUCCESSFULLY")
  {
    this.accessToken = accessToken;
    this.refreshToken = refreshToken;
  }
}