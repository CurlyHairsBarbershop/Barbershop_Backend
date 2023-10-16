using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AdminAlias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Barbers_BarberId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Clients_PublisherId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameIndex(
                name: "IX_Review_PublisherId",
                table: "Reviews",
                newName: "IX_Reviews_PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_BarberId",
                table: "Reviews",
                newName: "IX_Reviews_BarberId");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Barbers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlacedAt",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 15, 18, 35, 37, 476, DateTimeKind.Utc).AddTicks(4940),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 10, 15, 18, 11, 55, 541, DateTimeKind.Utc).AddTicks(7180));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    AdminAlias = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { -3, null, "Admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Barbers_BarberId",
                table: "Reviews",
                column: "BarberId",
                principalTable: "Barbers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Clients_PublisherId",
                table: "Reviews",
                column: "PublisherId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Barbers_BarberId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Clients_PublisherId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Barbers");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_PublisherId",
                table: "Review",
                newName: "IX_Review_PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_BarberId",
                table: "Review",
                newName: "IX_Review_BarberId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlacedAt",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 15, 18, 11, 55, 541, DateTimeKind.Utc).AddTicks(7180),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 10, 15, 18, 35, 37, 476, DateTimeKind.Utc).AddTicks(4940));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Barbers_BarberId",
                table: "Review",
                column: "BarberId",
                principalTable: "Barbers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Clients_PublisherId",
                table: "Review",
                column: "PublisherId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
