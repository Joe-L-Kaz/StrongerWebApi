using Microsoft.EntityFrameworkCore.Migrations;
using Stronger.Domain.Enums;

#nullable disable

namespace Stronger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Role" },
                values: new object[,]
                {
                    { 1, Role.Admin.ToString() },
                    { 2, Role.User.ToString() },
                    { 3, Role.PremiumUser.ToString() }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id" ,
                keyValues: new object[]
                {
                   1,
                   2,
                   3
                });
        }
    }
}
