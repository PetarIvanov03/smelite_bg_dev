using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smelite_app.Migrations
{
    /// <inheritdoc />
    public partial class ApprenticeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apprenticeships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprenticeProfileId = table.Column<int>(type: "int", nullable: false),
                    MasterProfileId = table.Column<int>(type: "int", nullable: false),
                    CraftId = table.Column<int>(type: "int", nullable: false),
                    SelectedProps = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apprenticeships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apprenticeships_ApprenticeProfiles_ApprenticeProfileId",
                        column: x => x.ApprenticeProfileId,
                        principalTable: "ApprenticeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Apprenticeships_Crafts_CraftId",
                        column: x => x.CraftId,
                        principalTable: "Crafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Apprenticeships_MasterProfiles_MasterProfileId",
                        column: x => x.MasterProfileId,
                        principalTable: "MasterProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apprenticeships_ApprenticeProfileId",
                table: "Apprenticeships",
                column: "ApprenticeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Apprenticeships_CraftId",
                table: "Apprenticeships",
                column: "CraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Apprenticeships_MasterProfileId",
                table: "Apprenticeships",
                column: "MasterProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apprenticeships");
        }
    }
}
