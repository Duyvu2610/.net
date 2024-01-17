using Microsoft.OpenApi.Models;
namespace Common.Config;
public class SwaggerConfig
{
  private IServiceCollection _services;
  public SwaggerConfig(IServiceCollection services)
  {
    _services = services;
    _config();
  }

  private void _config()
  {
    _services.AddSwaggerGen(c =>
    {
      c.MapType<DateOnly>(() => new OpenApiSchema{
        Type = "string",
        Format = "date"
      });
      
      c.SwaggerDoc("v1",
        new OpenApiInfo
        {
          Title = "My API - V1",
          Version = "v1"
        }
      );
      var filePath = Path.Combine(System.AppContext.BaseDirectory, "hrm-server.xml");
      c.IncludeXmlComments(filePath);

      // add bearer auth for open api
      c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
      });
      c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
          }
      });
    });
  }
}