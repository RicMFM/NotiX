using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserFuncionario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "49390547-e32d-4dfe-b48f-cae962e22070", "AQAAAAIAAYagAAAAEDZJ0tg56MBmWiPtS+6YFWHgm5gTjC9U/322dfCG6ZNq1gbZjnXNDwOHSE43r4WqXA==", "98340025-63a2-4226-9a7d-23c2311c0abe" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "funcionario", 0, "3d50a9de-8ff5-480e-b775-847d8a563165", "func@func.pt", true, false, null, "FUNC@FUNC.PT", "FUNC@FUNC.PT", "AQAAAAIAAYagAAAAEMuCO+0kY32wI92T3HUDs8b7ZcZwCnef+XwfRR13IhRgnTpqRRtEsuroLHmxPff8vw==", null, false, "b006f943-a1fe-4caa-be26-60fac91087a6", false, "func@func.pt" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f", "funcionario" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f", "funcionario" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "funcionario");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d2fe7483-0ac9-4f35-868d-f4ed6f1d4de5", "AQAAAAIAAYagAAAAENMgHLhrHA152apXWI/Hy8ozHtOmVJW4MnPZwFTwPxd8Nun5Q9OICvhG0yX+nFioRQ==", "bd9378d0-9d36-4f07-9437-a77cf01134b7" });
        }
    }
}
