using MiAppCRUD.Server.Data;
using MiAppCRUD.Server.Factories;
using MiAppCRUD.Server.Repositories;
using MiAppCRUD.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString("DefaultConnection")  
    : builder.Configuration.GetValue<string>("DB_CONNECTION_STRING"); 

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)  
);


builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<CategoriaProductoService>();
builder.Services.AddScoped<ReabastecimientoStockService>();
builder.Services.AddScoped<EventoService, EventoServiceImpl>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoFactory, ProductoFactory>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioFactory, UsuarioFactory>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICategoriaProductoRepository, CategoriaProductoRepository>();
builder.Services.AddScoped<ICategoriaProductoService, CategoriaProductoService>();
builder.Services.AddScoped<IReabastecimientoStockRepository, ReabastecimientoStockRepository>();
builder.Services.AddScoped<IReabastecimientoStockFactory, ReabastecimientoStockFactory>();
builder.Services.AddScoped<IReabastecimientoStockService, ReabastecimientoStockService>();



builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<VerificadorReabastecimientoService>();

var app = builder.Build();


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

    
    var staticPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

    
    if (!Directory.Exists(staticPath))
    {
        Directory.CreateDirectory(staticPath);
    }

    
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(staticPath),
        RequestPath = ""
    });

    
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