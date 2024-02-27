using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHunt.Services.EmployerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddVacancyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VacancyDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublishedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    NoOfVacancies = table.Column<int>(type: "int", nullable: false),
                    MinimumQualification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExperienceRequired = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MinimumSalary = table.Column<double>(type: "float", nullable: false),
                    MaximumSalary = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacancyDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VacancyDetails");
        }
    }
}
