using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class Jwt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d2fe7483-0ac9-4f35-868d-f4ed6f1d4de5", "AQAAAAIAAYagAAAAENMgHLhrHA152apXWI/Hy8ozHtOmVJW4MnPZwFTwPxd8Nun5Q9OICvhG0yX+nFioRQ==", "bd9378d0-9d36-4f07-9437-a77cf01134b7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e43fa463-5b8a-4bce-9733-96a534695d94", "AQAAAAIAAYagAAAAEFLGIJPfcEngb55jzuETe/dIfvk6/sXZxom2nXp6X8Nr3VkepccyVfkpRNDfq8uZpw==", "faa0b48b-13c7-4538-a05c-87a3c55f9f91" });
        }
    }
}
