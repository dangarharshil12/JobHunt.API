using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHunt.Services.EmployerAPI.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationStatusColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationStatus",
                table: "UserVacancyRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationStatus",
                table: "UserVacancyRequests");
        }
    }
}
