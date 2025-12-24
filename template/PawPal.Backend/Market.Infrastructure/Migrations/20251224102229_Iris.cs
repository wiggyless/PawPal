using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawPal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Iris : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdoptionRequests_AdoptionRequirements_RequirementId",
                table: "AdoptionRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AdoptionRequests_Posts_PostId",
                table: "AdoptionRequests");

            migrationBuilder.AlterColumn<int>(
                name: "RequirementId",
                table: "AdoptionRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "AdoptionRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdoptionRequests_AdoptionRequirements_RequirementId",
                table: "AdoptionRequests",
                column: "RequirementId",
                principalTable: "AdoptionRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdoptionRequests_Posts_PostId",
                table: "AdoptionRequests",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdoptionRequests_AdoptionRequirements_RequirementId",
                table: "AdoptionRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AdoptionRequests_Posts_PostId",
                table: "AdoptionRequests");

            migrationBuilder.AlterColumn<int>(
                name: "RequirementId",
                table: "AdoptionRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "AdoptionRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AdoptionRequests_AdoptionRequirements_RequirementId",
                table: "AdoptionRequests",
                column: "RequirementId",
                principalTable: "AdoptionRequirements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdoptionRequests_Posts_PostId",
                table: "AdoptionRequests",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
