using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class LastChangeDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastChange",
                table: "Folders",
                nullable: false,
                defaultValue:  DateTime.Now);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChange",
                table: "Files",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7c3b41c7-2444-43ac-af87-f16454e68b36");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2beda1d0-feac-44e8-89bc-4f0c2f693290");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "67bab011-6d45-4b14-bf8d-01c9f2460f31");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a9359dd2-f3ba-4a6f-9f65-d128010c9c4b", "AQAAAAEAACcQAAAAEJBJAMSnjw6N4n7mgZaLuRPunqO/t9YVtyb4HEPz3pL/NWJ5VQv/tFIUKpqSNArSmQ==", "dc51d634-fa3f-4633-a7b7-9a0e0da63e40" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "42c67d7c-e8ce-4a6f-b809-2adbe6e58b60", "AQAAAAEAACcQAAAAEIcx1vhoWgvePa4K0f5aOyRnLoqTmSb6LwLRoilhdR5VQ33YmmHX5HhzgN+eDH23SA==", "79cf1271-542d-4206-9ff7-85459b1d093e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6184a50a-0587-482f-9854-023032c019df", "AQAAAAEAACcQAAAAEJRhSJfumflaqi7OksTdWyfEZ2WTB8WW+/5kYWkvhetl6Vtt++7b9xl2kZoxrqdNmw==", "7da86642-8a74-4076-bc21-11180e936d5d" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastChange",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "LastChange",
                table: "Files");

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
        }
    }
}
