using AuthenticationModule;
using Common.Utilities;
using LeaveModule;
using ProjectModule;
using UserModule;
namespace Common.Config;

public class ServiceConfig
{
  private IServiceCollection _services;
  public ServiceConfig(IServiceCollection services)
  {
    _services = services;
    configDI();
    configController();
  }

  public void configDI()
  {
    _services.AddSingleton<AuthenticationService>();
    _services.AddSingleton<UserService>();
    _services.AddSingleton<PasswordHasher>();
    _services.AddSingleton<JWTService>();
    _services.AddSingleton<ProjectEmpService>();
    _services.AddSingleton<ProjectService>();
    _services.AddSingleton<LeaveService>();
    _services.AddSingleton<PostgresFactory>();
    _services.AddSingleton(
      sp => sp.GetRequiredService<PostgresFactory>().CreateDbContext()
    );

  }

  public void configController()
  {
    _services.AddControllers();
  }
}