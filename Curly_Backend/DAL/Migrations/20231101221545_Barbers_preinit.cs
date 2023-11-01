using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Barbers_preinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PlacedAt",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 1, 22, 15, 45, 440, DateTimeKind.Utc).AddTicks(1800),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 10, 16, 9, 26, 56, 578, DateTimeKind.Utc).AddTicks(9750));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { -4, 0, "d6005819-9818-482e-b8d9-6c07efe5fb00", "20werasdf@gmail.com", false, "Mykhailo", "Tkachenko", false, null, null, null, null, "0970790944", false, null, false, null },
                    { -3, 0, "bb60f1d6-2475-433e-9e83-30ec3fd2b2a5", "maxbobryk@gmail.com", false, "Maksym", "Bobryk", false, null, null, null, null, "48934909434", false, null, false, null },
                    { -2, 0, "a907a74d-f807-4197-be22-1f020847586d", "alext@gmail.com", false, "Alex", "Thompson", false, null, null, null, null, "9478920606", false, null, false, null },
                    { -1, 0, "e8729713-3aed-4036-90d5-73688e0781bc", "johnjj@gmail.com", false, "John", "Johnson", false, null, null, null, null, "927804723", false, null, false, null }
                });

            migrationBuilder.InsertData(
                table: "Barbers",
                columns: new[] { "Id", "Image" },
                values: new object[,]
                {
                    { -4, null },
                    { -3, null },
                    { -2, null },
                    { -1, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Barbers",
                keyColumn: "Id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "Barbers",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "Barbers",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Barbers",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlacedAt",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 16, 9, 26, 56, 578, DateTimeKind.Utc).AddTicks(9750),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 11, 1, 22, 15, 45, 440, DateTimeKind.Utc).AddTicks(1800));
        }
    }
}
