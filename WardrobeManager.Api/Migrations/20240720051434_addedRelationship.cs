using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WardrobeManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class addedRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClothingItems_Users_OwnerId",
                table: "ClothingItems");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "ClothingItems",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ClothingItems_OwnerId",
                table: "ClothingItems",
                newName: "IX_ClothingItems_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClothingItems_Users_UserId",
                table: "ClothingItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClothingItems_Users_UserId",
                table: "ClothingItems");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ClothingItems",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_ClothingItems_UserId",
                table: "ClothingItems",
                newName: "IX_ClothingItems_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClothingItems_Users_OwnerId",
                table: "ClothingItems",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
