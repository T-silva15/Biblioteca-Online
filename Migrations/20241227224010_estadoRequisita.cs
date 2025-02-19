using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca_LAB.Migrations
{
    /// <inheritdoc />
    public partial class estadoRequisita : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Requisita",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Requisita");
        }
    }
}
