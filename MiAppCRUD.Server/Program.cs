using MiAppCRUD.Server.Data;
using MiAppCRUD.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de la base de datos con Entity Framework
var connectionString = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString("DefaultConnection")  // Para desarrollo
    : builder.Configuration.GetValue<string>("DB_CONNECTION_STRING");  // Para producci�n desde la variable de entorno

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)  // Usar PostgreSQL en lugar de SQL Server
);

// Servicios de la aplicaci�n
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ProductoService>();

// Configuraci�n para las vistas y Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configuraci�n de Swagger para la API (solo en desarrollo)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Si estamos en desarrollo, habilitamos herramientas de depuraci�n y Swagger
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Si estamos en producci�n, manejamos excepciones de manera global y habilitamos HSTS
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirige HTTP a HTTPS
app.UseHttpsRedirection();

// Sirve archivos est�ticos desde wwwroot (donde estar�n los archivos generados por React)
app.UseStaticFiles();

// Configura el enrutamiento
app.UseRouting();

// Configura los endpoints de la API
app.MapControllers();
app.MapRazorPages();

// Si una ruta no se encuentra (por ejemplo, rutas de React), redirige al archivo index.html
app.MapFallbackToFile("/index.html");

app.Run();
