using EFPractica01.Models;
using EFPractica01.Services;
using Microsoft.EntityFrameworkCore;
using MonsterDataSync.Mappers;
using MonsterDataSync.Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configuración de logging con Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// Verificación de la cadena de conexión para evitar errores durante la inicialización
string? sqlConnectionString = builder.Configuration.GetConnectionString("SqlConn");

if (string.IsNullOrEmpty(sqlConnectionString))
{
    Log.Fatal("No se encontró la cadena de conexión para la base de datos. Verifica la configuración.");
    throw new Exception("La cadena de conexión no está configurada.");
}

string? apiBaseAddress = builder.Configuration["MonsterHunetApi"];
if (string.IsNullOrEmpty(apiBaseAddress))
{
    Log.Fatal("No se encontró la URL base para la API externa. Verifica la configuración.");
    throw new Exception("La URL base para la API no está configurada.");
}

// Configuración de servicios de la aplicación
ConfigureServices(builder.Services, sqlConnectionString, apiBaseAddress);

var app = builder.Build();

// Configuración del pipeline de la aplicación
ConfigurePipeline(app);

app.Run();


// Función para configurar los servicios
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

    // Inyección de dependencias de Repository Skills
    services.AddScoped<ICrudBase, SkillsRepositorys>();

    // Configuración de AutoMapper con perfiles de mapeo
    services.AddAutoMapper(typeof(MappingProfile));

    // Configuración de DbContext con SQL Server y logging
    services.AddDbContext<SkillsContext>(options =>
    {
        options.UseSqlServer(sqlConnectionString)
               .LogTo(Console.WriteLine, LogLevel.Warning); // Ajusta el nivel de logging aquí
    });

    // Configuración del cliente HTTP para la API externa
    services.AddHttpClient<ServicesBackground>(c =>
    {
        c.BaseAddress = new Uri(apiBaseAddress);
    });

    // Servicio de background para sincronización
    services.AddHostedService<ServicesBackground>();
}


// Función para configurar el pipeline de la aplicación
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