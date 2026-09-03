using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawPal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LinkSecurityAnswersToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "SecurityAnswers",
                type: "int",
                nullable: true);

            // Backfill UserId from the existing Email column before it is dropped.
            migrationBuilder.Sql(@"
                UPDATE sa
                SET sa.UserId = u.Id
                FROM SecurityAnswers sa
                INNER JOIN Users u ON u.Email = sa.Email;
            ");

            // Answers that can't be tied to an existing user (e.g. the account was later
            // deleted/renamed) have no valid owner anymore; they can't be kept.
            migrationBuilder.Sql(@"
                DELETE FROM SecurityAnswers WHERE UserId IS NULL;
            ");

            // The (UserId, QuestionID) pair is about to become unique; drop older duplicates
            // that were allowed to accumulate before that constraint existed, keeping the newest.
            migrationBuilder.Sql(@"
                ;WITH Ranked AS (
                    SELECT Id, ROW_NUMBER() OVER (
                        PARTITION BY UserId, QuestionID
                        ORDER BY CreatedAtUtc DESC, Id DESC
                    ) AS RowNum
                    FROM SecurityAnswers
                )
                DELETE FROM SecurityAnswers WHERE Id IN (SELECT Id FROM Ranked WHERE RowNum > 1);
            ");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "SecurityAnswers");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "SecurityAnswers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SecurityAnswers_UserId_QuestionID",
                table: "SecurityAnswers",
                columns: new[] { "UserId", "QuestionID" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityAnswers_Users_UserId",
                table: "SecurityAnswers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityAnswers_Users_UserId",
                table: "SecurityAnswers");

            migrationBuilder.DropIndex(
                name: "IX_SecurityAnswers_UserId_QuestionID",
                table: "SecurityAnswers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "SecurityAnswers",
                type: "nvarchar(max)",
                nullable: true);

            // Best-effort restore based on the users' current email; if a user's email changed
            // after this migration ran, the original historical value cannot be recovered.
            migrationBuilder.Sql(@"
                UPDATE sa
                SET sa.Email = u.Email
                FROM SecurityAnswers sa
                INNER JOIN Users u ON u.Id = sa.UserId;
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "SecurityAnswers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SecurityAnswers");
        }
    }
}
