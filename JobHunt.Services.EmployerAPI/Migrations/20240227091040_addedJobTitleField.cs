using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHunt.Services.EmployerAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedJobTitleField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "VacancyDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "VacancyDetails");
        }
    }
}
