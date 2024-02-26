using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHunt.Services.AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class EmployerRoleChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5ff41ac-98fa-4003-950d-d5bbde128546",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Employer", "EMPLOYER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1f611e4b-35c2-4061-916e-a64c93b3b745",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7caa620a-7741-4175-aa4d-f988f668f10d", "AQAAAAIAAYagAAAAELGZ0wQdy9W9B/RzbEAHgucMqrpX+U+HrPzc6eQmdM9OZXad6sbK4j115LV9UsgFmQ==", "a5eb5ae9-bd7d-4e59-874a-37db022bab85" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5ff41ac-98fa-4003-950d-d5bbde128546",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Employeer", "EMPLOYEER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1f611e4b-35c2-4061-916e-a64c93b3b745",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "934040d3-f1f0-4db1-b906-3ca0696400fa", "AQAAAAIAAYagAAAAEHEiAXfp2K4q5cvrm2dwTMFQtOOcYvOUh+CZt8UTOOgZ2/3Tj1Td0sfyZK7TCp6chQ==", "2ac1606b-88d8-4515-9554-533210f9bd89" });
        }
    }
}
