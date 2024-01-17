

namespace Common.Models;
public class ExceptionModel : ResponseModel
{
    public List<string> errors {get; set;}
    public ExceptionModel(int statusCode, string messages, List<string> errors) : base(statusCode, messages)
    {
        this.errors = errors;
    }
} 