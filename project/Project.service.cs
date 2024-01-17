using System.Security.Cryptography.X509Certificates;
using Common.Config;
using Common.Models;
using Common.Services;
using hrm_server.entities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ProjectModule;
public class ProjectService : BaseService
{
    public ProjectService(PostgresFactory postgresFactory) : base(postgresFactory)
    {
    }

    private void unActive(string projectId, string githubId)
    {
        PostgresConfig pgContext = pgFactory.CreateDbContext();
        List<Projects> projects = pgContext.Projects.Where(p => p.ProjectId == projectId && p.GithubId != githubId).ToList();

        projects.ForEach(p => p.IsActive = false);

        pgContext.UpdateRange(projects);
        pgContext.SaveChanges();
    }
    public ResponseModel Create(ProjectModel model)
    {
        PostgresConfig pgContext = pgFactory.CreateDbContext();

        Projects? projectExist = pgContext.Projects.FirstOrDefault(p => p.ProjectId == model.ProjectId && p.GithubId == model.GithubId);

        if (projectExist != null)
        {
            projectExist.IsActive = true;
            pgContext.Update(projectExist);
        }
        else
        {
            Projects projects = new Projects
            {
                ProjectId = model.ProjectId,
                GithubId = model.GithubId,
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            pgContext.Projects.Add(projects);
        }
        pgContext.SaveChanges();
        unActive(model.ProjectId, model.GithubId);
        return new ResponseModel(200, "SUCESSFULLY");
    }
    public ResponseModel GetProject(string projectId)
    {
        PostgresConfig pgContext = pgFactory.CreateDbContext();
        Projects? project = pgContext.Projects.FirstOrDefault(u => u.ProjectId == projectId && u.IsActive == true);
        if (project == null) return new ProjectResponse(new ProjectModel { ProjectId = "", GithubId = "" });
        ProjectModel projectModel = new ProjectModel
        {
            ProjectId = project.ProjectId,
            GithubId = project.GithubId
        };
        return new ProjectResponse(projectModel);
    }
}