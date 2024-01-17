using Common.Models;

public class MembersResponse : ResponseModel
{
  public object members { get; set; }
  public MembersResponse(object members) : base(200, "SUCCESSFULLY")
  {
    this.members = members;
  }

}