using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShoppingApp.Migrations
{
    public partial class Blog_CreateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7000487d-4011-4916-aba5-bdd35b5119f3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d74b8f26-e18f-4a81-bc79-33eedd13ec99");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "670d4342-8626-4786-8d41-3f0ac2233984", "1d4df2c6-e6a4-4983-b83b-47a15e986d73", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "942a2299-2ff4-4c10-8a3f-eb859eb472d9", "41c9f7a9-8c81-429e-82cf-6b627d90eba7", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "670d4342-8626-4786-8d41-3f0ac2233984");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "942a2299-2ff4-4c10-8a3f-eb859eb472d9");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Blogs");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7000487d-4011-4916-aba5-bdd35b5119f3", "a06fb5c9-91eb-42f2-a001-e36410534a63", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d74b8f26-e18f-4a81-bc79-33eedd13ec99", "dd87a17f-ac3f-43a6-b97e-f3789b0a1a69", "Admin", "ADMIN" });
        }
    }
}
