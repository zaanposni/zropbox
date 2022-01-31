using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDN.Migrations
{
    public partial class MakeParentNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CDNEntries_CDNEntries_ParentId",
                table: "CDNEntries");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "CDNEntries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CDNEntries_CDNEntries_ParentId",
                table: "CDNEntries",
                column: "ParentId",
                principalTable: "CDNEntries",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CDNEntries_CDNEntries_ParentId",
                table: "CDNEntries");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "CDNEntries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CDNEntries_CDNEntries_ParentId",
                table: "CDNEntries",
                column: "ParentId",
                principalTable: "CDNEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
