using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zropbox.Migrations
{
    public partial class AddParentReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "CDNEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CDNEntries_ParentId",
                table: "CDNEntries",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CDNEntries_CDNEntries_ParentId",
                table: "CDNEntries",
                column: "ParentId",
                principalTable: "CDNEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CDNEntries_CDNEntries_ParentId",
                table: "CDNEntries");

            migrationBuilder.DropIndex(
                name: "IX_CDNEntries_ParentId",
                table: "CDNEntries");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "CDNEntries");
        }
    }
}
