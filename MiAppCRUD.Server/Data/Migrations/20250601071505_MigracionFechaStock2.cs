﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiAppCRUD.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigracionFechaStock2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaEntrega",
                table: "reabastecimientosstock",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaEntrega",
                table: "reabastecimientosstock",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
