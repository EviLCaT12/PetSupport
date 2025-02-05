using FluentValidation.AspNetCore;
using PetFamily.API.Extensions;
using PetFamily.API.Validation;
using PetFamily.Application;
using PetFamily.Infrastructure;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services
    .AddInfrastructure()
    .AddApplication();

var app = builder.Build();

app.UseExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();

    await app.ApplyMigration();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Use(async (context, next) =>
{
    Console.WriteLine(context.Request.Path);

    await next.Invoke();
});
    

app.Run();