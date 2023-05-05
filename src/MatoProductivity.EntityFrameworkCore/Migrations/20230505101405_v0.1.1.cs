using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatoProductivity.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class v011 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "NoteTemplate",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImage",
                table: "NoteTemplate",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "NoteTemplate",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "NoteTemplate",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "NoteTemplate",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEditable",
                table: "NoteTemplate",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PreViewContent",
                table: "NoteTemplate",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "NoteTemplate",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "NoteTemplate",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "Note",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImage",
                table: "Note",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Note",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "Note",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Note",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEditable",
                table: "Note",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PreViewContent",
                table: "Note",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Note",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Note",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "BackgroundImage",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "Desc",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "IsEditable",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "PreViewContent",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "BackgroundImage",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "Desc",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "IsEditable",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "PreViewContent",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Note");
        }
    }
}
