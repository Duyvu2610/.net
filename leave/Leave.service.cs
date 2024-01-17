using System.Globalization;
using Common.Config;
using Common.Models;
using Common.Services;
using hrm_server.entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LeaveModule;
public class LeaveService : BaseService
{
  public LeaveService(PostgresFactory postgresFactory) : base(postgresFactory)
  {
  }

  public ResponseModel leaves(int userId, string? type, string date) {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    User? user = pgContext.Users.Find(userId);
    if(user == null) {
      return new ExceptionModel(401, "UNAUTHORIZED", new List<string>());
    }
    var leavesList = pgContext.Leaves
    .Where(l => l.UserId == userId)
    .Where(l => type == "accepted" ? l.AcceptedBy != null : true)
    .Where(l => type == "canceled" ? l.CanceledBy != null : true)
    .Where(l => type == null ? l.AcceptedBy == null && l.CanceledBy == null: true)
    .Where(l => date != null ? l.CreatedAt.Date == DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture)  : true)
    .SelectMany(
      l => pgContext.Users
      .Where(u => u.Id == l.AcceptedBy)
      .DefaultIfEmpty(),
      (l, u) => new {
        id = l.LeaveId,
        userId = l.UserId,
        user = $"{user.FirstName} {user.LastName}",
        createdAt = l.CreatedAt,
        startDate = l.StartDate,
        endDate = l.EndDate,
        acceptedAt = l.AcceptedAt,
        canceledAt = l.CanceledAt,
        acceptedBy = u != null ? $"{u.FirstName} {u.LastName}" : null,
        canceledBy = l.CanceledBy,
        description = l.Description
      }
    ).SelectMany(
      l => pgContext.Users
      .Where(u => u.Id == l.canceledBy)
      .DefaultIfEmpty(),
      (l, u) => new {
        l.id,
        l.userId,
        l.user,
        l.createdAt,
        l.startDate,
        l.endDate,
        l.acceptedAt,
        l.canceledAt,
        l.acceptedBy,
        canceledBy = u != null ? $"{u.FirstName} {u.LastName}" : null,
        l.description
      }
    ).ToList();

    return new LeaveResponse(leavesList);
  }

  public ResponseModel leavesAll(string? type, int month, int year) {
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    var leavesList = pgContext.Leaves
    .Where(l => type == "accepted" ? l.AcceptedBy != null : true)
    .Where(l => type == "canceled" ? l.CanceledBy != null : true)
    .Where(l => type == null ? l.AcceptedBy == null && l.CanceledBy == null: true)
    .Where(l => l.CreatedAt.Date.Month == month && l.CreatedAt.Date.Year == year)
    .SelectMany(
      l => pgContext.Users
      .Where(u => u.Id == l.AcceptedBy)
      .DefaultIfEmpty(),
      (l, u) => new {
        id = l.LeaveId,
        userId = l.UserId,
        user = l.UserId,
        createdAt = l.CreatedAt,
        startDate = l.StartDate,
        endDate = l.EndDate,
        acceptedAt = l.AcceptedAt,
        canceledAt = l.CanceledAt,
        acceptedBy = u != null ? $"{u.FirstName} {u.LastName}" : null,
        canceledBy = l.CanceledBy,
        description = l.Description
      }
    ).SelectMany(
      l => pgContext.Users
      .Where(u => u.Id == l.canceledBy)
      .DefaultIfEmpty(),
      (l, u) => new {
        l.id,
        l.userId,
        l.user,
        l.createdAt,
        l.startDate,
        l.endDate,
        l.acceptedAt,
        l.canceledAt,
        l.acceptedBy,
        canceledBy = u != null ? $"{u.FirstName} {u.LastName}" : null,
        l.description
      }
    ).SelectMany(
      l => pgContext.Users
      .Where(u => u.Id == l.user)
      .DefaultIfEmpty(),
      (l, u) => new {
        l.id,
        l.userId,
        user = u != null ? $"{u.FirstName + u.LastName}": null,
        l.createdAt,
        l.startDate,
        l.endDate,
        l.acceptedAt,
        l.canceledAt,
        l.acceptedBy,
        l.canceledBy,
        l.description
      }
    ).ToList();

    return new LeaveResponse(leavesList);
  }

  public ResponseModel LeaveRequest(int userId, LeaveRequest leaveRequest)
  {
    if (leaveRequest.StartDate > leaveRequest.EndDate) return new ExceptionModel(400, "BAD REQUEST", new List<string> { "The start date must be before the end date" });
    PostgresConfig pgContext = pgFactory.CreateDbContext();
    Leaves leaves = new Leaves
    {
      UserId = userId,
      StartDate = leaveRequest.StartDate,
      EndDate = leaveRequest.EndDate,
      Description = leaveRequest.Description,
      CreatedAt = DateTime.Now
    };
    pgContext.Add(leaves);
    pgContext.SaveChanges();
    return new LeaveResponse(leaves);
  }

  public ResponseModel RejectRequest(int leaveId, int rejectedBy)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();

    Leaves leave = pgContext.Leaves.Find(leaveId);

    if (leave == null)
    {
      return new ExceptionModel(404, "NOT FOUND", new List<string> { "Leave request not found" });
    }
    if (leave.AcceptedBy != null) return new ExceptionModel(400, "BAD REQUEST", new List<string> { "leave application has been approved in advance" });
    leave.CanceledBy = rejectedBy;
    leave.CanceledAt = DateTime.Now;
    pgContext.SaveChanges();
    return new ResponseModel(200, "SUCESSFULLY");

  }

  public ResponseModel ApproveRequest(int leaveId, int acceptedBy)
  {
    PostgresConfig pgContext = pgFactory.CreateDbContext();

    Leaves leave = pgContext.Leaves.Find(leaveId);

    if (leave == null)
    {
      return new ExceptionModel(404, "NOT FOUND", new List<string> { "Leave request not found" });
    }
    if (leave.CanceledBy != null) return new ExceptionModel(400, "BAD REQUEST", new List<string> { "leave application has been rejected in advance" });
    leave.AcceptedBy = acceptedBy;
    leave.AcceptedAt = DateTime.Now;
    pgContext.SaveChanges();

    return new ResponseModel(200, "SUCESSFULLY");

  }
}