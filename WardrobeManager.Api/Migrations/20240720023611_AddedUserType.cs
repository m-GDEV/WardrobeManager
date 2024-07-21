using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WardrobeManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "ClothingItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Auth0Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClothingItems_OwnerId",
                table: "ClothingItems",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClothingItems_Users_OwnerId",
                table: "ClothingItems",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClothingItems_Users_OwnerId",
                table: "ClothingItems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ClothingItems_OwnerId",
                table: "ClothingItems");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ClothingItems");
        }
    }
}
