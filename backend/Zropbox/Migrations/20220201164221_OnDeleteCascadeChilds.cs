using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zropbox.Migrations
{
    public partial class OnDeleteCascadeChilds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CDNEntries_CDNEntries_ParentId",
                table: "CDNEntries");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CDNEntries_CDNEntries_ParentId",
                table: "CDNEntries",
                column: "ParentId",
                principalTable: "CDNEntries",
                principalColumn: "Id");
        }
    }
}
