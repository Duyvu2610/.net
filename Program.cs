using Common.Config;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Common.Models;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// ENV
DotenvConfig.Load();

// CONTROLLER
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// SWAGGER
new SwaggerConfig(builder.Services);

// SERVICE
new ServiceConfig(builder.Services);

builder.Services.AddCors();

builder.Services.AddAuthentication(options =>
{
  options.DefaultChallengeScheme = "FORBIDDEN";

  // you can also skip this to make the challenge scheme handle the forbid as well
  options.DefaultForbidScheme = "FORBIDDEN";

  // of course you also need to register that scheme, e.g. using
  options.AddScheme<MySchemeHandler>("FORBIDDEN", "scheme display name");
});

string host = Environment.GetEnvironmentVariable("DB_HOST") ?? "";
string dbname = Environment.GetEnvironmentVariable("DB_NAME") ?? "";
string dbuser = Environment.GetEnvironmentVariable("DB_USER") ?? "";
string dbpassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";
builder.Services.AddPooledDbContextFactory<PostgresConfig>(options =>
    options.UseNpgsql($"Host={host};Database={dbname};Username={dbuser};Password={dbpassword}"));
builder.Services.AddControllers(
  options =>
  {
    options.Filters.Add<HttpResponseExceptionFilter>();
  }
).AddJsonOptions(
  options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddMvcCore().ConfigureApiBehaviorOptions(options =>
{
  options.InvalidModelStateResponseFactory = (errorContext) =>
  {

    var errors = errorContext.ModelState.Values.SelectMany(e =>
      e.Errors.Select(m => m.ErrorMessage)
    ).ToList();

    var result = new ExceptionModel(
      (int)HttpStatusCode.BadRequest,
      "BAD REQUEST",
      errors
    );
    return new BadRequestObjectResult(result);
  };
});

var app = builder.Build();

app.UseCors(
  options => options.WithOrigins("http://localhost:3000")
  .AllowAnyMethod()
  .AllowAnyHeader()
  .AllowAnyOrigin()
);

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    options.DefaultModelsExpandDepth(-1);
  });
};

app.MapControllers();

app.Run();
// test
