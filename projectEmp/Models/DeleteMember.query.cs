using System.ComponentModel.DataAnnotations;

public class DeleteMemberQuery
{
  [Required(ErrorMessage = "user is not exist")]
  public int UserId { get; set; }

  [Required(ErrorMessage = "project is not exist")]
  public string ProjectId { get; set; }
}