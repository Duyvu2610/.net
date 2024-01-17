using Common.Models;

public class LeaveResponse : ResponseModel
{
    public object leave { get; set; }
    public LeaveResponse(object leave) : base(200, "SUCCESSFULLY")
    {
        this.leave = leave;
    }

}