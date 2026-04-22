using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawPal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdoptionRequirementFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "YardAvailable",
                table: "AdoptionRequirements",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "OtherPetsAround",
                table: "AdoptionRequirements",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "ElderlyAround",
                table: "AdoptionRequirements",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "ChildrenAround",
                table: "AdoptionRequirements",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AdoptionRequirements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Aggressiveness",
                table: "AdoptionRequirements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Allergy",
                table: "AdoptionRequirements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpDetails",
                table: "AdoptionRequirements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinalComment",
                table: "AdoptionRequirements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FloorNumber",
                table: "AdoptionRequirements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseDetials",
                table: "AdoptionRequirements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsGift",
                table: "AdoptionRequirements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PeopleAva",
                table: "AdoptionRequirements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "PetExp",
                table: "AdoptionRequirements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanedStay",
                table: "AdoptionRequirements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "SumMoney",
                table: "AdoptionRequirements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TakeBack",
                table: "AdoptionRequirements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YardDetails",
                table: "AdoptionRequirements",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AdoptionRequirements");

            migrationBuilder.DropColumn(
                name: "Aggressiveness",
                table: "AdoptionRequirements");

            migrationBuilder.DropColumn(
                name: "Allergy",
                table: "AdoptionRequirements");

            migrationBuilder.DropColumn(
                name: "ExpDetails",
                table: "AdoptionRequirements");

            migrationBuilder.DropColumn(
                name: "FinalComment",
                table: "AdoptionRequirements");

            migrationBuilder.DropColumn(
                name: "FloorNumber",
                table: "AdoptionRequirements");

            migrationBuilder.DropColumn(
                name: "HouseDetials",
                table: "AdoptionRequirements");

            migrationBuilder.DropColumn(
                name: "IsGift",
                table: "AdoptionRequirements");

            migrationBuilder.DropColumn(
                name: "PeopleAva",
                table: "AdoptionRequirements");

            migrationBuilder.DropColumn(
                name: "PetExp",
                table: "AdoptionRequirements");

            migrationBuilder.DropColumn(
                name: "PlanedStay",
                table: "AdoptionRequirements");

            migrationBuilder.DropColumn(
                name: "SumMoney",
                table: "AdoptionRequirements");

            migrationBuilder.DropColumn(
                name: "TakeBack",
                table: "AdoptionRequirements");

            migrationBuilder.DropColumn(
                name: "YardDetails",
                table: "AdoptionRequirements");

            migrationBuilder.AlterColumn<bool>(
                name: "YardAvailable",
                table: "AdoptionRequirements",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "OtherPetsAround",
                table: "AdoptionRequirements",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ElderlyAround",
                table: "AdoptionRequirements",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ChildrenAround",
                table: "AdoptionRequirements",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
