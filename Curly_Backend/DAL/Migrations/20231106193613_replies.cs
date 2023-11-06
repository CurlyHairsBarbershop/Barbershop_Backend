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
                defaultValue: new DateTime(2023, 11, 6, 19, 36, 13, 738, DateTimeKind.Utc).AddTicks(6800),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 11, 1, 22, 15, 45, 440, DateTimeKind.Utc).AddTicks(1800));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -4,
                column: "ConcurrencyStamp",
                value: "46323c1d-4ec1-4a06-a4d2-a4e4eac9cc3e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -3,
                column: "ConcurrencyStamp",
                value: "abccfc05-ae46-45a2-980f-fb582a36a765");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -2,
                column: "ConcurrencyStamp",
                value: "1897e13c-6b94-4a02-a12c-f5bbac92b0c1");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: -1,
                column: "ConcurrencyStamp",
                value: "33b83c1e-3a58-470c-9b34-66184ff0956b");
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
                oldDefaultValue: new DateTime(2023, 11, 6, 19, 36, 13, 738, DateTimeKind.Utc).AddTicks(6800));

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
