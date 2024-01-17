using Common.Models;

public class ProjectResponse : ResponseModel
{
    public object project { get; set; }
    public ProjectResponse(object project) : base(200, "SUCCESSFULLY")
    {
        this.project = project;
    }

}