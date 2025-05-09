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

            // Configuraciones específicas para el modelo Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(u => u.Nombre)
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                entity.Property(u => u.Apellido)
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                entity.Property(u => u.Edad)
                    .IsRequired();

                entity.Property(u => u.Genero)
                    .HasColumnType("varchar(20)")
                    .IsRequired();

                entity.Property(u => u.Correo)
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                entity.Property(u => u.Provincia)
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                entity.Property(u => u.Ciudad)
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                entity.Property(u => u.Contrasena)
                    .HasColumnType("text")
                    .IsRequired();

                // Índice único para el correo electrónico
                entity.HasIndex(u => u.Correo)
                    .IsUnique();
            });

            // Configuraciones específicas para el modelo Producto
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(p => p.Nombre)
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                entity.Property(p => p.Descripcion)
                    .HasColumnType("text");

                entity.Property(p => p.Precio)
                    .HasColumnType("numeric(18,2)")
                    .IsRequired();

                entity.Property(p => p.Stock)
                    .IsRequired();
            });
        }
    }
}