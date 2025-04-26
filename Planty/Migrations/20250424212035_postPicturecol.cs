using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planty.Migrations
{
    /// <inheritdoc />
    public partial class postPicturecol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostPicture",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostPicture",
                table: "Posts");
        }
    }
}
