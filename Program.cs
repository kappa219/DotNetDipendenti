using corsosharp.Models;
using Microsoft.EntityFrameworkCore;
using corsosharp.Data;
using corsosharp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Security.Claims;
using Scalar.AspNetCore;
using MassTransit;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

//logger
builder.Host.UseSerilog((ctx, configuration) =>
    configuration.ReadFrom.Configuration(ctx.Configuration).Enrich.FromLogContext()
    .Enrich.WithProperty("ApplicationName", "CorsoSharpAPI")
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.Seq(ctx.Configuration["Seq:Url"] ?? "http://seq:5341")
    .WriteTo.MySQL(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection")!,
        tableName: "Logs",
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
     )
);    





builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc( "v1", new OpenApiInfo { Title = "CorsoSharp API", Version = "v1" });

    // Configurazione JWT per Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
       // Description = "Inserisci il token JWT. Esempio: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            RoleClaimType = ClaimTypes.Role
        };
    });



    

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthorization();

// CORS - permette richieste dal frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Registra i Service per Dependency Injection
// AddScoped = una istanza per ogni richiesta HTTP (come @Scope("request") in Spring)
// Altre opzioni:
// - AddSingleton = una sola istanza per tutta l'app
// - AddTransient = nuova istanza ogni volta che viene richiesta
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<AnagrafiaService>();
builder.Services.AddScoped<GiornateLavorativeServices>();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DatabaseConnection per ADO.NET (MySqlClient)
builder.Services.AddScoped(_ => new corsosharp.DB.DatabaseConnection(connectionString!));

         // Entity Framework Core con MySQL mi connetto al database usando EF Core e MySQL, con la stringa di connessione dal file di configurazione
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


    builder.Services.AddScoped<IAuthService, AuthService>();

// MassTransit con RabbitMQ per messaggistica asincrona
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

builder.Services.AddScoped<ReportClientService>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}


// Serve i file statici (immagini, ecc.) dalla cartella wwwroot
app.UseStaticFiles();

// Abilita CORS (deve essere prima di MapControllers)
app.UseCors("AllowAll");

// Autenticazione e Autorizzazione (ordine importante!)
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
app.MapScalarApiReference(options =>
{
    options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
});

}


//app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");

app.MapControllers();
app.Run();
