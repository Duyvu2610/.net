namespace Common.Models;
public class ResponseModel
{
  /// <summary>
  /// status code
  /// </summary>
  /// <example>200</example>
  public int statusCode {get; set;}
  /// <summary>
  /// message  
  /// </summary>
  /// <example>SUCCESSFULLY</example>
  public string messages {get; set;}

  public ResponseModel(int statusCode, string messages)
  {
    this.statusCode = statusCode;
    this.messages = messages;
  }
}