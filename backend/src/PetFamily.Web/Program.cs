using Microsoft.OpenApi.Models;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Species.Application;
using PetFamily.Species.Infrastructure;
using PetFamily.Species.Presentation;
using PetFamily.Volunteer.Api;
using PetFamily.Volunteer.Infrastructure;
using PetFamily.Volunteers.Application;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") 
                 ?? throw new ArgumentNullException("Seq connection string is null")) 
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentUserName()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "My API", 
        Version = "v1" 
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header, 
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey 
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { 
            new OpenApiSecurityScheme 
            { 
                Reference = new OpenApiReference 
                { 
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" 
                } 
            },
            new string[] { } 
        } 
    });
});
builder.Services.AddOpenApi();
builder.Services.AddSerilog();

builder.Services
    .AddSpeciesApplication()
    .AddSpeciesInfrastructure(builder.Configuration)
    .AddSpeciesPresentation()
    .AddVolunteerApplication()
    .AddVolunteerInfrastructure(builder.Configuration)
    .AddVolunteerPresentation()
    .AddAccountsInfrastructure(builder.Configuration)
    .AddAccountsApplication();


builder.Services.AddAuthorization();

var app = builder.Build();

// await app.ApplyMigration(); 

//app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

namespace PetFamily.Web
{
    public partial class Program;
}
