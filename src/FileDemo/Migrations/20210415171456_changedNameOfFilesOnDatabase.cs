using Microsoft.EntityFrameworkCore.Migrations;

namespace FileDemo.Migrations
{
    public partial class changedNameOfFilesOnDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FilesOnDatabaseModel",
                table: "FilesOnDatabaseModel");

            migrationBuilder.RenameTable(
                name: "FilesOnDatabaseModel",
                newName: "FilesOnDatabase");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilesOnDatabase",
                table: "FilesOnDatabase",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FilesOnDatabase",
                table: "FilesOnDatabase");

            migrationBuilder.RenameTable(
                name: "FilesOnDatabase",
                newName: "FilesOnDatabaseModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilesOnDatabaseModel",
                table: "FilesOnDatabaseModel",
                column: "Id");
        }
    }
}
