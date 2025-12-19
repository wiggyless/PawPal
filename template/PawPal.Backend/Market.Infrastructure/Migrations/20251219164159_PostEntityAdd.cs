using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawPal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PostEntityAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Animals_AnimalID",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Cities_CityId",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Posts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AnimalID",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnimalHealthHistoryId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AnimalHealthHistoryId",
                table: "Posts",
                column: "AnimalHealthHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AnimalHealthHistories_AnimalHealthHistoryId",
                table: "Posts",
                column: "AnimalHealthHistoryId",
                principalTable: "AnimalHealthHistories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Animals_AnimalID",
                table: "Posts",
                column: "AnimalID",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Cities_CityId",
                table: "Posts",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AnimalHealthHistories_AnimalHealthHistoryId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Animals_AnimalID",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Cities_CityId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_AnimalHealthHistoryId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "AnimalHealthHistoryId",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AnimalID",
                table: "Posts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Animals_AnimalID",
                table: "Posts",
                column: "AnimalID",
                principalTable: "Animals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Cities_CityId",
                table: "Posts",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
