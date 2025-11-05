using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawPal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AnimalHealthHistory_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnimalId",
                table: "AnimalHealthHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AnimalHealthHistories_AnimalId",
                table: "AnimalHealthHistories",
                column: "AnimalId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalHealthHistories_Animals_AnimalId",
                table: "AnimalHealthHistories",
                column: "AnimalId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalHealthHistories_Animals_AnimalId",
                table: "AnimalHealthHistories");

            migrationBuilder.DropIndex(
                name: "IX_AnimalHealthHistories_AnimalId",
                table: "AnimalHealthHistories");

            migrationBuilder.DropColumn(
                name: "AnimalId",
                table: "AnimalHealthHistories");
        }
    }
}
