using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace back_end.Migrations
{
    /// <inheritdoc />
    public partial class Pages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pageNumber = table.Column<int>(type: "integer", nullable: false),
                    imageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ChapterTranslationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.id);
                    table.CheckConstraint("CK_PageNumber", "\"pageNumber\" >= 1");
                    table.ForeignKey(
                        name: "FK_Pages_ChapterTranslations_ChapterTranslationId",
                        column: x => x.ChapterTranslationId,
                        principalTable: "ChapterTranslations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ChapterTranslationId_pageNumber",
                table: "Pages",
                columns: new[] { "ChapterTranslationId", "pageNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pages");
        }
    }
}
