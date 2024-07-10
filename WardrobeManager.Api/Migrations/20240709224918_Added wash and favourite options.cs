using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WardrobeManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class Addedwashandfavouriteoptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DesiredTimesWornBeforeWash",
                table: "ClothingItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Favourited",
                table: "ClothingItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TimesWornSinceWash",
                table: "ClothingItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimesWornTotal",
                table: "ClothingItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DesiredTimesWornBeforeWash",
                table: "ClothingItems");

            migrationBuilder.DropColumn(
                name: "Favourited",
                table: "ClothingItems");

            migrationBuilder.DropColumn(
                name: "TimesWornSinceWash",
                table: "ClothingItems");

            migrationBuilder.DropColumn(
                name: "TimesWornTotal",
                table: "ClothingItems");
        }
    }
}
