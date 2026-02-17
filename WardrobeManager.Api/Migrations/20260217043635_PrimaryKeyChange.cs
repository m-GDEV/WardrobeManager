using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WardrobeManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class PrimaryKeyChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_AppUserId",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ClothingItems_Users_UserId",
                table: "ClothingItems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Logs",
                newName: "PrimaryKeyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ClothingItems",
                newName: "PrimaryKeyId");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "AspNetRoles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoles_AppUserId",
                table: "AspNetRoles",
                newName: "IX_AspNetRoles_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PrimaryKeyId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureBase64",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_PrimaryKeyId",
                table: "AspNetUsers",
                column: "PrimaryKeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_UserId",
                table: "AspNetRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClothingItems_AspNetUsers_UserId",
                table: "ClothingItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "PrimaryKeyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_UserId",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ClothingItems_AspNetUsers_UserId",
                table: "ClothingItems");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_PrimaryKeyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrimaryKeyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePictureBase64",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PrimaryKeyId",
                table: "Logs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PrimaryKeyId",
                table: "ClothingItems",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AspNetRoles",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoles_UserId",
                table: "AspNetRoles",
                newName: "IX_AspNetRoles_AppUserId");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Auth0Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ProfilePictureBase64 = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_AppUserId",
                table: "AspNetRoles",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClothingItems_Users_UserId",
                table: "ClothingItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
