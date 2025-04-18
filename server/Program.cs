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
builder.Services.AddTransient<ICalendarioRepository, CalendarioRepository>();
builder.Services.AddTransient<ITrabalhoRepository, TrabalhoRepository>();
builder.Services.AddTransient<IAlunoRepository, AlunoRepository>();
builder.Services.AddTransient<IGrupoRepository, GrupoRepository>();
builder.Services.AddTransient<IIntegranteRepository, IntegranteRepository>();
builder.Services.AddTransient<IEventoRepository, EventoRepository>();
builder.Services.AddTransient<IDiasCalendarioRepository, DiasCalendarioRepository>();

SqlMapper.AddTypeHandler(new TrilhaIdTypeHandler());
SqlMapper.AddTypeHandler(new NomeTrilhaTypeHandler());
SqlMapper.AddTypeHandler(new EdicaoIdTypeHandler());
SqlMapper.AddTypeHandler(new NomeEdicaoTypeHandler());
SqlMapper.AddTypeHandler(new CalendarioIdTypeHandler());

DefaultTypeMap.MatchNamesWithUnderscores = true;

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
