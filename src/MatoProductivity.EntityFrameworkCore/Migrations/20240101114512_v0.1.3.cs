using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatoProductivity.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class v013 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanSimplified",
                table: "NoteTemplate",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanSimplified",
                table: "Note",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanSimplified",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "CanSimplified",
                table: "Note");
        }
    }
}
