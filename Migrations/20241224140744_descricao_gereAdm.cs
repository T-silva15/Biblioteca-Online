using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca_LAB.Migrations
{
    /// <inheritdoc />
    public partial class descricao_gereAdm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Gere_Adm",
                columns: table => new
                {
                    Id_Utilizador = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    Id_Admin = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    Data_GA = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Tipo_Alteracao = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Descricao = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gere_Adm", x => new { x.Id_Utilizador, x.Id_Admin, x.Data_GA });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gere_Adm");

        }
    }
}
