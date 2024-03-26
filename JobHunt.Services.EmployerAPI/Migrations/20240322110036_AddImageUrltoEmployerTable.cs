using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHunt.Services.EmployerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrltoEmployerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Employers");
        }
    }
}
