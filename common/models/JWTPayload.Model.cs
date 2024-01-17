
public class JWTPayload
{
  public int id { get; set; }
  public string email { get; set; }

  public DateTime createdAt { get; set; }

  public string role { get; set; }

  public JWTPayload(int id, string email, string role)
  {
    this.id = id;
    this.email = email;
    this.role = role;
    createdAt = DateTime.Now;
  }
}