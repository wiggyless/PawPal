using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawPal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserImageFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "UserImage");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "UserImage",
                newName: "PhotoURL");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserImage",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoURL",
                table: "UserImage",
                newName: "ContentType");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserImage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "UserImage",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
