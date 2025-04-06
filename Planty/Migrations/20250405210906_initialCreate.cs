using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planty.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlantImage",
                table: "Plants",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "PlantID",
                table: "Plants",
                newName: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Plants",
                newName: "PlantImage");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Plants",
                newName: "PlantID");
        }
    }
}
