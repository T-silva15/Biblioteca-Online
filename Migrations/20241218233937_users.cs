using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca_LAB.Migrations
{
    /// <inheritdoc />
    public partial class users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UtilizadorPendente");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Utilizador",
                type: "varchar(36)",
                unicode: false,
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldUnicode: false,
                oldMaxLength: 32);

            migrationBuilder.AddColumn<bool>(
                name: "Block",
                table: "Utilizador",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataBlock",
                table: "Utilizador",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataSub",
                table: "Utilizador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Block",
                table: "Utilizador");

            migrationBuilder.DropColumn(
                name: "DataBlock",
                table: "Utilizador");

            migrationBuilder.DropColumn(
                name: "DataSub",
                table: "Utilizador");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Utilizador",
                type: "varchar(32)",
                unicode: false,
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(36)",
                oldUnicode: false,
                oldMaxLength: 36);

            migrationBuilder.CreateTable(
                name: "UtilizadorPendente",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    AdminAprov = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Cidade = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    Codigo_Postal = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    Contacto = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: true),
                    DataResultado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataSubmissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    Endereco = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    Estado = table.Column<int>(type: "int", unicode: false, maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Resultado = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Username = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilizadorPendente", x => x.Id);
                });
        }
    }
}
