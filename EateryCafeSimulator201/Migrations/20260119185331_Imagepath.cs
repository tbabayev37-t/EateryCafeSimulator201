using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EateryCafeSimulator201.Migrations
{
    /// <inheritdoc />
    public partial class Imagepath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Cheffs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Cheffs");
        }
    }
}
