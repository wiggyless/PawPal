using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawPal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatePostFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnimalHistoryId",
                table: "Posts",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimalHistoryId",
                table: "Posts");
        }
    }
}
