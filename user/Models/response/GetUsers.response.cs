using Common.Models;

public class GetUsersResponse : ResponseModel
{
  public object users { get; set; }
  public GetUsersResponse(object users) : base(200, "SUCCESSFULLY")
  {
    this.users = users;
  }
}