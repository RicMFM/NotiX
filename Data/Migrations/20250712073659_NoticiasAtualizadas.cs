using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class NoticiasAtualizadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_Noticias_NoticiasId",
                table: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Utilizadores_NoticiasId",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "NoticiasId",
                table: "Utilizadores");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "671c35d4-9a21-4f81-85ff-cffd4cc19544", "AQAAAAIAAYagAAAAEHa+mOPHc75ZpKTQxBRQNPj0cRo0WPnbMZOfxrjqvU/IbaxbK2kt3HPoqNFe6VWJWw==", "d0058faf-4f55-4197-b3be-d8f00ffb40b1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "funcionario",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4fda67cf-47e6-4a9a-8c10-b20a545b4c2c", "AQAAAAIAAYagAAAAEPGtGGcS6/rqHOq/q1RxQvOvTyYy9O+Yr15PwxUrovZ41F8ya4SPxd+HphOAENy3qg==", "46b444e4-3565-44f6-826d-c875e03d088c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoticiasId",
                table: "Utilizadores",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "49390547-e32d-4dfe-b48f-cae962e22070", "AQAAAAIAAYagAAAAEDZJ0tg56MBmWiPtS+6YFWHgm5gTjC9U/322dfCG6ZNq1gbZjnXNDwOHSE43r4WqXA==", "98340025-63a2-4226-9a7d-23c2311c0abe" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "funcionario",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3d50a9de-8ff5-480e-b775-847d8a563165", "AQAAAAIAAYagAAAAEMuCO+0kY32wI92T3HUDs8b7ZcZwCnef+XwfRR13IhRgnTpqRRtEsuroLHmxPff8vw==", "b006f943-a1fe-4caa-be26-60fac91087a6" });

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_NoticiasId",
                table: "Utilizadores",
                column: "NoticiasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Noticias_NoticiasId",
                table: "Utilizadores",
                column: "NoticiasId",
                principalTable: "Noticias",
                principalColumn: "Id");
        }
    }
}
