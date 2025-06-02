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
        public DbSet<ClaveAdmin> ClavesAdmin { get; set; }
        public DbSet<CategoriaProducto> CategoriasProducto { get; set; }
        public DbSet<ReabastecimientoStock> ReabastecimientosStock { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración general para PostgreSQL (nombres de tabla y strings)
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToLower());

                foreach (var property in entity.GetProperties()
                    .Where(p => p.ClrType == typeof(string)))
                {
                    if (string.IsNullOrEmpty(property.GetColumnType()))
                    {
                        property.SetColumnType("text");
                    }
                }
            }

            // Configuración para Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(u => u.Nombre).HasColumnType("varchar(50)").IsRequired();
                entity.Property(u => u.Apellido).HasColumnType("varchar(50)").IsRequired();
                entity.Property(u => u.Edad).IsRequired();
                entity.Property(u => u.Genero).HasColumnType("varchar(20)").IsRequired();
                entity.Property(u => u.Correo).HasColumnType("varchar(100)").IsRequired();
                entity.Property(u => u.Provincia).HasColumnType("varchar(50)").IsRequired();
                entity.Property(u => u.Ciudad).HasColumnType("varchar(50)").IsRequired();
                entity.Property(u => u.Contrasena).HasColumnType("text").IsRequired();

                entity.HasIndex(u => u.Correo).IsUnique();
            });

            // Configuración para Producto
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(p => p.Nombre).HasColumnType("varchar(100)").IsRequired();
                entity.Property(p => p.Descripcion).HasColumnType("text");
                entity.Property(p => p.Precio).HasColumnType("numeric(18,2)").IsRequired();
                entity.Property(p => p.Stock).IsRequired();
                entity.Property(p => p.ImagenUrl).HasColumnType("varchar(200)");
                entity.Property(p => p.EcuniPoints).HasColumnType("numeric(18,2)").IsRequired();

                entity.HasOne(p => p.Categoria)
                    .WithMany(c => c.Productos)
                    .HasForeignKey(p => p.CategoriaProductoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para ClaveAdmin
            modelBuilder.Entity<ClaveAdmin>(entity =>
            {
                entity.Property(c => c.Correo).HasColumnType("varchar(100)").IsRequired();
                entity.Property(c => c.Clave).HasColumnType("varchar(100)").IsRequired();
                entity.Property(c => c.Usada).IsRequired();

                entity.HasIndex(c => c.Clave).IsUnique();
            });

            // Configuración para CategoriaProducto
            modelBuilder.Entity<CategoriaProducto>(entity =>
            {
                entity.Property(c => c.Nombre).HasColumnType("varchar(100)").IsRequired();
            });

            // ✅ Configuración para ReabastecimientoStock
            modelBuilder.Entity<ReabastecimientoStock>(entity =>
            {
                entity.Property(r => r.Estado).HasColumnType("varchar(50)").IsRequired();
                entity.Property(r => r.Cantidad).IsRequired();

                // ✅ Ambos con timestamp with time zone
                entity.Property(r => r.FechaSolicitud)
                    .HasColumnType("timestamptz")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(r => r.FechaEntrega)
                    .HasColumnType("timestamptz");

                entity.HasOne(r => r.Producto)
                    .WithMany()
                    .HasForeignKey(r => r.ProductoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
