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

builder.Services.AddScoped<DbSession>(_ =>
    new DbSession(builder.Configuration["DbConnectionString"]!));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ITrilhaRepository, TrilhaRepository>();
builder.Services.AddTransient<IEdicaoRepository, EdicaoRepository>();

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
