

using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using MinimalApiTest.Endpoints;
using MinimalApiTest.Model;
using MinimalApiTest.Service;

var builder = WebApplication.CreateBuilder(args);
const string ConnectionString = "Data Source=DESKTOP-KKSH6NU\\SQLSERVER2022;Database=Test;User=sa;Password=********;Encrypt=True;Trusted_Connection=True;Trust Server Certificate=True";
// Add services to the container.

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton(serviceProvider =>
{
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    return new SqlConnectionFactory(ConnectionString);
});

builder.Services
    .AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddSqlServer()
        .WithGlobalConnectionString(ConnectionString)
        .ScanIn(typeof(Program).Assembly).For.Migrations());

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapCustomerEndpoints();

using (var scope = app.Services.CreateScope())
{
    var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    try
    {
        migrationRunner.MigrateUp();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while running migrations.");
    }
}

app.Run();
