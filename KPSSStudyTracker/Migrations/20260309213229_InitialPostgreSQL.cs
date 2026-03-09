using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KPSSStudyTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgreSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dersler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dersler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MotivasyonSozleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Author = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsCustom = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivasyonSozleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Konular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    LessonId = table.Column<int>(type: "integer", nullable: false),
                    Source = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Konular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Konular_Dersler_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalismaPlanlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BlockName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    WeekNumber = table.Column<int>(type: "integer", nullable: false),
                    DayNumber = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Topics = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalismaPlanlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalismaPlanlari_Kullanicilar_UserId",
                        column: x => x.UserId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Denemeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Denemeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Denemeler_Kullanicilar_UserId",
                        column: x => x.UserId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HaftalikPlanlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WeekNumber = table.Column<int>(type: "integer", nullable: false),
                    PlanTitle = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HaftalikPlanlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HaftalikPlanlar_Kullanicilar_UserId",
                        column: x => x.UserId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Yapilacaklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yapilacaklar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Yapilacaklar_Kullanicilar_UserId",
                        column: x => x.UserId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciKonuIlerlemeleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TopicId = table.Column<int>(type: "integer", nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    SolvedQuestions = table.Column<int>(type: "integer", nullable: false),
                    CorrectAnswers = table.Column<int>(type: "integer", nullable: false),
                    WrongAnswers = table.Column<int>(type: "integer", nullable: false),
                    Source = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciKonuIlerlemeleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KullaniciKonuIlerlemeleri_Konular_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Konular",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KullaniciKonuIlerlemeleri_Kullanicilar_UserId",
                        column: x => x.UserId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DenemeSonuclari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MockExamId = table.Column<int>(type: "integer", nullable: false),
                    LessonId = table.Column<int>(type: "integer", nullable: false),
                    Correct = table.Column<int>(type: "integer", nullable: false),
                    Wrong = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DenemeSonuclari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DenemeSonuclari_Denemeler_MockExamId",
                        column: x => x.MockExamId,
                        principalTable: "Denemeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DenemeSonuclari_Dersler_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GunlukPlanlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WeeklyPlanId = table.Column<int>(type: "integer", nullable: false),
                    DayNumber = table.Column<int>(type: "integer", nullable: false),
                    LessonId = table.Column<int>(type: "integer", nullable: false),
                    TopicCount = table.Column<int>(type: "integer", nullable: false),
                    DailyGoal = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GunlukPlanlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GunlukPlanlar_Dersler_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GunlukPlanlar_HaftalikPlanlar_WeeklyPlanId",
                        column: x => x.WeeklyPlanId,
                        principalTable: "HaftalikPlanlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanKonulari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WeeklyPlanId = table.Column<int>(type: "integer", nullable: false),
                    DailyPlanId = table.Column<int>(type: "integer", nullable: false),
                    TopicId = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanKonulari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanKonulari_GunlukPlanlar_DailyPlanId",
                        column: x => x.DailyPlanId,
                        principalTable: "GunlukPlanlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanKonulari_HaftalikPlanlar_WeeklyPlanId",
                        column: x => x.WeeklyPlanId,
                        principalTable: "HaftalikPlanlar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlanKonulari_Konular_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Konular",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalismaPlanlari_UserId",
                table: "CalismaPlanlari",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Denemeler_UserId",
                table: "Denemeler",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DenemeSonuclari_LessonId",
                table: "DenemeSonuclari",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_DenemeSonuclari_MockExamId",
                table: "DenemeSonuclari",
                column: "MockExamId");

            migrationBuilder.CreateIndex(
                name: "IX_GunlukPlanlar_LessonId",
                table: "GunlukPlanlar",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_GunlukPlanlar_WeeklyPlanId",
                table: "GunlukPlanlar",
                column: "WeeklyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_HaftalikPlanlar_UserId",
                table: "HaftalikPlanlar",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Konular_LessonId",
                table: "Konular",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciKonuIlerlemeleri_TopicId",
                table: "KullaniciKonuIlerlemeleri",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciKonuIlerlemeleri_UserId",
                table: "KullaniciKonuIlerlemeleri",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciKonuIlerlemeleri_UserId_TopicId",
                table: "KullaniciKonuIlerlemeleri",
                columns: new[] { "UserId", "TopicId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_Username",
                table: "Kullanicilar",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanKonulari_DailyPlanId",
                table: "PlanKonulari",
                column: "DailyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanKonulari_TopicId",
                table: "PlanKonulari",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanKonulari_WeeklyPlanId",
                table: "PlanKonulari",
                column: "WeeklyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Yapilacaklar_Date",
                table: "Yapilacaklar",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Yapilacaklar_UserId_Date",
                table: "Yapilacaklar",
                columns: new[] { "UserId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalismaPlanlari");

            migrationBuilder.DropTable(
                name: "DenemeSonuclari");

            migrationBuilder.DropTable(
                name: "KullaniciKonuIlerlemeleri");

            migrationBuilder.DropTable(
                name: "MotivasyonSozleri");

            migrationBuilder.DropTable(
                name: "PlanKonulari");

            migrationBuilder.DropTable(
                name: "Yapilacaklar");

            migrationBuilder.DropTable(
                name: "Denemeler");

            migrationBuilder.DropTable(
                name: "GunlukPlanlar");

            migrationBuilder.DropTable(
                name: "Konular");

            migrationBuilder.DropTable(
                name: "HaftalikPlanlar");

            migrationBuilder.DropTable(
                name: "Dersler");

            migrationBuilder.DropTable(
                name: "Kullanicilar");
        }
    }
}
