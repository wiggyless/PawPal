using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawPal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PostForeignKeyAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnimalID",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AnimalID",
                table: "Posts",
                column: "AnimalID");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Animals_AnimalID",
                table: "Posts",
                column: "AnimalID",
                principalTable: "Animals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Animals_AnimalID",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_AnimalID",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "AnimalID",
                table: "Posts");
        }
    }
}
