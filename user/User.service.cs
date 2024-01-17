using Common.Config;
using Common.Models;
using Common.Services;
using Common.Utilities;
using hrm_server.entities;
using Microsoft.EntityFrameworkCore;

namespace UserModule;
public class UserService : BaseService
{
  private readonly PasswordHasher _passwordHasher;
  private readonly ProjectEmpService _peService;
  public UserService(PasswordHasher passwordHasher, ProjectEmpService peService, PostgresFactory postgresFactory) : base(postgresFactory)
  {
    _passwordHasher = passwordHasher;
    _peService = peService;
  }

  public ResponseModel GetUsers(GetUsersQuery query)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    List<User> users = pgContext.Users
    .Where(user => (query.IsActive == "false") ? user.IsActive == false : user.IsActive == true)
    .Where(user => (query.Role != null) ? user.Role == query.Role : true)
    .Where(user => query.keyword == null || (
      EF.Functions.ILike(user.FirstName, $"%{query.keyword}%") ||
      EF.Functions.ILike(user.LastName, $"%{query.keyword}%")
    ))
    .Select(user => new User
    {
      Id = user.Id,
      Email = user.Email,
      FirstName = user.FirstName,
      LastName = user.LastName,
      Birth = user.Birth,
      Gender = user.Gender,
      Role = user.Role,
      Github = user.Github
    })
    .ToList();

    return new GetUsersResponse(users);
  }
  public ResponseModel GetUser(int id)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    User? user = pgContext.Users
    .Select(user => new User
    {
      Id = user.Id,
      Email = user.Email,
      FirstName = user.FirstName,
      LastName = user.LastName,
      Birth = user.Birth,
      Gender = user.Gender,
      PhoneNumber = user.PhoneNumber,
      Github = user.Github
    })
    .FirstOrDefault(u => u.Id == id);
    if (user == null) return new ExceptionModel(404, "NOT FOUND", new List<string> { });
    return new GetUserResponse(user);
  }

  public ResponseModel CreateUser(RegisterBody body)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    User? userWithEmail = pgContext.Users.FirstOrDefault(u => u.Email == body.Email);
    if (userWithEmail != null)
    {
      return new ExceptionModel(400, "BAD REQUEST", new List<string> { "Your Email Already Exist" });
    }
    User user = new User
    {
      FirstName = body.FirstName,
      LastName = body.LastName,
      Email = body.Email,
      PhoneNumber = body.PhoneNumber,
      Birth = body.Birth,
      HashPassword = _passwordHasher.HashPassword(_passwordHasher.RandomPassword(8)),
      CreatedAt = DateTime.Now,
      Role = nameof(RoleModel.EMP).Trim(),
      Gender = body.Gender,
      IsActive = true
    };
    pgContext.Add(user);
    pgContext.SaveChanges();
    _peService.create(new CreateProjectEmpModel
    {
      ProjectId = body.ProjectId,
      UserId = user.Id,
      Role = body.Role
    });
    return new ResponseModel(200, "SUCCESSFULLY");
  }
  public ResponseModel Update(int id, UpdateBody user)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    User? currentUser = pgContext.Users.Find(id);
    if (currentUser == null)
    {
      return new ExceptionModel(401, "UNAUTHORIZED", new List<string>());
    }
    currentUser.PhoneNumber = user.PhoneNumber;
    currentUser.Birth = user.Birth;
    currentUser.FirstName = user.FirstName;
    currentUser.LastName = user.LastName;
    pgContext.SaveChanges();

    return new ResponseModel(200, "SUCCESSFULLY");
  }
  public User? Delete(int id)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    var user = pgContext.Users.Find(id);
    if (user != null)
    {
      pgContext.Remove(user);
      pgContext.SaveChanges();
    }
    return user;
  }

  public ResponseModel UpdateGithub(int userId, string github)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    User? user = pgContext.Users.FirstOrDefault(u => u.Id == userId);
    if (user == null)
    {
      return new ExceptionModel(401, "UNAUTHORIZED", new List<string>());
    }
    user.Github = github;
    pgContext.Update(user);
    pgContext.SaveChanges();
    return new ResponseModel(200, "SUCCESSFULLY");
  }

  public ResponseModel UpdateRole(UpdateRoleBody body)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    User? exist = pgContext.Users.FirstOrDefault(u => u.Id == body.UserId);
    if (exist == null)
    {
      return new ExceptionModel(404, "NOT FOUND", new List<string> { "user is not exist" });
    }

    exist.Role = body.Role;
    exist.UpdatedAt = DateTime.Now;
    pgContext.Users.Update(exist);
    pgContext.SaveChanges();
    return new ResponseModel(200, "SUCCESSFULLY");
  }
}
