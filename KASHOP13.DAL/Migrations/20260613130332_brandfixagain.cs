using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KASHOP13.DAL.Migrations
{
    /// <inheritdoc />
    public partial class brandfixagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "BrandTranslations");

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Brands");

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "BrandTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
