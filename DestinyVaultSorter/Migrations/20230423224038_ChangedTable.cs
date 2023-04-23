using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DestinyVaultSorter.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "weaponId",
                table: "Weapons",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "weaponIconLink",
                table: "Weapons",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "weaponSlot",
                table: "Weapons",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "weaponIconLink",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "weaponSlot",
                table: "Weapons");

            migrationBuilder.AlterColumn<int>(
                name: "weaponId",
                table: "Weapons",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);
        }
    }
}
