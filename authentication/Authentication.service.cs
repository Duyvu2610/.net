using Common.Services;
using hrm_server.entities;
using Common.Models;
using Common.Utilities;
using Common.Config;

namespace AuthenticationModule;

public class AuthenticationService : BaseService
{

  private readonly PasswordHasher _passwordHasher;
  private readonly JWTService _jwtService;
  public AuthenticationService(PasswordHasher passwordHasher, JWTService jwtService, PostgresFactory postgresFactory) : base(postgresFactory)
  {
    _passwordHasher = passwordHasher;
    _jwtService = jwtService;
  }

  public ResponseModel Login(LoginBody body)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    User? user = pgContext.Users.Where(u => u.Email == body.email).FirstOrDefault();
    if (user == null)
    {
      return new ExceptionModel(400, "BAD REQUEST", new List<string> { "email or password is invalid" });
    }

    bool passwordIsValid = _passwordHasher.VerifyPassword(user.HashPassword, body.password);
    if (!passwordIsValid)
    {
      return new ExceptionModel(400, "BAD REQUEST", new List<string> { "email or password is invalid" });
    }

    JWTPayload payload = new JWTPayload(user.Id, user.Email, user.Role);
    string jwt = _jwtService.create(payload);
    string refresh = _jwtService.createRefresh(payload);

    return new LoginResponse(jwt, refresh);
  }

  public ResponseModel Logout(string token)
  {
    JWTPayload payload = _jwtService.verifyToken(token);

    _jwtService.unActiveTokens(payload.id);

    return new ResponseModel(200, "SUCCESSFULLY");
  }

  public ResponseModel refresh(string refreshToken)
  {
    RefreshToken? refresh = _jwtService.getRefresh(refreshToken);
    if (refresh == null)
    {
      return new ExceptionModel(401, "UNAUTHORIZED", new List<string> { });
    }

    JWTPayload payload = _jwtService.verifyToken(refreshToken);
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    User? user = pgContext.Users.FirstOrDefault(p => p.Id == payload.id);
    if (user == null) return new ExceptionModel(401, "UNAUTHORIZED", new List<string> { });
    payload.createdAt = DateTime.Now;
    payload.email = user.Email;
    payload.role = user.Role;
    string accessToken = _jwtService.create(payload);

    return new LoginResponse(accessToken, refreshToken);
  }
  public ResponseModel changePassword(int id, ChangePasswordBody body)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    if (body.OldPassword == body.NewPassword)
    {
      return new ExceptionModel(400, "BAD REQUEST", new List<string> { "New password must be different from the old password" });
    }
    User? currentUser = pgContext.Users.Find(id);
    if (currentUser == null)
    {
      return new ExceptionModel(404, "NOT FOUND", new List<string> { "Not found user" });
    }
    bool passwordIsValid = _passwordHasher.VerifyPassword(currentUser.HashPassword, body.OldPassword);
    if (!passwordIsValid)
    {
      return new ExceptionModel(400, "BAD REQUEST", new List<string> { "old password is invalid" });
    }
    currentUser.HashPassword = _passwordHasher.HashPassword(body.NewPassword);
    pgContext.SaveChanges();
    return new ResponseModel(200, "SUCCESSFULLY");
  }
}

