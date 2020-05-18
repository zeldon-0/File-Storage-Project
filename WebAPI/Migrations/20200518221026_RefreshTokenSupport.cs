using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class RefreshTokenSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(nullable: true),
                    Expiration = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "27e3257b-98ce-4138-ac0f-f7a5e04ee7fb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "ce786df8-c35a-46b2-82a2-a93e9eba9ee7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "5f721e58-fc83-4d55-935d-f41ec4d5a28f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "938650b4-c83e-4948-a57e-238d0b5475a9", "AQAAAAEAACcQAAAAEEp1fRCL5m3o7gdxaWUbnB/Iual7/AhHMTsv8xL3DTUKLpUvx/nxgi2DOtw/LrvQgA==", "002453c1-43fe-454a-827e-2f2257134f59" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fcde4d2e-7127-4347-88d5-8ae3dce9238a", "AQAAAAEAACcQAAAAEIw9Ph5Mp/bgMueUTAC/GaFRKe9yBBV212DOhnBgvcq32uJhiuZEf1HKMRZvyz5GKA==", "ef999256-2648-4bea-a3f7-ef369e51d085" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6cae57be-2940-4825-8e01-8fb367744ce1", "AQAAAAEAACcQAAAAEJcCHAbRCiz7uoKaByyURcLR9F3abWM+pQsRwc56RczjjAaw01qjzc8AaP6UYShNfQ==", "14f84676-79be-4f0e-87e4-c319d9d90aa0" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "180cb5b3-084a-4657-9ff4-5ec125f88603");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "27fd1ca5-290d-43fe-b1f2-2d8dc2976e39");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "9275304c-6f3e-4751-950a-9950dae55fd1");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95508c0a-eaa5-427e-b32a-94b4a8031bfc", "AQAAAAEAACcQAAAAEE/Vu3Uj7dT8RCp4odCkPNcgeFkNmtMmbKVbBEEXRNkvajp9RIDx1m5ip6QTCPEicA==", "3179cac1-fa4b-48bd-a139-f93287baa317" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2de2fdeb-a935-42ad-b038-d3121042db14", "AQAAAAEAACcQAAAAEHi0JIlmF39JHNXHvf5x3OS467JlX/Y7kOr5A3f9H+U9ZI76QnTtp08WHJo+Eq/8vA==", "db3776c8-f23d-4cb6-bfa9-261b62937007" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7817322f-085b-4847-b129-254e867f3164", "AQAAAAEAACcQAAAAEAJInVp+FtZDIT3oL8Klwh+ZfkPpjKMtbOd5QVYnwSqkU9VxVHd9OpY3pqPNH+Fkfg==", "54089f61-c2b9-4f5e-8102-ceaf451cab3a" });
        }
    }
}
