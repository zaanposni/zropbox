using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zropbox.Migrations
{
    public partial class RenameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CDNTEmpEntries_CDNEntries_EntryId",
                table: "CDNTEmpEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CDNTEmpEntries",
                table: "CDNTEmpEntries");

            migrationBuilder.RenameTable(
                name: "CDNTEmpEntries",
                newName: "CDNTempEntries");

            migrationBuilder.RenameIndex(
                name: "IX_CDNTEmpEntries_EntryId",
                table: "CDNTempEntries",
                newName: "IX_CDNTempEntries_EntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CDNTempEntries",
                table: "CDNTempEntries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CDNTempEntries_CDNEntries_EntryId",
                table: "CDNTempEntries",
                column: "EntryId",
                principalTable: "CDNEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CDNTempEntries_CDNEntries_EntryId",
                table: "CDNTempEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CDNTempEntries",
                table: "CDNTempEntries");

            migrationBuilder.RenameTable(
                name: "CDNTempEntries",
                newName: "CDNTEmpEntries");

            migrationBuilder.RenameIndex(
                name: "IX_CDNTempEntries_EntryId",
                table: "CDNTEmpEntries",
                newName: "IX_CDNTEmpEntries_EntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CDNTEmpEntries",
                table: "CDNTEmpEntries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CDNTEmpEntries_CDNEntries_EntryId",
                table: "CDNTEmpEntries",
                column: "EntryId",
                principalTable: "CDNEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
