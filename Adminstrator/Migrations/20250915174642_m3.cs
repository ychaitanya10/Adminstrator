using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adminstrator.Migrations
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Users_UserId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Speakers_Users_UserId",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Speakers_UserId",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Participants_UserId",
                table: "Participants");

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_UserId",
                table: "Speakers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_UserId",
                table: "Participants",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Users_UserId",
                table: "Participants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Speakers_Users_UserId",
                table: "Speakers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Users_UserId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Speakers_Users_UserId",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Speakers_UserId",
                table: "Speakers");

            migrationBuilder.DropIndex(
                name: "IX_Participants_UserId",
                table: "Participants");

            migrationBuilder.CreateIndex(
                name: "IX_Speakers_UserId",
                table: "Speakers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_UserId",
                table: "Participants",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Users_UserId",
                table: "Participants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Speakers_Users_UserId",
                table: "Speakers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
