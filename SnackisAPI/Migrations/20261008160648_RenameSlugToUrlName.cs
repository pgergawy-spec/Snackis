using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnackisAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameSlugToUrlName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "Topics",
                newName: "UrlName");

            migrationBuilder.RenameIndex(
                name: "IX_Topics_Slug",
                table: "Topics",
                newName: "IX_Topics_UrlName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UrlName",
                table: "Topics",
                newName: "Slug");

            migrationBuilder.RenameIndex(
                name: "IX_Topics_UrlName",
                table: "Topics",
                newName: "IX_Topics_Slug");
        }
    }
}
