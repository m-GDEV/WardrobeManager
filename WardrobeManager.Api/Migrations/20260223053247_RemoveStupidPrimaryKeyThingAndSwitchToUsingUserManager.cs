using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WardrobeManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStupidPrimaryKeyThingAndSwitchToUsingUserManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClothingItems_AspNetUsers_UserId",
                table: "ClothingItems");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_PrimaryKeyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrimaryKeyId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PrimaryKeyId",
                table: "Logs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PrimaryKeyId",
                table: "ClothingItems",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ClothingItems",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_ClothingItems_AspNetUsers_UserId",
                table: "ClothingItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClothingItems_AspNetUsers_UserId",
                table: "ClothingItems");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Logs",
                newName: "PrimaryKeyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ClothingItems",
                newName: "PrimaryKeyId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ClothingItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "PrimaryKeyId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_PrimaryKeyId",
                table: "AspNetUsers",
                column: "PrimaryKeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClothingItems_AspNetUsers_UserId",
                table: "ClothingItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "PrimaryKeyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
