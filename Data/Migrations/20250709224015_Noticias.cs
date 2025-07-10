using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NotiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class Noticias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a949b764-c3d7-47c1-8ad4-5357cfbffd82", "AQAAAAIAAYagAAAAEGa7ZGk+Apud1SIS5zksteLmOQCf2U8ubgG2yUyNgrvFH/qFNWQzWIZVgZMxclzYvw==", "23ac6b82-4108-436b-bee3-793e88ed4679" });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Categoria" },
                values: new object[,]
                {
                    { 1, "Música" },
                    { 2, "Cultura" },
                    { 3, "Tecnologia" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bd742d3f-ebb1-404d-bca4-0c8a9bab9bf3", "AQAAAAIAAYagAAAAEK28EbuslIWK2wsjliG8IiNOO1lI7a/V7vEMgoswgHcfLXpnEqQPMkzLXaIBy417kw==", "0e08c63f-d09b-4f21-9fbf-f18e092ed4ea" });
        }
    }
}
