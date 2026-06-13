using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KASHOP13.DAL.Migrations
{
    /// <inheritdoc />
    public partial class brandfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "logo",
                table: "BrandTranslations",
                newName: "Logo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "BrandTranslations",
                newName: "logo");
        }
    }
}
