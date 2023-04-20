using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DestinyVaultSorter.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    weaponId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    weaponName = table.Column<string>(type: "TEXT", nullable: false),
                    weaponType = table.Column<string>(type: "TEXT", nullable: false),
                    weaponElement = table.Column<string>(type: "TEXT", nullable: false),
                    weaponLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.weaponId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weapons");
        }
    }
}
