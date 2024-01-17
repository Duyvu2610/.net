using System.ComponentModel.DataAnnotations;

public class LeaveRequest
{
    /// <summary>
    /// start date
    /// </summary>
    /// <example>2023-12-28</example>
    [Required(ErrorMessage = "Start date is not empty")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// end date
    /// </summary>
    /// <example>2023-12-30</example>
    [Required(ErrorMessage = "End date is not empty")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// description
    /// </summary>
    /// <example>Some description</example>
    [Required(ErrorMessage = "Description is not empty")]
    public string Description { get; set; }
}
