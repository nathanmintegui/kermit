using Dapper;

using Kermit.Database;
using Kermit.Repositories;
using Kermit.Repositories.CustomHandlers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new NpgsqlDbConnectionFactory(builder.Configuration["DbConnectionString"]!));

builder.Services.AddScoped<ITrilhaRepository, TrilhaRepository>();
builder.Services.AddScoped<IEdicaoRepository, EdicaoRepository>();

SqlMapper.AddTypeHandler(new TrilhaIdTypeHandler());
SqlMapper.AddTypeHandler(new NomeTrilhaTypeHandler());
SqlMapper.AddTypeHandler(new EdicaoIdTypeHandler());
SqlMapper.AddTypeHandler(new NomeEdicaoTypeHandler());

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();
