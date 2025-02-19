using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca_LAB.Migrations
{
    /// <inheritdoc />
    public partial class fixId_U_P : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "UtilizadorPendente",
                type: "varchar(36)",
                unicode: false,
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldUnicode: false,
                oldMaxLength: 32);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "UtilizadorPendente",
                type: "varchar(32)",
                unicode: false,
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(36)",
                oldUnicode: false,
                oldMaxLength: 36);
        }
    }
}
