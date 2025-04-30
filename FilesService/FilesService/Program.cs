using Amazon.S3;
using FilesService.Endpoints;
using FilesService.Infrastructure;
using FilesService.Middlewares;
using FilesService.MongoDataAccess;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .Enrich.WithEnvironmentName()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddSerilog();

builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddEndpoints();

builder.Services.AddSingleton<IMongoClient>(new MongoClient(builder.Configuration.GetConnectionString("MongoConnection")));

builder.Services.AddScoped<FileMongoDbContext>();

builder.Services.AddScoped<IFileRepository, FileRepository>();

builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseExceptionMiddleware();

app.MapEndpoints();

app.UseHttpsRedirection();

app.Run();

