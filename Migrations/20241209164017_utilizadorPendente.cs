using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca_LAB.Migrations
{
    /// <inheritdoc />
    public partial class utilizadorPendente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BibPendente");

            migrationBuilder.CreateTable(
                name: "UtilizadorPendente",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    Nome = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    Contacto = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: true),
                    Cidade = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    Endereco = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    Codigo_Postal = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    Tipo = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Estado = table.Column<int>(type: "int", unicode: false, maxLength: 20, nullable: false),
                    DataSubmissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataResultado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminAprov = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Resultado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilizadorPendente", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UtilizadorPendente");

            migrationBuilder.CreateTable(
                name: "BibPendente",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
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
                    table.PrimaryKey("PK_BibPendente", x => x.Id);
                });
        }
    }
}
