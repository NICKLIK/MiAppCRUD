using MiAppCRUD.Server.Data;
using MiAppCRUD.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos con Entity Framework
var connectionString = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString("DefaultConnection")  // Para desarrollo
    : builder.Configuration.GetValue<string>("DB_CONNECTION_STRING");  // Para producción desde la variable de entorno

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)  // Usar PostgreSQL en lugar de SQL Server
);

// Servicios de la aplicación
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ProductoService>();

// Configuración para las vistas y Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configuración de Swagger para la API (solo en desarrollo)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();

    // Configuración específica para producción
    var staticPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

    // Asegúrate que el directorio wwwroot existe
    if (!Directory.Exists(staticPath))
    {
        Directory.CreateDirectory(staticPath);
    }

    // Servir archivos estáticos del frontend React
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(staticPath),
        RequestPath = ""
    });

    // Manejar rutas del SPA
    app.MapFallbackToFile("index.html", new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(staticPath)
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();
app.MapRazorPages();

app.Run();