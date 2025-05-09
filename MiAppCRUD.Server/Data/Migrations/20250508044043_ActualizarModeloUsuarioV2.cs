using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiAppCRUD.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarModeloUsuarioV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NombreUsuario",
                table: "usuarios",
                newName: "Provincia");

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "usuarios",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "usuarios",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "usuarios",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Edad",
                table: "usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Genero",
                table: "usuarios",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "usuarios",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_Correo",
                table: "usuarios",
                column: "Correo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_usuarios_Correo",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Edad",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Genero",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "usuarios");

            migrationBuilder.RenameColumn(
                name: "Provincia",
                table: "usuarios",
                newName: "NombreUsuario");
        }
    }
}
