using Microsoft.OpenApi.Models;
using Test.Web.Api.Data;
using Test.Web.Api.Domain;
using Test.Web.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = builder.Environment.ApplicationName,
        Version = "1.0.0"
    });
    c.EnableAnnotations();
    c.CustomSchemaIds(x => x.FullName);
});

builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("TestDatabase")!;
builder.Services.AddSingleton(new MicrosoftSqlConnectionString(connectionString));

builder.Services.AddTransient<IObjectRepository, ObjectRepository>();
builder.Services.AddTransient<IObjectManager, ObjectManager>();

builder.Services.AddMvc();

var app = builder.Build();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();