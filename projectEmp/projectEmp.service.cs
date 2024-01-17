using Common.Config;
using Common.Models;
using Common.Services;
using hrm_server.entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
public class ProjectEmpService : BaseService
{
  public ProjectEmpService(PostgresFactory postgresFactory) : base(postgresFactory)
  {
  }

  public ResponseModel members(MembersParams param)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    var members = pgContext.ProjectEmps
      .Join(
        pgContext.Users,
        pe => pe.UserId,
        u => u.Id,
        (pe, u) => new
        {
          projectId = pe.ProjectId,
          id = u.Id,
          firstName = u.FirstName,
          lastName = u.LastName,
          role = pe.Role,
          joinedDate = pe.CreatedAt,
          email = u.Email,
          clickup = pe.ClickupId,
          endedAt = pe.EndedAt,
          github = u.Github
        }
      ).Where(m => (
        m.projectId == param.projectId) && m.endedAt == null &&
        ((param.role == null) || (m.role == param.role)) &&
        ((param.keyword == null) || (
          EF.Functions.ILike(m.firstName, $"%{param.keyword}%") ||
          EF.Functions.ILike(m.lastName, $"%{param.keyword}%")
        ))
      );
    members.ToList();
    return new MembersResponse(members);
  }
  public ResponseModel create(CreateProjectEmpModel model)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    ProjectEmp? exist = pgContext.ProjectEmps
      .Where(pe => pe.UserId == model.UserId && pe.ProjectId == model.ProjectId)
      .FirstOrDefault();
    if (exist != null)
    {
      exist.EndedAt = null;
      exist.Role = model.Role;
      pgContext.ProjectEmps.Update(exist);
      pgContext.SaveChanges();
    }
    else
    {
      ProjectEmp pe = new ProjectEmp
      {
        ProjectId = model.ProjectId,
        UserId = model.UserId,
        Role = model.Role,
        CreatedAt = DateTime.Now
      };

      pgContext.ProjectEmps.Add(pe);
      pgContext.SaveChanges();
    }
    return new ResponseModel(200, "SUCESSFULLY");
  }

  public ResponseModel syncClickup(SyncMemberBody body)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    List<int> userIds = new List<int>();
    Dictionary<int, int> clickups = new Dictionary<int, int>();

    body.data?.ForEach(d =>
    {
      userIds.Add(d.UserId);
      clickups.Add(d.UserId, d.ClickupId);
    });

    List<ProjectEmp> projectEmps = pgContext.ProjectEmps
      .Where(pe => pe.ProjectId == body.ProjectId)
      .Where(pe => userIds.Contains(pe.UserId))
      .ToList();
    if (projectEmps.Count() <= 0)
    {
      return new ResponseModel(200, "SUCCESSFULLY");
    }
    projectEmps.ForEach(pe =>
    {
      pe.ClickupId = clickups[pe.UserId];
    });

    pgContext.ProjectEmps.UpdateRange(projectEmps);
    pgContext.SaveChanges();
    return new ResponseModel(200, "SUCCESSFULLY");
  }

  public ResponseModel MemberFree()
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();

    var users = pgContext.Users
      .SelectMany(
        u => pgContext.ProjectEmps
        .Where(pe => pe.UserId == u.Id)
        .DefaultIfEmpty(),
        (u, pe) => new
        {
          Email = u.Email,
          Id = u.Id,
          ProjectId = pe.ProjectId,
          EndedAt = pe.EndedAt
        }
      )
      .Where(pe => pe.ProjectId == null || pe.EndedAt != null)
      .Select(pe => new { Email = pe.Email, Id = pe.Id })
      .ToList();

    return new MembersResponse(users);
  }

  public ResponseModel DeleteFromProject(DeleteMemberQuery query) {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    User? exist = pgContext.Users.FirstOrDefault(u => u.Id == query.UserId);
    if(exist == null) {
      return new ExceptionModel(404, "NOT FOUND", new List<string> {"user is not exist"});
    }

    ProjectEmp? member = pgContext.ProjectEmps.FirstOrDefault(pe => pe.ProjectId == query.ProjectId && pe.UserId == query.UserId);
    if(member == null) {
      return new ExceptionModel(404, "NOT FOUND", new List<string> {"member is not exist"});
    }

    member.EndedAt = DateTime.Now;
    pgContext.ProjectEmps.Update(member);
    pgContext.SaveChanges();
    return new ResponseModel(200, "SUCCESSFULLY");
  }
}