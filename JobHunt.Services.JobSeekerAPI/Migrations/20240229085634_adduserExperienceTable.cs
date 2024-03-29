using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHunt.Services.JobSeekerAPI.Migrations
{
    /// <inheritdoc />
    public partial class adduserExperienceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserExperiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<DateOnly>(type: "date", nullable: false),
                    EndYear = table.Column<DateOnly>(type: "date", nullable: false),
                    CompanyUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExperiences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserExperiences_UserId",
                table: "UserExperiences",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserExperiences");
        }
    }
}
