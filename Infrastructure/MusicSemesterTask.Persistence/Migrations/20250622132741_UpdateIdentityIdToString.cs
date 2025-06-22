using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicSemesterTask.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentityIdToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdentityId",
                table: "ApplicationUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdentityId",
                table: "ApplicationUsers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
