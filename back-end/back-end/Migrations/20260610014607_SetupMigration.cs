using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace back_end.Migrations
{
    /// <inheritdoc />
    public partial class SetupMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ContentRatings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentRatings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Demographics",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demographics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ScanGroups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    websiteUrl = table.Column<string>(type: "text", nullable: true),
                    isActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanGroups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Titles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    synopsis = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    publicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    img = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DemographicId = table.Column<int>(type: "integer", nullable: false),
                    ContentRatingId = table.Column<int>(type: "integer", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.id);
                    table.ForeignKey(
                        name: "FK_Titles_ContentRatings_ContentRatingId",
                        column: x => x.ContentRatingId,
                        principalTable: "ContentRatings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Titles_Demographics_DemographicId",
                        column: x => x.DemographicId,
                        principalTable: "Demographics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Titles_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlternativeNames",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    TitleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlternativeNames", x => x.id);
                    table.ForeignKey(
                        name: "FK_AlternativeNames_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlternativeNames_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistTitle",
                columns: table => new
                {
                    Artistid = table.Column<int>(type: "integer", nullable: false),
                    Titleid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTitle", x => new { x.Artistid, x.Titleid });
                    table.ForeignKey(
                        name: "FK_ArtistTitle_Artists_Artistid",
                        column: x => x.Artistid,
                        principalTable: "Artists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistTitle_Titles_Titleid",
                        column: x => x.Titleid,
                        principalTable: "Titles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorTitle",
                columns: table => new
                {
                    Authorid = table.Column<int>(type: "integer", nullable: false),
                    Titleid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorTitle", x => new { x.Authorid, x.Titleid });
                    table.ForeignKey(
                        name: "FK_AuthorTitle_Authors_Authorid",
                        column: x => x.Authorid,
                        principalTable: "Authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorTitle_Titles_Titleid",
                        column: x => x.Titleid,
                        principalTable: "Titles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TitleId = table.Column<int>(type: "integer", nullable: false),
                    number = table.Column<decimal>(type: "numeric(5,1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.id);
                    table.CheckConstraint("CK_ChapterNumber", "number >= 0");
                    table.ForeignKey(
                        name: "FK_Chapters_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenreTitle",
                columns: table => new
                {
                    Genreid = table.Column<int>(type: "integer", nullable: false),
                    Titleid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreTitle", x => new { x.Genreid, x.Titleid });
                    table.ForeignKey(
                        name: "FK_GenreTitle_Genres_Genreid",
                        column: x => x.Genreid,
                        principalTable: "Genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreTitle_Titles_Titleid",
                        column: x => x.Titleid,
                        principalTable: "Titles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThemeTitle",
                columns: table => new
                {
                    Themeid = table.Column<int>(type: "integer", nullable: false),
                    Titleid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeTitle", x => new { x.Themeid, x.Titleid });
                    table.ForeignKey(
                        name: "FK_ThemeTitle_Themes_Themeid",
                        column: x => x.Themeid,
                        principalTable: "Themes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThemeTitle_Titles_Titleid",
                        column: x => x.Titleid,
                        principalTable: "Titles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChapterTranslations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    chapterTitle = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    isOfficial = table.Column<bool>(type: "boolean", nullable: false),
                    uploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    viewCount = table.Column<int>(type: "integer", nullable: false),
                    ChapterId = table.Column<int>(type: "integer", nullable: false),
                    ScanGroupId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterTranslations", x => x.id);
                    table.CheckConstraint("CK_ChapterTranslation_ViewCount", "\"viewCount\" >= 0");
                    table.ForeignKey(
                        name: "FK_ChapterTranslations_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChapterTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChapterTranslations_ScanGroups_ScanGroupId",
                        column: x => x.ScanGroupId,
                        principalTable: "ScanGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeNames_LanguageId",
                table: "AlternativeNames",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeNames_TitleId",
                table: "AlternativeNames",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTitle_Titleid",
                table: "ArtistTitle",
                column: "Titleid");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorTitle_Titleid",
                table: "AuthorTitle",
                column: "Titleid");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_TitleId_number",
                table: "Chapters",
                columns: new[] { "TitleId", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChapterTranslations_ChapterId_ScanGroupId_LanguageId",
                table: "ChapterTranslations",
                columns: new[] { "ChapterId", "ScanGroupId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChapterTranslations_LanguageId",
                table: "ChapterTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterTranslations_ScanGroupId",
                table: "ChapterTranslations",
                column: "ScanGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreTitle_Titleid",
                table: "GenreTitle",
                column: "Titleid");

            migrationBuilder.CreateIndex(
                name: "IX_ThemeTitle_Titleid",
                table: "ThemeTitle",
                column: "Titleid");

            migrationBuilder.CreateIndex(
                name: "IX_Titles_ContentRatingId",
                table: "Titles",
                column: "ContentRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Titles_DemographicId",
                table: "Titles",
                column: "DemographicId");

            migrationBuilder.CreateIndex(
                name: "IX_Titles_name",
                table: "Titles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Titles_StatusId",
                table: "Titles",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlternativeNames");

            migrationBuilder.DropTable(
                name: "ArtistTitle");

            migrationBuilder.DropTable(
                name: "AuthorTitle");

            migrationBuilder.DropTable(
                name: "ChapterTranslations");

            migrationBuilder.DropTable(
                name: "GenreTitle");

            migrationBuilder.DropTable(
                name: "ThemeTitle");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "ScanGroups");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropTable(
                name: "Titles");

            migrationBuilder.DropTable(
                name: "ContentRatings");

            migrationBuilder.DropTable(
                name: "Demographics");

            migrationBuilder.DropTable(
                name: "Statuses");
        }
    }
}
