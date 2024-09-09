using EFPractica01.Models;
using EFPractica01.Services;
using Microsoft.EntityFrameworkCore;
using MonsterDataSync.Mappers;
using MonsterDataSync.Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de logging con Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// Verificaci�n de la cadena de conexi�n para evitar errores durante la inicializaci�n
string? sqlConnectionString = builder.Configuration.GetConnectionString("SqlConn");

if (string.IsNullOrEmpty(sqlConnectionString))
{
    Log.Fatal("No se encontr� la cadena de conexi�n para la base de datos. Verifica la configuraci�n.");
    throw new Exception("La cadena de conexi�n no est� configurada.");
}

string? apiBaseAddress = builder.Configuration["MonsterHunetApi"];
if (string.IsNullOrEmpty(apiBaseAddress))
{
    Log.Fatal("No se encontr� la URL base para la API externa. Verifica la configuraci�n.");
    throw new Exception("La URL base para la API no est� configurada.");
}

// Configuraci�n de servicios de la aplicaci�n
ConfigureServices(builder.Services, sqlConnectionString, apiBaseAddress);

var app = builder.Build();

// Configuraci�n del pipeline de la aplicaci�n
ConfigurePipeline(app);

app.Run();


// Funci�n para configurar los servicios
void ConfigureServices(IServiceCollection services, string sqlConnectionString, string apiBaseAddress)
{
    // Add services to the container.
    services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

    // Configurar Swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // Inyecci�n de dependencias de Repository Skills
    services.AddScoped<ICrudBase, SkillsRepositorys>();

    // Configuraci�n de AutoMapper con perfiles de mapeo
    services.AddAutoMapper(typeof(MappingProfile));

    // Configuraci�n de DbContext con SQL Server y logging
    services.AddDbContext<SkillsContext>(options =>
    {
        options.UseSqlServer(sqlConnectionString)
               .LogTo(Console.WriteLine, LogLevel.Warning); // Ajusta el nivel de logging aqu�
    });

    // Configuraci�n del cliente HTTP para la API externa
    services.AddHttpClient<ServicesBackground>(c =>
    {
        c.BaseAddress = new Uri(apiBaseAddress);
    });

    // Servicio de background para sincronizaci�n
    services.AddHostedService<ServicesBackground>();
}


// Funci�n para configurar el pipeline de la aplicaci�n
void ConfigurePipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
}