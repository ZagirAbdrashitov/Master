using Microsoft.EntityFrameworkCore.Migrations;

namespace DrugsManager.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drug",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ndc = table.Column<string>(maxLength: 8, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    PackSize = table.Column<int>(nullable: false),
                    Unit = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drug", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drug_Ndc",
                table: "Drug",
                column: "Ndc",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drug");
        }
    }
}
