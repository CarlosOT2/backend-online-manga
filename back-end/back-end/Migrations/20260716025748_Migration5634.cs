using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_end.Migrations
{
    /// <inheritdoc />
    public partial class Migration5634 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlternativeNames_Languages_LanguageId",
                table: "AlternativeNames");

            migrationBuilder.RenameColumn(
                name: "LanguageId",
                table: "AlternativeNames",
                newName: "Languageid");

            migrationBuilder.RenameIndex(
                name: "IX_AlternativeNames_LanguageId",
                table: "AlternativeNames",
                newName: "IX_AlternativeNames_Languageid");

            migrationBuilder.AlterColumn<int>(
                name: "Languageid",
                table: "AlternativeNames",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "AlternativeNames",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeNames_LanguageId",
                table: "AlternativeNames",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlternativeNames_Languages_LanguageId",
                table: "AlternativeNames",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AlternativeNames_Languages_Languageid",
                table: "AlternativeNames",
                column: "Languageid",
                principalTable: "Languages",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlternativeNames_Languages_LanguageId",
                table: "AlternativeNames");

            migrationBuilder.DropForeignKey(
                name: "FK_AlternativeNames_Languages_Languageid",
                table: "AlternativeNames");

            migrationBuilder.DropIndex(
                name: "IX_AlternativeNames_LanguageId",
                table: "AlternativeNames");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "AlternativeNames");

            migrationBuilder.RenameColumn(
                name: "Languageid",
                table: "AlternativeNames",
                newName: "LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_AlternativeNames_Languageid",
                table: "AlternativeNames",
                newName: "IX_AlternativeNames_LanguageId");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageId",
                table: "AlternativeNames",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AlternativeNames_Languages_LanguageId",
                table: "AlternativeNames",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
