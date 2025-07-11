using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotiX.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitLimpo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e43fa463-5b8a-4bce-9733-96a534695d94", "AQAAAAIAAYagAAAAEFLGIJPfcEngb55jzuETe/dIfvk6/sXZxom2nXp6X8Nr3VkepccyVfkpRNDfq8uZpw==", "faa0b48b-13c7-4538-a05c-87a3c55f9f91" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a949b764-c3d7-47c1-8ad4-5357cfbffd82", "AQAAAAIAAYagAAAAEGa7ZGk+Apud1SIS5zksteLmOQCf2U8ubgG2yUyNgrvFH/qFNWQzWIZVgZMxclzYvw==", "23ac6b82-4108-436b-bee3-793e88ed4679" });
        }
    }
}
