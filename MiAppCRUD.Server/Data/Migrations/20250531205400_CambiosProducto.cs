using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MiAppCRUD.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class CambiosProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaProductoId",
                table: "productos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "EcuniPoints",
                table: "productos",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "productos",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "categoriasproducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoriasproducto", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_productos_CategoriaProductoId",
                table: "productos",
                column: "CategoriaProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_productos_categoriasproducto_CategoriaProductoId",
                table: "productos",
                column: "CategoriaProductoId",
                principalTable: "categoriasproducto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productos_categoriasproducto_CategoriaProductoId",
                table: "productos");

            migrationBuilder.DropTable(
                name: "categoriasproducto");

            migrationBuilder.DropIndex(
                name: "IX_productos_CategoriaProductoId",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "CategoriaProductoId",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "EcuniPoints",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "productos");
        }
    }
}
