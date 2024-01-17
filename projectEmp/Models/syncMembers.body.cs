public class DataClickup
{
  public int UserId { get; set; }
  public int ClickupId { get; set; }
}

public class SyncMemberBody
{
  public string? ProjectId { get; set; }

  public List<DataClickup> data { get; set; } = new List<DataClickup>();
}