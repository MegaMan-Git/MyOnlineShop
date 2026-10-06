using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "asdfghzxcvbnmsdgxbh", "Admin", "ADMIN" },
                    { "2", "asdsbnmsdgxKNDnjbhakhsb", "Seller", "SELLER" },
                    { "3", "hslhdlsjdjfkdsbksddkj", "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "11111111-1111-1111-1111-111111111111", 0, "ONMLKJIHGFEDCBA", "rafieyg2708@gmail.com", true, false, null, "RAFIEYG2708@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEJ3Z1lrLkYaNpXaQFhyQG41AQ81ch98O2hkvi24Lf3fTfzr/iKrWiI3bNeQZGeQx7w==", null, false, "ABCDEFGHIJKLMNO", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "11111111-1111-1111-1111-111111111111" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "11111111-1111-1111-1111-111111111111" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111");
        }
    }
}
