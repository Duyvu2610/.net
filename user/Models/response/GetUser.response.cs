using Common.Models;

public class GetUserResponse : ResponseModel
{
  public object user { get; set; }
  public GetUserResponse(object user) : base(200, "SUCCESSFULLY")
  {
    this.user = user;
  }
}