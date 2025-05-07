using Microsoft.EntityFrameworkCore;
using MiAppCRUD.Server.Models;
using System.Linq;

namespace MiAppCRUD.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración específica para PostgreSQL
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Configurar nombres de tablas en minúsculas para PostgreSQL
                entity.SetTableName(entity.GetTableName().ToLower());

                // Configurar tipos para strings
                foreach (var property in entity.GetProperties()
                    .Where(p => p.ClrType == typeof(string)))
                {
                    // Si no tiene configuración específica, usar text por defecto
                    if (string.IsNullOrEmpty(property.GetColumnType()))
                    {
                        property.SetColumnType("text");
                    }
                }
            }

            // Configuraciones específicas para cada modelo
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(u => u.NombreUsuario)
                    .HasColumnType("varchar(50)");

                entity.Property(u => u.Contrasena)
                    .HasColumnType("text");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(p => p.Nombre)
                    .HasColumnType("varchar(100)");

                entity.Property(p => p.Descripcion)
                    .HasColumnType("text");

                entity.Property(p => p.Precio)
                    .HasColumnType("numeric(18,2)");
            });
        }
    }
}