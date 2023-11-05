using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class replies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReplyTo",
                table: "Reviews",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlacedAt",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 5, 17, 6, 36, 836, DateTimeKind.Utc).AddTicks(1290),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 11, 1, 22, 15, 45, 440, DateTimeKind.Utc).AddTicks(1800));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -4,
                column: "ConcurrencyStamp",
                value: "bc1ad83a-205e-4895-b13e-59774176c2ec");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -3,
                column: "ConcurrencyStamp",
                value: "b47852a1-ea9c-4272-b961-082439876168");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -2,
                column: "ConcurrencyStamp",
                value: "45c3ebdb-6f61-449b-a44f-a8ea7f2570d3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                column: "ConcurrencyStamp",
                value: "2d24168d-5711-41ed-9a21-72d45bc4a34f");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyTo",
                table: "Reviews");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlacedAt",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 1, 22, 15, 45, 440, DateTimeKind.Utc).AddTicks(1800),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 11, 5, 17, 6, 36, 836, DateTimeKind.Utc).AddTicks(1290));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -4,
                column: "ConcurrencyStamp",
                value: "d6005819-9818-482e-b8d9-6c07efe5fb00");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -3,
                column: "ConcurrencyStamp",
                value: "bb60f1d6-2475-433e-9e83-30ec3fd2b2a5");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -2,
                column: "ConcurrencyStamp",
                value: "a907a74d-f807-4197-be22-1f020847586d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                column: "ConcurrencyStamp",
                value: "e8729713-3aed-4036-90d5-73688e0781bc");
        }
    }
}
